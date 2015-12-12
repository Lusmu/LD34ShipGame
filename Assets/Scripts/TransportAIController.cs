using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IfelseMedia.GuideShip
{
    [RequireComponent(typeof(ShipController))]
    public class TransportAIController : MonoBehaviour
    {
        private ShipController ship;
        private List<Beacon> beacons = new List<Beacon>();

        void Awake()
        {
            ship = GetComponent<ShipController>();
        }

        void Update()
        {
            if (beacons.Count > 0)
            {
                var beacon = beacons[0];
                if (beacon != null)
                {
                    var sqrDistance = (transform.position - beacon.transform.position).sqrMagnitude;
                    if (sqrDistance > 2) ship.Thrust = 1;
                    else ship.Thrust = -0.1f;

                    Vector3 relativePos = beacon.transform.position - transform.position;

                    var deltaRotation = AngleDir(transform.forward, relativePos, Vector3.up);

                    if (deltaRotation < 0) ship.Rudder = -1;
                    else if (deltaRotation > 0) ship.Rudder = 1;
                    else ship.Rudder = 0;
                }
                else
                {
                    beacons.RemoveAt(0);
                }
            }
            else
            {
                ship.Thrust = 0;
            }
        }

        public void EnteredBeaconRange(Beacon beacon)
        {
            if (!beacons.Contains(beacon)) beacons.Add(beacon);
        }

        public void ExitBeaconRange(Beacon beacon)
        {
            if (beacons.Contains(beacon)) beacons.Remove(beacon);
        }

        float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0f)
            {
                return 1f;
            }
            else if (dir < 0f)
            {
                return -1f;
            }
            else
            {
                return 0f;
            }
        }
    }
}