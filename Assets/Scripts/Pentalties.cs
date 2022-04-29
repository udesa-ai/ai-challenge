using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Pentalties : MonoBehaviour
{
    [SerializeField] Rigidbody ball;
    [SerializeField] Transform[] targets;
    [SerializeField] Animator keeperAnimator;
    [SerializeField] Transform penaltySpot;
    
    [SerializeField] float aimRandomness;
    [SerializeField] float kickForceMin;
    [SerializeField] float kickForceMax;
    [SerializeField] float resetTime;
    
    bool readyToShoot = true;
    string[] diveTriggers = new[] {"None", "DiveRight", "DiveLeft"};
    
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
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        ball.transform.position = penaltySpot.transform.position;
        readyToShoot = true;
    }

    Vector3 RandomTarget() =>
        targets.PickOne().position;

    void KickBall(Vector3 target)
    {
        var aimedTarget = target + Random.insideUnitSphere * aimRandomness;
        var direction = (aimedTarget - ball.transform.position).normalized;
        ball.AddForce(direction * Random.Range(kickForceMin, kickForceMax));
    }
    
}
