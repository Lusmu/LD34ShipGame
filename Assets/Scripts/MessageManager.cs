using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IfelseMedia.GuideShip
{
    public class MessageManager : MonoBehaviour
    {
        public static MessageManager Instance { get; private set; }

        [SerializeField]
        private UnityEngine.UI.Text messageLabel;
        [SerializeField]
        private RectTransform messagePanel;

        private Queue<string> messages = new Queue<string>();

        private float messageHideTime;

		private string currentMessage = null;

        void Awake()
        {
            Instance = this;
        }

        public void ShowMessage(string message, float delay = 0)
        {
			if (!messages.Contains (message)) 
			{
				StartCoroutine (ShowMessageDelayed_Coroutine (message, delay));
			}
        }

		IEnumerator ShowMessageDelayed_Coroutine(string message, float delay = 0)
		{
			yield return new WaitForSeconds (delay);

			if (!messages.Contains(message)) messages.Enqueue(message);
		}

        private void ShowTopMessage(float delay = 0)
        {    
			if (currentMessage == null && messages.Count > 0)
            {
				SoundManager.Instance.PlayEffect (SoundEffect.BlibSoft);
                messagePanel.gameObject.SetActive(true);

                currentMessage = messages.Dequeue();
                messageLabel.text = currentMessage;
                messageHideTime = Time.time + 6;

                var pos = messagePanel.position;
                pos.y = -200;
                messagePanel.position = pos;
                var tween = LeanTween.moveY(messagePanel.gameObject, 0, 0.5f);
                tween.delay = delay;
                tween.setEase(LeanTweenType.easeOutElastic);
            }
        }

        private void HideMessage(bool animated = false)
        {
			if (currentMessage == null) return;

            currentMessage = null;

            if (animated)
            {
                var tween = LeanTween.moveY(messagePanel.gameObject, -200, 0.35f);
                tween.setEase(LeanTweenType.easeInCirc);
                tween.onComplete = () => { messagePanel.gameObject.SetActive(false); };
            }
            else
            {
                messagePanel.gameObject.SetActive(false);
            }
        }

	    void Update ()
        {
	        if (Time.time > messageHideTime)
            {
				HideMessage();
            }
			if (currentMessage == null) 
			{
				ShowTopMessage ();
			}
	    }
    }
}
