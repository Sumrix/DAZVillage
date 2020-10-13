using UnityEngine;
using System;
using Collections;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GameUI
{
    /// <summary>
    /// Представление коллекции однотипных элементов
    /// </summary>
    public class CollectionView :
        MonoBehaviour
    {
        #region Fields and properties

        /// <summary>
        ///     An event reporting a mouse button was pressed once in a item.
        /// </summary>
        public event EventHandler<CollectionViewClickEventArgs> Click;
        /// <summary>
        ///     An event reporting a mouse button was pressed twice in a item.
        /// </summary>
        public event EventHandler<CollectionViewClickEventArgs> DoubleClick;
        /// <summary>
        ///     An event reporting a mouse button was long pressed in a item.
        /// </summary>
        public event EventHandler<CollectionViewClickEventArgs> LongClick;
        /// <summary>
        ///     An event fired when the selection changes.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;
        /// <summary>
        ///     The index of the first item in the current selection or -1 if the selection is empty.
        /// </summary>
        [HideInInspector]
        public int SelectedIndex;
        /// <summary>
        ///  The first item in the current selection, or null if the selection is empty.
        /// </summary>
        [HideInInspector]
        public object SelectedItem;
        [SerializeField]
        [RequiredField]
        private CollectionViewItem _itemPrefab;
        [SerializeField]
        [RequiredField]
        private Slot _slotPrefab;
        [SerializeField]
        [RequiredField]
        private Transform _container;
        [SerializeField]
        private DragDropMode DragDropMode;

        private List<Slot> _slots;
        private List<CollectionViewItem> _items;
        private IObservableCollection _dataSource;
        public List<CollectionViewItem> ViewItems
        {
            get { return _items; }
        }
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

        #endregion

        #region Constructors

        public CollectionView()
        {
            _items = new List<CollectionViewItem>();
            _slots = new List<Slot>();
        }

        #endregion

        #region Common Methods

        private void RemoveItem(int index)
        {
            var item = _items[index];
            var draggable = item.GetComponent<Draggable>();
            if (draggable)
            {
                draggable.BeginDrag -= Item_BeginDrag;
                draggable.EndDrag -= Item_EndDrag;
            }
            var clickable = item.GetComponent<Clickable>();
            if (clickable)
            {
                clickable.Click -= Item_Click;
            }
            item.Destroy();
            _items[index] = null;
        }
        private void CreateItem(int index)
        {
            var slot = _slots[index];
            // Чтобы встал перед слотом перемещаем его туда-сюда
            var item = Instantiate(_itemPrefab);
            item.transform.SetParent(slot.transform, false);
            item.transform.SetParent(Managers.UI.Canvas.transform);
            item.transform.SetParent(slot.transform);
            item.transform.localPosition = Vector2.zero;
            item.CollectionView = this;

            var draggable = item.GetComponent<Draggable>();
            if (draggable)
            {
                draggable.BeginDrag += Item_BeginDrag;
                draggable.EndDrag += Item_EndDrag;
            }
            var clickable = item.GetComponent<Clickable>();
            if (clickable)
            {
                clickable.Click += Item_Click;
                clickable.DoubleClick += Item_DoubleClick;
                clickable.LongClick += Item_LongClick;
            }
            _items[index] = item;
        }
        protected void SetValue(int startIndex, IList values)
        {
            for (int index = 0; index < values.Count; index++)
            {
                var item = _items[index + startIndex];
                var value = values[index];
                if (value != null)
                {
                    // Создаём и устанавливаем значение CollectionViewItem
                    if (!item) CreateItem(index + startIndex);
                    _items[index + startIndex].Upd(value);
                    // Если новое значение это SelectedItem
                    if (value == SelectedItem)
                    {
                        SelectedIndex = index + startIndex;
                    }
                }
                else
                {
                    if (item) RemoveItem(index + startIndex);
                }
                // Устанавливаем значение Slot, если требуется
                var dict = _dataSource as IDictionary;
                if (dict != null)
                {
                    var key = dict.Keys.Cast<object>().ElementAt(index + startIndex);
                    _slots[index + startIndex].Upd(key);
                }
            }
            
            ResetSelection();
        }
        protected void Insert(int startIndex, IList list)
        {
            for (int index = 0; index < list.Count; index++)
            {
                var slot = Instantiate(_slotPrefab);
                slot.transform.SetParent(_container, false);
                slot.Dropped += Slot_Dropped;
                _slots.Insert(index + startIndex, slot);

                _items.Insert(index + startIndex, null);
            }

            if (SelectedIndex >= startIndex)
            {
                SelectedIndex += list.Count;
            }

            if (SelectedIndex == -1)
            {
                ResetSelection();
            }

            SetValue(startIndex, list);
        }
        protected void RemoveAt(int startIndex, int count)
        {
            SetValue(startIndex, new object[count]);

            for (int index = 0; index < count; index++)
            {
                _items.RemoveAt(index + startIndex);

                var slot = _slots[index + startIndex];
                slot.Dropped -= Slot_Dropped;
                slot.Destroy();
                _slots.RemoveAt(index + startIndex);
            }

            if (SelectedIndex >= startIndex)
            {
                ResetSelection();
            }
        }
        protected virtual bool Move(IObservableCollection dstList, int dstIndex, object srcValue,
            IObservableCollection srcList, int srcIndex, object dstValue)
        {
            try
            {
                // To act depending on the mode
                switch (DragDropMode)
                {
                    // Возможно этот режим работает не правильно, ну и ладно, мы им всёравно не пользуемся
                    case DragDropMode.InsertRemove:
                        dstList.Insert(dstIndex, srcValue);
                        try
                        {
                            srcList[srcIndex] = dstValue;
                            return true;
                        }
                        catch { dstList.Insert(dstIndex, dstValue); }
                        break;
                    case DragDropMode.Replace:
                        srcList[srcIndex] = dstValue;
                        try
                        {
                            dstList[dstIndex] = srcValue;
                            return true;
                        }
                        catch { srcList[srcIndex] = srcValue; }
                        break;
                }
            }
            catch (Exception e)
            {
                switch (DragDropMode)
                {
                    case DragDropMode.InsertRemove:
                        break;
                    case DragDropMode.Replace:
                        srcList[srcIndex] = srcValue;
                        break;
                }
                this.Log("Wrong drop", e.ToString());
            }
            return false;
        }
        private void Reset()
        {
            Clear();
            if (_dataSource != null)
            {
                Insert(0, _dataSource);
            }
        }
        private void Clear()
        {
            foreach (var item in _items)
            {
                item.Destroy();
            }
            _items.Clear();
            foreach (var slot in _slots)
            {
                slot.Destroy();
            }
            _slots.Clear();

            ResetSelection();
        }
        public void Select(int index)
        {
            var oldItem = SelectedItem;
            int oldIndex = SelectedIndex;
            SelectedItem = _dataSource[index];
            SelectedIndex = index;

            if (oldIndex != SelectedIndex || oldItem != SelectedItem)
            {
                OnSelectionChange(new SelectionChangedEventArgs(
                    new object[] { oldItem },
                    new object[] { SelectedItem }));
            }
        }
        private void ResetSelection()
        {
            if (_dataSource == null || _dataSource.Count == 0)
            {
                // Если списка нет или он пуст, то пустое выделение
                SelectedItem = null;
                SelectedIndex = -1;

                OnSelectionChange(new SelectionChangedEventArgs(
                    new object[0],
                    new object[0]));
            }
            else
            {
                int index = Math.Min(_dataSource.Count - 1, Math.Max(SelectedIndex, 0));
                if (_dataSource[index] != null)
                {
                    // Если на месте выделения что-то есть, то выбираем его
                    Select(index);
                }
                else
                {
                    // Если нет, пытаемся найти ненулевой элемент
                    index = 0;
                    while (index < _dataSource.Count && _dataSource[index] == null) index++;
                    if (index != _dataSource.Count)
                    {
                        // Если есть, выбираем его
                        Select(index);
                    }
                    else
                    {
                        // Если нет, то пустое выделение
                        SelectedItem = null;
                        SelectedIndex = -1;
                    }
                }
            }
        }

        #endregion

        #region Event Callbacks

        private void DataSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Insert(e.NewStartingIndex, e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveAt(e.OldStartingIndex, e.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    SetValue(e.NewStartingIndex, e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    RemoveAt(e.OldStartingIndex, e.OldItems.Count);
                    Insert(e.NewStartingIndex, e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
            }
        }
        private void Item_BeginDrag(object sender, BeginDragEventArgs e)
        {
            e.Cancel = DragDropMode == DragDropMode.None;
            if (!e.Cancel)
            {
                var item = ((Draggable)sender).GetComponent<CollectionViewItem>();
                int index = _items.IndexOf(item);
                OnClick(new CollectionViewClickEventArgs(index, _dataSource[index]));
                Select(index);
            }
        }
        private void Item_EndDrag(object sender, EndDragEventArgs e)
        {
            var item = ((Draggable)sender).GetComponent<CollectionViewItem>();
            int index = _items.IndexOf(item);
            if (index >= 0)
            {
                item.transform.SetParent(_slots[index].transform);
                item.transform.localPosition = Vector3.zero;
            }
        }
        protected void Slot_Dropped(object sender, DropEventArgs e)
        {
            var item = e.Object.GetComponent<CollectionViewItem>();
            if (item != null || DragDropMode == DragDropMode.None)
            {
                // Prepare variables
                int srcIndex = item.CollectionView._items.IndexOf(item);
                var srcValue = item.CollectionView.DataSource[srcIndex];
                var srcList = item.CollectionView.DataSource;
                int dstIndex = _slots.IndexOf((Slot)sender);
                var dstValue = DataSource[dstIndex];
                var dstList = DataSource;

                e.Cancel = !Move(dstList, dstIndex, srcValue, srcList, srcIndex, dstValue);

                if (_dataSource[dstIndex] != null)
                {
                    Select(dstIndex);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void Item_Click(object sender, EventArgs e)
        {
            var item = ((Clickable)sender).GetComponent<CollectionViewItem>();
            int index = _items.IndexOf(item);

            Select(index);
            OnClick(new CollectionViewClickEventArgs(index, _dataSource[index]));
        }
        private void Item_LongClick(object sender, EventArgs e)
        {
            var item = ((Clickable)sender).GetComponent<CollectionViewItem>();
            int index = _items.IndexOf(item);

            Select(index);
            OnLongClick(new CollectionViewClickEventArgs(index, _dataSource[index]));
        }
        private void Item_DoubleClick(object sender, EventArgs e)
        {
            var item = ((Clickable)sender).GetComponent<CollectionViewItem>();
            int index = _items.IndexOf(item);

            OnDoubleClick(new CollectionViewClickEventArgs(index, _dataSource[index]));
        }

        #endregion

        #region Event Methods

        protected void OnSelectionChange(SelectionChangedEventArgs e)
        {
            var handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnClick(CollectionViewClickEventArgs e)
        {
            var handler = Click;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnDoubleClick(CollectionViewClickEventArgs e)
        {
            var handler = DoubleClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnLongClick(CollectionViewClickEventArgs e)
        {
            var handler = LongClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void OnDestroy()
        {
            if (_dataSource != null)
            {
                _dataSource.CollectionChanged -= DataSource_CollectionChanged;
            }
        }

        #endregion
    }

    public enum DragDropMode
    {
        None,
        Replace,
        InsertRemove,
    }

    #region EventArgs

    public class CollectionViewClickEventArgs :
        EventArgs
    {
        public readonly int Index;
        public readonly object Object;

        public CollectionViewClickEventArgs(int index, object obj)
        {
            Index = index;
            Object = obj;
        }
    }

    #endregion
}
