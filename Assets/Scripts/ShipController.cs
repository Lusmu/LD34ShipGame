using UnityEngine;
using System.Collections;
namespace IfelseMedia.GuideShip
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipController : MonoBehaviour
    {
		[SerializeField]
		private float hitPoins = 10;
		[SerializeField]
		private float armor = 1;

		private float damage;

        [SerializeField]
        private Transform visualsRoot;
        [SerializeField]
        public float rockAmount = 8;

        [SerializeField]
        private float maxThrustForward = 10;

        [SerializeField]
        private float minDrag = 1;
        [SerializeField]
        private float maxDrag = 4;

        [SerializeField]
        private float maxRudder = 10;
        
        private float targetDirection;
        public float Rudder { get; set; }

        [SerializeField]
        private float speedForMaxRudder = 5;

        public Rigidbody Physics { get; set; }

		public bool IsSinking { get; private set; }

		private float stationaryTime = 0;

		private RigidbodyConstraints constraints;

        private float thrust;
        public float Thrust
        {
            get
            {
                return thrust;
            }
            set
            {
                thrust = Mathf.Clamp(value, -1, 1);
            }
        }

		void OnEnable()
		{
			if (!Physics) 
			{
				Physics = GetComponent<Rigidbody> ();
				constraints = Physics.constraints;
			}

            Physics.isKinematic = false;
            Physics.constraints = constraints;
			IsSinking = false;
			Physics.velocity = Vector3.zero;
            var euler = transform.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            transform.eulerAngles = euler;
			Physics.constraints = constraints;
			var pos = transform.position;
			pos.y = 0;
			transform.position = pos;

			var boyancy = GetComponent<Boyancy> ();
			if (boyancy) boyancy.enabled = true;
		}

        void OnDisable()
        {
            Debug.Log("Disabled", gameObject);
        }

        void Update()
        {
            RockShip();
        }

        void FixedUpdate()
        {
			if (IsSinking) 
			{
				if (transform.position.y < -8) gameObject.SetActive (false);
			}
			else 
			{
				var appliedRudder = Rudder * (1 - (speedForMaxRudder - Physics.velocity.magnitude) / speedForMaxRudder) * maxRudder;

				Physics.drag = TurnDependentDrag(appliedRudder);

				var localEuler = transform.localEulerAngles;
				localEuler.x = 0;
				localEuler.z = 0;
				transform.localEulerAngles = localEuler;

				Quaternion deltaRotation = Quaternion.Euler(Vector3.up * appliedRudder * Time.deltaTime);
				Physics.MoveRotation(Physics.rotation * deltaRotation);

				Physics.AddRelativeForce(thrust * Vector3.forward * maxThrustForward, ForceMode.Force);

				if (Thrust > 0.75f && Physics.velocity.sqrMagnitude < 0.1f) 
				{
					stationaryTime += Time.deltaTime;

					if (stationaryTime > 1) TakeDamage (Time.deltaTime * 0.5f, true);
				}
				else 
				{
					stationaryTime = 0;
				}
			}
        }

        float TurnDependentDrag(float turn)
        {
            return minDrag + (maxDrag - minDrag) * (Mathf.Abs(turn) / maxRudder);
        }

        void RockShip()
        {
            if (visualsRoot == null) return;

            var targetRock = rockAmount * (Mathf.Sin(Time.time) + Rudder * 2);

            var rock = Mathf.MoveTowardsAngle(visualsRoot.localEulerAngles.z, targetRock, Time.deltaTime * 30);
            visualsRoot.transform.localEulerAngles = Vector3.forward * rock;
        }

		void OnCollisionEnter(Collision col)
		{
			bool underWaterline = false;
			for (int i = 0; i < col.contacts.Length; i++) 
			{
				if (col.contacts [i].point.y < 0) 
				{
					underWaterline = true;
					break;
				}
			}

			float newDamage = col.impulse.sqrMagnitude;
			if (underWaterline) newDamage *= 2;

			TakeDamage (newDamage);
		}

		public void TakeDamage(float amount, bool ignoreArmor = false)
		{
			if (ignoreArmor || amount > armor) 
			{
				if (!ignoreArmor) amount -= armor;
				damage += amount;

				if (damage >= hitPoins) 
				{
					Sink ();
				}
			}
		}

		public void Sink()
		{
			if (IsSinking) return;
		
			IsSinking = true;

			Debug.Log ("Ship sunk", gameObject);

			StartCoroutine (Sink_Coroutine ());
		}

		IEnumerator Sink_Coroutine()
		{
			Physics.constraints = RigidbodyConstraints.None;

			yield return new WaitForSeconds (2);

			Physics.drag = 10;

			var boyancy = GetComponent<Boyancy> ();
			if (boyancy) boyancy.enabled = false;
		}
    }
}