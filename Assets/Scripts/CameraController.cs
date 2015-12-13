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
        private float turnSpeed = 30;

        [SerializeField]
        private float prediction = 5;

        void LateUpdate()
        {
            if (target != null)
            {
                var targetPosition = target.position - target.forward * offset.z;
				targetPosition.y = offset.y;
                var targetLookPosition = target.position;

                var physics = target.GetComponent<Rigidbody>();
                if (physics != null)
                {
                    targetPosition -= physics.velocity * prediction;
                    targetLookPosition += physics.velocity * prediction;
                }

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

                var lookRotation = Quaternion.LookRotation(targetLookPosition - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            }
        }
    }
}