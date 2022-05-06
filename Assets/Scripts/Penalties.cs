using System.Collections;
using Core.Core.Manager;
using Core.Games;
using Core.Games.Models;
using Core.TeamSelector;
using Core.Utils;
using DefaultNamespace;
using Presentation.FinishGame;
using UnityEngine;
using UnityEngine.UI;

public class Penalties : MonoBehaviour
{
    [SerializeField] Transform[] targets;
    [SerializeField] Animator keeperAnimator;
    [SerializeField] Transform penaltySpot;
    [SerializeField] Collider goalCollider;
    [SerializeField] PenaltyBall ball;
    
    [SerializeField] float aimRandomness;
    [SerializeField] float kickForceMin;
    [SerializeField] float kickForceMax;
    [SerializeField] float resetTime;
    [SerializeField] float shotTime;

    [SerializeField] private FinishGameView FinishGameView;
    [SerializeField] private WebHelper webHelper;

    Team homeTeam;
    Team awayTeam;
    
    [SerializeField] TeamPlayer kicker;
    [SerializeField] TeamPlayer keeper;

    [SerializeField] PenaltyScoreBoard homeScoreBoard;
    [SerializeField] PenaltyScoreBoard awayScoreBoard;
    [SerializeField] Text shotCounter;

    [SerializeField] PenaltiesAudio audio;

    int scoreHome;
    int scoreAway;
    int shots;
    bool homeToKick = true;
    bool scored;
    
    bool readyToShoot = true;

    void Start()
    {
        LoadTeams();
        ball.Initialize(OnGoal, OnSave);
        StartCoroutine(WaitAndTakeShot());
    }

    void LoadTeams()
    {
        homeTeam = TeamPersistence.Home;
        awayTeam = TeamPersistence.Away;
        homeScoreBoard.SetTeamInfo(homeTeam);
        awayScoreBoard.SetTeamInfo(awayTeam);
        
        kicker.ChangeTeam(homeTeam.PrimaryColor);
        keeper.ChangeTeam(awayTeam.PrimaryColor);
    }

    IEnumerator WaitAndTakeShot()
    {
        yield return new WaitForSeconds(shotTime);
        KickPenalty();
    }

    void OnSave()
    {
        Debug.Log("What a save!");
        audio.PlayCheer();
    }

    void KickPenalty()
    {
        KickBall();
        KeeperDive();
        readyToShoot = false;
        shots++;
        shotCounter.text = shots.ToString();
        audio.PlayKick();
        StartCoroutine(WaitAndReset());
    }

    void KeeperDive()
    {
        string diveTrigger;
        if (homeToKick)
            diveTrigger = GetTargetTrigger(awayTeam.PenaltyKickPreference.GetTarget());
        else
            diveTrigger = GetTargetTrigger(homeTeam.PenaltyKickPreference.GetTarget());
        keeperAnimator.SetTrigger(diveTrigger);
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(resetTime);
        Reset();
    }

    void Reset()
    {
        ResolveMiss();
        ball.Reset(penaltySpot.transform.position);
        readyToShoot = true;
        goalCollider.enabled = true;
        scored = false;

        if (PenaltiesCompleted())
        {
            FinishGameView.Initialize(scoreHome, scoreAway);
            if (MainManager.Instance != null && MainManager.Instance.isTournament)
                webHelper.Put(
                    $"https://{PlayerPrefs.GetString("username")}:{PlayerPrefs.GetString("api_key")}@api.challonge.com/v1/tournaments/{MainManager.Instance.SelectedTournament.Url}/matches/{MainManager.Instance.CurrentMatch.Id}.json",
                    MatchResultBody(), OnComplete);
            Debug.Log($"Penalty round ended, Home-{scoreHome} Away-{scoreAway}");
        }
        else
        {
            ResetPips();
            SwitchTeams();
            StartCoroutine(WaitAndTakeShot());
        }
    }
    
    private void OnComplete(string obj)
    {
        Debug.Log(obj);
    }

    private object MatchResultBody()
    {
        return new Root
        {
            Match = new Match
            {
                ScoresCsv = $"{scoreHome}-{scoreAway}",
                WinnerId = GetWinnerId()
            }
        };
    }
    
    private int GetWinnerId() => scoreHome > scoreAway ? MainManager.Instance.HomeTeam.Id : MainManager.Instance.AwayTeam.Id;


    bool PenaltiesCompleted()
    {
        if (scoreHome != scoreAway)
            return shots == 6 || (shots > 6 && shots % 2 == 0);
        return false;
    }

    void ResetPips()
    {
        if (shots % 6 == 0)
        {
            homeScoreBoard.Reset();
            awayScoreBoard.Reset();
        }
    }

    void ResolveMiss()
    {
        if (scored) return;
        if (homeToKick)
            homeScoreBoard.Miss();
        else
            awayScoreBoard.Miss();
    }

    void SwitchTeams()
    {
        homeToKick = !homeToKick;
        if (homeToKick)
        {
            kicker.ChangeTeam(homeTeam.PrimaryColor);
            keeper.ChangeTeam(awayTeam.PrimaryColor);
        }
        else
        {
            kicker.ChangeTeam(awayTeam.PrimaryColor);
            keeper.ChangeTeam(homeTeam.PrimaryColor);
        }
    }

    void OnGoal()
    {
        goalCollider.enabled = false;
        if (homeToKick)
            ScoreHome();
        else
            ScoreAway();
        
        audio.PlayCheer();
    }

    void ScoreAway()
    {
        scoreAway++;
        awayScoreBoard.Score();
        scored = true;
    }

    void ScoreHome()
    {
        scoreHome++;
        homeScoreBoard.Score();
        scored = true;
    }

    Vector3 RandomTarget() =>
        targets.PickOne().position;

    void KickBall()
    {
        Vector3 target;
        if (homeToKick)
            target = GetTargetPosition(homeTeam.PenaltyKickPreference.GetTarget());
        else
            target = GetTargetPosition(awayTeam.PenaltyKickPreference.GetTarget());

        var aimedTarget = target + Random.insideUnitSphere * aimRandomness;
        var direction = (aimedTarget - ball.transform.position).normalized;
        ball.ShootTo(direction * Random.Range(kickForceMin, kickForceMax));
    }

    Vector3 GetTargetPosition(PenaltyTarget target)
    {
        switch (target)
        {
            case PenaltyTarget.Right:
                return targets[0].position;
            case PenaltyTarget.Center:
                return targets[1].position;
            case PenaltyTarget.Left:
                return targets[2].position;
        }
        
        return targets[1].position;
    }
    
    string GetTargetTrigger(PenaltyTarget target)
    {
        switch (target)
        {
            case PenaltyTarget.Right:
                return "DiveRight";
            case PenaltyTarget.Center:
                return "None";
            case PenaltyTarget.Left:
                return "DiveLeft";
        }
        
        return "None";
    }
}
