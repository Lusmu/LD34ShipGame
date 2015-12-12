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

        void Update()
        {
            OnUpdateScreenSize();
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