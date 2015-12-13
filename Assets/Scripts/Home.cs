using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{ 
    public class Home : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            var go = other.gameObject;
            if (other.attachedRigidbody != null) go = other.attachedRigidbody.gameObject;

            var ship = go.GetComponent<TransportAIController>();

            if (ship != null)
            {
                ship.EnteredHome(this);
            }
        }
    }
}