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

        [SerializeField]
        private PlayerController Player;
        [SerializeField]
        private UnityEngine.UI.Text scoreLabel;

		void Awake()
		{
			instance = this;
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
        }
	}
}