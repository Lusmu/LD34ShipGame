using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IfelseMedia.GuideShip
{
    public class TransportAIController : MonoBehaviour
    {
        public enum TransportState
        {
            Lost,
            Following,
            GoingHome
        }

        private ShipController ship;
        private List<Beacon> beacons = new List<Beacon>();

        public TransportState CurrentState { get; private set; }

        [SerializeField]
		private ParticleSystem distressTorch;
		[SerializeField]
		private ParticleSystem happyTorch;
		[SerializeField]
		private ParticleSystem distressFlare;
        [SerializeField]
        private ParticleSystem happyFlare;

        [SerializeField]
        private int score = 1;

        public void EnteredHome(Home home)
        {
            if (CurrentState != TransportState.GoingHome)
            {
                StartCoroutine(EnteredHome_Coroutine());
            }
        }

        void OnEnabled()
        {
            beacons = new List<Beacon>();
        }

        IEnumerator EnteredHome_Coroutine()
        {
            CurrentState = TransportState.GoingHome;

            GameManager.Instace.AddScore(score);

            if (happyFlare) happyFlare.Play(true);

            var despawner = GetComponent<Despawnable>();
            if (despawner != null) despawner.enabled = false;

            ship.Physics.isKinematic = true;
            ship.Physics.Sleep();

            var euler = transform.eulerAngles;
            euler.x = 90;
            transform.eulerAngles = euler;

            while (transform.position.y < 50)
            {
                transform.position += Vector3.up * Time.deltaTime * 10;
                transform.eulerAngles += Vector3.up * Time.deltaTime * 180;
                yield return null;
            }

            gameObject.SetActive(false);

            if (despawner != null) despawner.enabled = true;
        }

        void Awake()
        {
            ship = GetComponent<ShipController>();
        }

        void Update()
        {
			if (!ship || CurrentState == TransportState.GoingHome) return;

			if (ship.IsSinking) 
			{
				if (distressTorch) distressTorch.Stop (true);
				if (happyTorch) happyTorch.Stop (true);
			}
			else 
			{
				if (beacons.Count > 0)
				{
					var beacon = beacons[beacons.Count - 1];
					if (beacon != null)
					{
						Vector3 relativePos = beacon.transform.position - transform.position;

						var deltaRotation = AngleDir(transform.forward, relativePos, Vector3.up);

						if (deltaRotation < 0) ship.Rudder = -1;
						else if (deltaRotation > 0) ship.Rudder = 1;
						else ship.Rudder = 0;

						var sqrDistance = (transform.position - beacon.transform.position).sqrMagnitude;
						if (sqrDistance > 40) 
						{
							ship.Thrust = 1;
						}
						else if (sqrDistance > 20) 
						{
							ship.Thrust = (sqrDistance - 20) / 20;
						}
						else 
						{
							ship.Thrust = -0.1f;
						}
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
            
        }

        public void EnteredBeaconRange(Beacon beacon)
        {
			if (ship.IsSinking) return;

            if (happyFlare) happyFlare.Play(true);

            if (beacons.Count == 0) 
			{
				if (distressTorch) distressTorch.Stop (true);
				if (happyTorch) happyTorch.Play (true);
			}

			if (!beacons.Contains (beacon)) 
			{
				Debug.Log ("Ship found beacon", gameObject);
				beacons.Add (beacon);
			}
        }

        public void ExitBeaconRange(Beacon beacon)
        {
			if (ship.IsSinking) return;

			if (beacons.Count == 1) 
			{
				if (distressFlare) distressFlare.Play (true);
				if (distressTorch) distressTorch.Play (true);
				if (happyTorch) happyTorch.Stop (true);

				Debug.Log ("Ship lost beacon", gameObject);
			}

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