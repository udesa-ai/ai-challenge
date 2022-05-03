using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Pentalties : MonoBehaviour
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

    [SerializeField] Color homeJersey;
    [SerializeField] Color awayJersey;
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
        ball.Initialize(OnGoal, OnSave);
        StartCoroutine(WaitAndTakeShot());
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
        
        if (shots % 6 == 0)
        {
            homeScoreBoard.Reset();
            awayScoreBoard.Reset();
        }
        
        SwitchTeams();
        
        StartCoroutine(WaitAndTakeShot());
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
        
        Debug.Log($"Goal!!! Home-{scoreHome} Away-{scoreAway}");
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
