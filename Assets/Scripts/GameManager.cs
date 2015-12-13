using UnityEngine;
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
	}
}