using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace IfelseMedia.GuideShip
{
	public class GameManager : MonoBehaviour 
	{
		private static GameManager instance;
		public static GameManager Instace 
		{
			get 
			{
				if (instance == null) 
				{
					instance = FindObjectOfType<GameManager> ();
				}
				return instance;
			}
		}

		public Transform despawner;

        public PlayerController Player;
        [SerializeField]
        private UnityEngine.UI.Text scoreLabel;

		void Awake()
		{
			instance = this;
		}

        void OnEnable()
        {
            ShipController.OnShipSink += ShipController_OnShipSink;
        }

        void OnDisable()
        {
            ShipController.OnShipSink -= ShipController_OnShipSink;
        }

        private void ShipController_OnShipSink(ShipController ship)
        {
            if (ship == Player.ship)
            {
                MessageManager.Instance.ShowMessage("S.O.S.");
                MessageManager.Instance.ShowMessage("Game Over");
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void AddScore(int amount)
        {
            Player.Score += amount;
            scoreLabel.text = Player.Score.ToString();
            var tween = LeanTween.scale(scoreLabel.gameObject, Vector3.one, 0.5f);
            tween.setEase(LeanTweenType.punch);

            Player.SetBeaconStage(1 + Mathf.CeilToInt(Player.Score * 0.1f));
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit();
        }
	}
}