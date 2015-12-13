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

		void Awake()
		{
			instance = this;
		}

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}