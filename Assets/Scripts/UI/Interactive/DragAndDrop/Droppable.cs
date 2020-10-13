using UnityEngine;
using System;

namespace GameUI
{
	public class DropEventArgs :
		EventArgs
	{
		public readonly GameObject Object;
		public bool Cancel { get; set; }

		public DropEventArgs(GameObject obj)
		{
			Object = obj;
			Cancel = false;
		}
	}
	public class Droppable :
		MonoBehaviour
	{
		public delegate void DropEventHandler(object sender, DropEventArgs e);
		public event DropEventHandler Dropped;

		protected void OnDropped(DropEventArgs e)
		{
			DropEventHandler handler = Dropped;
			if (handler != null)
			{
				handler(this, e);
			}
		}
		public bool OnDrop(GameObject obj)
		{
			var eventArgs = new DropEventArgs (obj);
			OnDropped(eventArgs);
			return !eventArgs.Cancel;
		}
	}
}