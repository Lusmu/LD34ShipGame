using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
	[RequireComponent(typeof(Rigidbody))]
	public class Boyancy : MonoBehaviour 
	{
		public float strength = 1.1f;

        public Vector3 wind;

        private Rigidbody physics;

		void Awake()
		{
			physics = GetComponent<Rigidbody> ();
		}

		void FixedUpdate () 
		{
			var waterLevel = WaterManager.GetWaveHeight (transform.position);

			var pos = transform.position;
			var diff = Mathf.Clamp01(waterLevel - pos.y);
			if (diff > 0)
			{
				physics.AddForce(wind + Vector3.up * 2000 * diff * diff * Time.deltaTime, ForceMode.Acceleration);
			}
		}

		void LateUpdate2()
		{
			var pos = transform.position;
			pos.y = WaterManager.GetWaveHeight (transform.position);
			transform.position = pos;
		}
	}
}