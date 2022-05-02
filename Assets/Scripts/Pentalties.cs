using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

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

    int scoreHome;
    int scoreAway;
    bool homeToKick = true;
    
    bool readyToShoot = true;
    string[] diveTriggers = new[] {"None", "DiveRight", "DiveLeft"};

    void Start() => 
        ball.Initialize(OnGoal, OnSave);

    void OnSave()
    {
        Debug.Log("What a save!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) & readyToShoot)
            KickPenalty();
    }

    void KickPenalty()
    {
        KickBall(RandomTarget());
        keeperAnimator.SetTrigger(diveTriggers.PickOne());
        readyToShoot = false;
        StartCoroutine(WaitAndReset());
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(resetTime);
        Reset();
    }

    void Reset()
    {
        ball.Reset(penaltySpot.transform.position);
        readyToShoot = true;
        goalCollider.enabled = true;
        SwitchTeams();
    }

    void SwitchTeams()
    {
        homeToKick = !homeToKick;
    }

    void OnGoal()
    {
        goalCollider.enabled = false;
        if (homeToKick)
            scoreHome++;
        else
            scoreAway++;
        
        Debug.Log($"Goal!!! Home-{scoreHome} Away-{scoreAway}");
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
