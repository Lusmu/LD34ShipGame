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
        private float maxThrustBackward = 5;

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

        private Rigidbody physics;

		public bool IsSinking { get; private set; }

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

        void Start()
        {
            physics = GetComponent<Rigidbody>();
        }

        void Update()
        {
            RockShip();
        }

        void FixedUpdate()
        {
			if (IsSinking) return;

            var appliedRudder = Rudder * (1 - (speedForMaxRudder - physics.velocity.magnitude) / speedForMaxRudder) * maxRudder;

            physics.drag = TurnDependentDrag(appliedRudder);

            var localEuler = transform.localEulerAngles;
            localEuler.x = 0;
            localEuler.z = 0;
            transform.localEulerAngles = localEuler;

            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * appliedRudder * Time.deltaTime);
            physics.MoveRotation(physics.rotation * deltaRotation);

            physics.AddRelativeForce(thrust * Vector3.forward * maxThrustForward, ForceMode.Force);
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

			if (newDamage > armor) 
			{
				damage += newDamage - armor;
				Debug.Log ("Damage: " + damage, gameObject);
				if (damage >= hitPoins) 
				{
					Sink ();
				}
			}
		}

		public void Sink()
		{
			if (IsSinking) return;
		
			Debug.Log ("Ship sunk", gameObject);

			StartCoroutine (Sink_Coroutine ());
		}

		IEnumerator Sink_Coroutine()
		{
			IsSinking = true;

			physics.freezeRotation = false;
			physics.constraints = RigidbodyConstraints.None;

			yield return new WaitForSeconds (2);

			physics.drag = 10;

			physics.useGravity = true;

			Destroy (this);
		}
    }
}