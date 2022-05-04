using System.Collections;
using Core.TeamSelector;
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

    Color homeJersey;
    Color awayJersey;
    [SerializeField] TeamPlayer kicker;
    [SerializeField] TeamPlayer keeper;

    [SerializeField] PenaltyScoreBoard homeScoreBoard;
    [SerializeField] PenaltyScoreBoard awayScoreBoard;
    [SerializeField] Text shotCounter;

    int scoreHome;
    int scoreAway;
    int shots;
    bool homeToKick = true;
    bool scored;
    
    bool readyToShoot = true;
    string[] diveTriggers = new[] {"None", "DiveRight", "DiveLeft"};

    void Start()
    {
        LoadTeams();
        ball.Initialize(OnGoal, OnSave);
        StartCoroutine(WaitAndTakeShot());
    }

    void LoadTeams()
    {
        homeJersey = TeamPersistence.Home.PrimaryColor;
        awayJersey = TeamPersistence.Away.PrimaryColor;
        homeScoreBoard.SetTeamInfo(TeamPersistence.Home);
        awayScoreBoard.SetTeamInfo(TeamPersistence.Away);
        
        kicker.ChangeTeam(homeJersey);
        keeper.ChangeTeam(awayJersey);
    }

    IEnumerator WaitAndTakeShot()
    {
        yield return new WaitForSeconds(shotTime);
        KickPenalty();
    }

    void OnSave()
    {
        Debug.Log("What a save!");
    }

    void KickPenalty()
    {
        KickBall(RandomTarget());
        keeperAnimator.SetTrigger(diveTriggers.PickOne());
        readyToShoot = false;
        shots++;
        shotCounter.text = shots.ToString();
        StartCoroutine(WaitAndReset());
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
            Debug.Log($"Penalty round ended, Home-{scoreHome} Away-{scoreAway}");
        }
        else
        {
            ResetPips();
            SwitchTeams();
            StartCoroutine(WaitAndTakeShot());
        }
    }

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
            kicker.ChangeTeam(homeJersey);
            keeper.ChangeTeam(awayJersey);
        }
        else
        {
            kicker.ChangeTeam(awayJersey);
            keeper.ChangeTeam(homeJersey);
        }
    }

    void OnGoal()
    {
        goalCollider.enabled = false;
        if (homeToKick)
            ScoreHome();
        else
            ScoreAway();
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

    void KickBall(Vector3 target)
    {
        var aimedTarget = target + Random.insideUnitSphere * aimRandomness;
        var direction = (aimedTarget - ball.transform.position).normalized;
        ball.ShootTo(direction * Random.Range(kickForceMin, kickForceMax));
    }
    
}
