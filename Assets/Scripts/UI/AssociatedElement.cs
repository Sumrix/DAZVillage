using System;
using UnityEngine;

namespace GameUI
{
    public class AssociatedElement :
        MonoBehaviour,
        IManagerWaiter
    {
        [SerializeField]
        protected GameObject _sceneObject;
        [SerializeField]
        private Vector3 _sceneOffset;
        private RectTransform _canvasRect;
        private RectTransform _uiElement;

        private void Awake()
        {
            _uiElement = GetComponent<RectTransform>();
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            _canvasRect = Managers.UI.Canvas.GetComponent<RectTransform>();
            UpdatePosition();
        }
        protected void LateUpdate()
        {
            UpdatePosition();
        }
        private void UpdatePosition()
        {
            if (_sceneObject)
            {
                Vector3 worldPosition = _sceneObject.transform.position + _sceneOffset;
                Vector2 viewportPosition = Managers.UI.Camera.WorldToViewportPoint(worldPosition);
                Vector2 screenPosition = new Vector2(
                    ((viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
                    ((viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));
                _uiElement.anchoredPosition = screenPosition;
            }
        }
    }
}