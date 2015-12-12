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
        private float textureScale = 1;

        void LateUpdate()
        {
            //OnUpdateScreenSize();
			if (Camera.main) UpdateCameraCenter(Camera.main);
        }

		void UpdateCameraCenter(Camera cam)
		{
			var k = Mathf.Sin (90 - cam.transform.eulerAngles.x) * cam.transform.position.y;
			var camLookDistance = Mathf.Sqrt (k * k + cam.transform.position.y * cam.transform.position.y);
			var pos = cam.transform.position + cam.transform.forward * camLookDistance;


			/*
			var rot = transform.eulerAngles;
			rot.y = cam.transform.eulerAngles.y;
			transform.eulerAngles = rot;*/

			//var pos = cam.transform.position;
			//pos += cam.transform.forward * 5;
			pos.y = 0;
			pos.x = Mathf.Floor (pos.x * 0.2f) * 5;
			pos.z = Mathf.Floor (pos.z * 0.2f) * 5;
			transform.position = pos;

		}

        void OnUpdateScreenSize()
        {
            var ratio = ((float)Screen.width / (float)Screen.height);
            var size = waterPlane.transform.localScale;
            size.x = size.z * ratio;
            waterPlane.transform.localScale = size;
        }
    }
}