using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IfelseMedia.GuideShip
{
    public class WaterManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject waterPlane;

        [SerializeField]
        private float _waveHeight;
        public static float waveHeight = 0.75f;

        [SerializeField]
        private float _waveTime = 1;
        public static float waveTime = 0.75f;

        [SerializeField]
        private float _waveScale = 2;
        public static float waveScale = 2f;

        void Update()
		{
            waveHeight = _waveHeight;
            waveTime = _waveTime;
            waveScale = _waveScale;
            Shader.SetGlobalFloat("_WaveScale", waveScale);
            Shader.SetGlobalFloat ("_WaveHeight", waveHeight);
			Shader.SetGlobalFloat ("_WaveTime", Time.time * waveTime);
		}

        void LateUpdate()
        {
			if (Camera.main) UpdateCameraCenter(Camera.main);
        }

		void UpdateCameraCenter(Camera cam)
		{
			var k = Mathf.Sin (90 - cam.transform.eulerAngles.x) * cam.transform.position.y;
			var camLookDistance = Mathf.Sqrt (k * k + cam.transform.position.y * cam.transform.position.y);
			var pos = cam.transform.position + cam.transform.forward * camLookDistance * 1.2f;

			pos.y = 0;
			pos.x = Mathf.Floor (pos.x * 0.2f) * 5;
			pos.z = Mathf.Floor (pos.z * 0.2f) * 5;
			transform.position = pos;

		}

		public static float GetWaveHeight(Vector3 pos)
		{
			return Mathf.Sin(Time.time * waveTime - pos.x * 0.25f * waveScale) * waveHeight;
		}
    }
}