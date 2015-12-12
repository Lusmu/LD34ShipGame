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

        }

        void Update()
        {
            ship.Rudder = Input.GetAxis("Horizontal");
        }
    }
}
