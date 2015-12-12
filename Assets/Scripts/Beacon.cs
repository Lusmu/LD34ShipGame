using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
    [RequireComponent(typeof(Collider))]
    public class Beacon : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            var go = other.gameObject;
            if (other.attachedRigidbody != null) go = other.attachedRigidbody.gameObject;

            var ship = go.GetComponent<TransportAIController>();

            if (ship != null)
            {
                ship.EnteredBeaconRange(this);
            }
        }

        void OnTriggerExit(Collider other)
        {
            var go = other.gameObject;
            if (other.attachedRigidbody != null) go = other.attachedRigidbody.gameObject;

            var ship = go.GetComponent<TransportAIController>();

            if (ship != null)
            {
                ship.ExitBeaconRange(this);
            }
        }
    }
}