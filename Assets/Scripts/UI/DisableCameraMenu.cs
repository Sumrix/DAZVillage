using UnityEngine;
using System.Collections;

namespace GameUI
{
    public class DisableCameraMenu :
        MonoBehaviour
    {
        public Camera Camera;

        private void OnEnable()
        {
            Camera.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            Camera.gameObject.SetActive(true);
        }
    }
}