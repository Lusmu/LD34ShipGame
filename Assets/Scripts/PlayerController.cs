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
            ship.Rudder = Input.GetAxis("Horizontal");
        }
    }
}
