using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset;

        [SerializeField]
        private float moveSpeed = 10;

        [SerializeField]
        private float prediction = 5;

        void Update()
        {
            if (target != null)
            {
                var targetPosition = target.position + offset;

                var physics = target.GetComponent<Rigidbody>();
                if (physics != null)
                {
                    targetPosition += physics.velocity * prediction; 
                }

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            }
        }
    }
}