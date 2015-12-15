using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
    public class PlayerController : MonoBehaviour
    {
        public ShipController ship;

        public Transform harbour;

        public ParticleSystem homeFlare;

        public ParticleSystem levelUpParticles;

        public int Score { get; set; }

        float lastFiredHomeFlare = 0;

        int currentBeaconStage = 1;

		float homeFlareDistance = 70;

		static bool homeFlareHintGiven = false;

        [SerializeField]
        private Beacon beacon;

        // Use this for initialization
        void Start()
        {
            ship.Thrust = 1;
        }

        void Update()
        {
			if (GameManager.Instace.CurrentState != GameState.Playing) return;

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
				if (distSqr > homeFlareDistance * homeFlareDistance && Time.time > lastFiredHomeFlare + 10)
                {
                    lastFiredHomeFlare = Time.time;
                    homeFlare.Play();

					if (!homeFlareHintGiven) 
					{
						homeFlareHintGiven = true;
						MessageManager.Instance.ShowMessage ("Lost? Follow the yellow flares.");
					}
                }
            }
        }

        public void SetBeaconStage(int stage)
        {
            if (stage <= currentBeaconStage) return;

            currentBeaconStage = stage;

            bool updated = false;
            switch (stage)
            {
                case 2:
                    updated = true;
                    beacon.GetComponent<Light>().range = 15;
                    beacon.GetComponent<Light>().intensity = 1f;
                    beacon.GetComponent<SphereCollider>().radius = 8;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 1.2f;
                    break;
                case 3:
                    updated = true;
                    beacon.GetComponent<Light>().range = 20;
                    beacon.GetComponent<Light>().intensity = 1.2f;
                    beacon.GetComponent<SphereCollider>().radius = 11;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 1.4f;
                    break;
                case 4:
                    updated = true;
                    beacon.GetComponent<Light>().range = 30;
                    beacon.GetComponent<SphereCollider>().radius = 13;
                    beacon.GetComponent<Light>().intensity = 1.3f;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 2f;
                    break;
                case 5:
                    updated = true;
                    beacon.GetComponent<Light>().range = 36;
                    beacon.GetComponent<SphereCollider>().radius = 15;
                    beacon.GetComponent<Light>().intensity = 1.5f;
                    beacon.GetComponentInChildren<ParticleSystem>().startSize = 2.5f;
                    break;
            }

            if (updated)
            {
				SoundManager.Instance.PlayEffect (SoundEffect.Levelup);
                MessageManager.Instance.ShowMessage("Guiding Light Upgraded to Level " + stage);
                levelUpParticles.Play();
            }
        }
    }
}
