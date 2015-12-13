using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
    public class PlayerController : MonoBehaviour
    {
        public ShipController ship;

        public Transform harbour;

        public ParticleSystem homeFlare;

        public int Score { get; set; }

        float lastFiredHomeFlare = 0;

        // Use this for initialization
        void Start()
        {
            ship.Thrust = 1;
        }

        void Update()
        {
			if (Input.touchCount == 1) 
			{
				ship.Rudder = Input.touches [0].position.x < Screen.width * 0.5f ? -1 : 1;
			}
			else 
			{
				ship.Rudder = Input.GetAxis("Horizontal");
			}

            if (ship.isActiveAndEnabled && !ship.IsSinking && harbour && homeFlare)
            {
                var distSqr = (ship.transform.position - harbour.position).sqrMagnitude;
                if (distSqr > 100 * 100 && Time.time > lastFiredHomeFlare + 10)
                {
                    lastFiredHomeFlare = Time.time;
                    homeFlare.Play();
                }
            }
        }
    }
}
