using UnityEngine;
using Collections;
using System.Collections;
using System.Collections.Specialized;
using System;

namespace GameUI
{
    public class GameObjectCollection :
        MonoBehaviour
    {
        private IObservableCollection _dataSource;
        public IObservableCollection DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                if (_dataSource != null)
                {
                    _dataSource.CollectionChanged -= DataSource_CollectionChanged;
                }
                _dataSource = value;
                if (_dataSource != null)
                {
                    _dataSource.CollectionChanged += DataSource_CollectionChanged;
                }
                Reset();
            }
        }
        private void DataSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Insert(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Replace(e.NewItems, e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Remove(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
            }
        }
        private void RemoveItem(object item)
        {
            var component = (Component)item;
            if (component.transform.parent == transform)
            {
                component.transform.parent = null;
            }
        }
        private void AddItem(System.Object item)
        {
            var component = (Component)item;
            component.transform.SetParent(transform);
            component.transform.localPosition = Vector3.zero;
            component.gameObject.SetActive(false);
        }
        private void Remove(IList oldItems)
        {
            foreach (var item in oldItems)
            {
                if (item != null)
                {
                    RemoveItem(item);
                }
            }
        }
        private void Replace(IList newItems, IList oldItems)
        {
            for (int i = 0; i < newItems.Count; i++)
            {
                var newItem = newItems[i];
                var oldItem = oldItems[i];

                if (oldItem != newItem)
                {
                    if (oldItem != null)
                    {
                        RemoveItem(oldItem);
                    }
                    if (newItem != null)
                    {
                        AddItem(newItem);
                    }
                }
            }
        }
        private void Insert(IList items)
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    AddItem(item);
                }
            }
        }
        private void Reset()
        {
            Remove(transform.GetComponentsInChildren<Transform>());
            if (_dataSource != null)
            {
                Insert(_dataSource);
            }
        }
    }
}
