using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PenaltyBall : MonoBehaviour
    {
        [SerializeField] Rigidbody myBody;
        [SerializeField] Vector3 deflection;
        
        
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                myBody.AddForce(deflection);
                Debug.Log("save!");
            }

        }
        
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("goal"))
                Debug.Log("goal!");
                
        }
    }
}