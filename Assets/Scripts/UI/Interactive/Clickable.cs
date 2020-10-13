using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace GameUI
{
    public class Clickable :
        MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler
    {
        public event EventHandler Click;
        public event EventHandler LongClick;
        public event EventHandler DoubleClick;
        public event EventHandler ButtonDown;
        public event EventHandler ButtonUp;
        
        [HideInInspector]
        public bool IsActive;
        public bool IsPressed { get; private set; }

        private static readonly float _longClickTime = 1;
        private static readonly float _doubleClickTime = 3;
        private float _clickTime;
        private float _pointerDownTime;
        private static Clickable _clickedObject;

        public Clickable()
        {
            IsActive = true;
            IsPressed = false;
            _clickTime = -_doubleClickTime;
            _pointerDownTime = 0;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            OnButtonDown();
            _pointerDownTime = Time.realtimeSinceStartup;
            Invoke("OnLongClicked", _longClickTime);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            OnButtonUp();
            if (!eventData.dragging)
            {
                if (Time.realtimeSinceStartup - _pointerDownTime < _longClickTime)
                {
                    CancelInvoke("OnLongClicked");
                    if (Time.realtimeSinceStartup - _clickTime < _doubleClickTime && _clickedObject == this)
                    {
                        _clickTime = 0;
                        _clickedObject = this;
                        OnClicked();
                        OnDoubleClicked();
                    }
                    else
                    {
                        _clickTime = Time.realtimeSinceStartup;
                        _clickedObject = this;
                        OnClicked();
                    }
                }
            }
        }
        public void OnDrag(PointerEventData data)
        {
            CancelInvoke("OnLongClicked");
        }
        private void OnClicked()
        {
            if (IsActive)
            {
                //this.Log("Click");
                var handler = Click;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        private void OnLongClicked()
        {
            if (IsActive)
            {
                //this.Log("LongClick");
                var handler = LongClick;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        private void OnDoubleClicked()
        {
            if (IsActive)
            {
                //this.Log("DoubleClick");
                var handler = DoubleClick;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        private void OnButtonDown()
        {
            if (IsActive)
            {
                //this.Log("DoubleClick");
                var handler = ButtonDown;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        private void OnButtonUp()
        {
            if (IsActive)
            {
                //this.Log("DoubleClick");
                var handler = ButtonUp;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
    }
}