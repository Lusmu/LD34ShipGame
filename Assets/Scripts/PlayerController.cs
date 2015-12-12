using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
    public class PlayerController : MonoBehaviour
    {
        public ShipController ship;

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
        }
    }
}
