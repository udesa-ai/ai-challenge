using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PenaltyBall : MonoBehaviour
    {
        [SerializeField] Rigidbody myBody;
        [SerializeField] Vector3 deflection;
        Action onGoal;
        Action onSave;

        public void Initialize(Action onGoal, Action onSave)
        {
            this.onGoal = onGoal;
            this.onSave = onSave;
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                myBody.AddForce(deflection);
                onSave();
            }

        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("goal"))
                onGoal();

        }

        public void Reset(Vector3 spot)
        {
            myBody.velocity = Vector3.zero;
            myBody.angularVelocity = Vector3.zero;
            transform.position = spot;
        }

        public void ShootTo(Vector3 force) => 
            myBody.AddForce(force);
    }
}