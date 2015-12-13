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
                float zOffset = offset.z;
                float yOffset = offset.y;
                float appliedPrediction = prediction;
                if (Screen.height > Screen.width)
                {
                    zOffset *= 0.7f;
                    yOffset *= 0.4f;
                    appliedPrediction *= 0.8f;
                    Camera.main.fieldOfView = 50;
                }
                else
                {
                    Camera.main.fieldOfView = 30;
                }

                var targetPosition = target.position - target.forward * zOffset;
				targetPosition.y = yOffset;
                var targetLookPosition = target.position;

                var physics = target.GetComponent<Rigidbody>();
                if (physics != null)
                {
                    targetPosition -= physics.velocity * appliedPrediction;
                    targetLookPosition += physics.velocity * appliedPrediction;
                }

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

                var lookRotation = Quaternion.LookRotation(targetLookPosition - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            }
        }
    }
}