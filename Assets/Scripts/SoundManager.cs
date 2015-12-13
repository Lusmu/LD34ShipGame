using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
	public enum SoundEffect
	{
		Hurt,
		Boom,
		Levelup,
		Blib,
		BlibSoft
	}

	public class SoundManager : MonoBehaviour 
	{
		public static SoundManager Instance { get; private set; }

		[SerializeField]
		private AudioClip hurt;
		[SerializeField]
		private AudioClip boom;
		[SerializeField]
		private AudioClip levelup;
		[SerializeField]
		private AudioClip blib;
		[SerializeField]
		private AudioClip blibSoft;

		private AudioSource audioSource;

		void Awake()
		{
			Instance = this;
		}

		public void PlayEffect(SoundEffect effect)
		{
			if (audioSource == null) 
			{
				audioSource = gameObject.AddComponent<AudioSource> ();
				audioSource.spatialize = false;
			}
			var clip = GetClipForEffect (effect);
			if (clip != null) 
			{
				audioSource.PlayOneShot (clip);
			}
		}

		public void PlayEffect(SoundEffect effect, Vector3 position)
		{
			var clip = GetClipForEffect (effect);
			if (clip) AudioSource.PlayClipAtPoint (clip, position);
		}

		AudioClip GetClipForEffect(SoundEffect effect)
		{
			AudioClip clip = null;

			switch (effect) 
			{
			case SoundEffect.Blib:
				clip = blib;
				break;
			case SoundEffect.Boom:
				clip = boom;
				break;
			case SoundEffect.Hurt:
				clip = hurt;
				break;
			case SoundEffect.Levelup:
				clip = levelup;
				break;
			case SoundEffect.BlibSoft:
				clip = blibSoft;
				break;
			}

			return clip;
		}
	}
}