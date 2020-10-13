using UnityEngine;
using System;
using GameUI;
using ActiveObjects.Triggers;
using System.Collections.Generic;

namespace ActiveObjects
{
    public class PickUper :
        MonoBehaviour
    {
        public event EventHandler<PickUperEventArgs> PickUpped;
        public TimerTrigger MessageTimer;

        private struct ItemInfo
        {
            public Sprite sprite;
            public string text;
        }
        private Queue<ItemInfo> _items;

        private void Awake()
        {
            _items = new Queue<ItemInfo>();
            MessageTimer.Active += MessageTimer_Active;
        }

        private void MessageTimer_Active(object sender, EventArgs e)
        {
            if (_items.Count > 0)
            {
                var item = _items.Dequeue();
                Managers.UI.ShowPopupIcon(item.sprite, item.text);
            }
            else
            {
                MessageTimer.Disable();
            }
        }

        public void OnTriggerStay(Collider other)
        {
            var item = other.GetComponent<Item>();
            if (item != null)
            {
                // Добавляем вещь в инвентарь
                var e = new PickUperEventArgs { Item = item };
                OnPickUpped(e);
                // Если вещь успешно добавилась, выводим сообщение и удаляем объект
                if (!e.Cancel)
                {
                    _items.Enqueue(new ItemInfo
                    {
                        sprite = item.Sprite,
                        text = "x" + Mathf.RoundToInt(item.Stack.Current).ToString()
                    });
                    MessageTimer.Enable();

                    item.gameObject.SetActive(false);
                    item.Destroy();
                }
            }
        }
        private void Start()
        {
            Managers.Inventory.Dropped += Inventory_Dropped; //подписка на событие
        }
        private void Inventory_Dropped(object sender, DroppedEventArgs e)
        {
            e.Item.transform.localPosition = transform.position;
            e.Item.gameObject.SetActive(true);
        }
        private void OnPickUpped(PickUperEventArgs e)
        {
            var handler = PickUpped;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class PickUperEventArgs :
        EventArgs
    {
        public Item Item;
        public bool Cancel = false;
    }
}