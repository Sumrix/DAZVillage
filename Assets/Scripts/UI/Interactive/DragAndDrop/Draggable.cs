using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

namespace GameUI
{
	public class Draggable :
		MonoBehaviour,
		IBeginDragHandler,
		IDragHandler,
		IEndDragHandler
	{
		public event EventHandler<BeginDragEventArgs> BeginDrag;
		public event EventHandler<EndDragEventArgs> EndDrag;

		private bool isDragAllowed = false;
        [HideInInspector]
        public bool IsActive = true;

		private void OnBeginDrag(BeginDragEventArgs e)
		{
			var handler = BeginDrag;
			if (handler != null)
			{
				handler (this, e);
			}
		}
		private void OnEndDrag(EndDragEventArgs e)
		{
			var handler = EndDrag;
			if (handler != null)
			{
				handler (this, e);
			}
		}
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
            if (IsActive)
            {
                var args = new BeginDragEventArgs(true);
                OnBeginDrag(args);
                if (!args.Cancel)
                {
                    isDragAllowed = true;
                    transform.SetParent(Managers.UI.Canvas.transform);
                } else
                {
                    isDragAllowed = false;
                }
            }
		}
		void IDragHandler.OnDrag(PointerEventData data)
		{
            if (IsActive && isDragAllowed)
			{
				transform.position = Input.mousePosition;
			}
		}
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (IsActive)
            {
                var raycastResults = GetObjectsBelowUs();
                var topReceiver = ChooseTopReceiver(raycastResults);

                bool cancel = true;
                GameObject receiverGameObject = null;
                if (topReceiver != null)
                {
                    cancel = !topReceiver.OnDrop(gameObject);
                    if (!cancel)
                    {
                        receiverGameObject = topReceiver.gameObject;
                    }
                }

                OnEndDrag(new EndDragEventArgs(receiverGameObject, cancel));
            }
		}

		private List<RaycastResult> GetObjectsBelowUs ()
		{
			PointerEventData pointer = new PointerEventData (EventSystem.current);
			pointer.position = transform.position;
			List<RaycastResult> raycastResults = new List<RaycastResult> ();
			EventSystem.current.RaycastAll (pointer, raycastResults);
			return raycastResults;
		}

		static Droppable ChooseTopReceiver (List<RaycastResult> raycastResults)
		{
			int minDepth = 0;
			Droppable topReceiver = null;

			foreach (var obj in raycastResults)
			{
				var receiver = obj.gameObject.GetComponent<Droppable> ();
				if (receiver != null && (topReceiver == null || minDepth < obj.depth))
				{
					topReceiver = receiver;
					minDepth = obj.depth;
				}
			}
			return topReceiver;
		}
	}
    public class EndDragEventArgs :
        EventArgs
    {
        public readonly GameObject Receiver;
        public readonly bool Cancel;

        public EndDragEventArgs(GameObject receiver, bool cancel)
        {
            Receiver = receiver;
            Cancel = cancel;
        }
    }
    public class BeginDragEventArgs :
        EventArgs
    {
        public bool Cancel { get; set; }

        public BeginDragEventArgs(bool cancel)
        {
            Cancel = cancel;
        }
    }
}