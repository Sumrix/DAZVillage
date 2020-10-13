using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Collections
{
    /// <summary>
    /// Implementation of a dynamic data collection based on generic KeyedCollection&lt;TValue&gt;,
    /// implementing INotifyCollectionChanged to notify listeners
    /// when items get added, removed or the whole list is refreshed.
    /// </summary>
    [Serializable()]
    public class ObservableKeyedCollection<TKey, TValue> :
        KeyedCollection1<TKey, TValue>,
        IObservableCollection<TValue>,
        IObservableCollection
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of ObservableCollection that is empty and has default initial capacity.
        /// </summary>
        public ObservableKeyedCollection(Func<TValue, TKey> getKeyFromValue) : this(getKeyFromValue, null, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableKeyedCollection{TKey,TValue}">ObservableKeyedCollection&lt;TKey,TValue&gt;</see> class using the specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}">IEqualityComparer&lt;TKey&gt;</see> to use when comparing keys, or <null/> to use the default <see cref="EqualityComparer{TKey}">EqualityComparer&lt;TKey&gt;</see> for the type of the key.</param>
        protected ObservableKeyedCollection(Func<TValue, TKey> getKeyFromValue, IEqualityComparer<TKey> comparer) : this(getKeyFromValue, comparer, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableKeyedCollection{TKey,TValue}">ObservableKeyedCollection&lt;TKey,TValue&gt;</see> class using the specified initial capacity and comparer.
		/// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}">IEqualityComparer&lt;TKey&gt;</see> to use when comparing keys, or <null/> to use the default <see cref="EqualityComparer{TKey}">EqualityComparer&lt;TKey&gt;</see> for the type of the key.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="ObservableKeyedCollection{TKey,TValue}">ObservableKeyedCollection&lt;TKey,TValue&gt;</see> collection can contain.</param>
        protected ObservableKeyedCollection(Func<TValue, TKey> getKeyFromValue, IEqualityComparer<TKey> comparer, int capacity) :
            base(comparer, capacity)
        {
            if (getKeyFromValue == null)
            {
                throw new ArgumentNullException("getKeyFromValue");
            }

            _getKeyFromValue = getKeyFromValue;
        }
        /// <summary>
        /// Initializes a new instance of the ObservableCollection class
        /// that contains elements copied from the specified list
        /// </summary>
        /// <param name="list">The list whose elements are copied to the new list.</param>
        /// <remarks>
        /// The elements are copied onto the ObservableCollection in the
        /// same order they are read by the enumerator of the list.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> list is a null reference </exception>
        public ObservableKeyedCollection(Func<TValue, TKey> getKeyFromValue, IList<TValue> list)
            : this(getKeyFromValue, null, list != null ? list.Count : 0)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            CopyFrom(list);
        }
        /// <summary>
        /// Initializes a new instance of the ObservableCollection class
        /// that contains elements copied from the specified dictionary
        /// </summary>
        /// <param name="dict">The dictionary whose elements are copied to the new dictionary.</param>
        /// <remarks>
        /// The elements are copied onto the ObservableCollection in the
        /// same order they are read by the enumerator of the dictionary.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> list is a null reference </exception>
        public ObservableKeyedCollection(Func<TValue, TKey> getKeyFromValue, IDictionary<TKey, TValue> dict)
            : this(getKeyFromValue, null, dict != null ? dict.Count : 0)
        {
            if (dict == null)
                throw new ArgumentNullException("dict");

            CopyFrom(dict);
        }

        /// <summary>
        /// Initializes a new instance of the ObservableCollection class that contains
        /// elements copied from the specified collection and has sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <remarks>
        /// The elements are copied onto the ObservableCollection in the
        /// same order they are read by the enumerator of the collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> collection is a null reference </exception>
        public ObservableKeyedCollection(Func<TValue, TKey> getKeyFromValue, IEnumerable<TValue> collection):
            this(getKeyFromValue)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            CopyFrom(collection);
        }

        private void CopyFrom(IEnumerable<TValue> collection)
        {
            IList<TValue> items = Items;
            if (collection != null && items != null)
            {
                using (IEnumerator<TValue> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Add(enumerator.Current);
                    }
                }
            }
        }
        private void CopyFrom(IDictionary<TKey, TValue> dict)
        {
            IList<TValue> items = Items;
            if (dict != null && items != null)
            {
                using (var enumerator = dict.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ((IDictionary<TKey, TValue>)this).Add(enumerator.Current);
                    }
                }
            }
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Move item at oldIndex to newIndex.
        /// </summary>
        public void Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        /// <summary>
        /// Raises a CollectionChanged event of type Replace for the item at the specified position.
        /// </summary>
        /// <param name="position">A zero-based index of the item to be reset.</param>
        public void ResetItem(int position)
        {
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, this[position], this[position], position);
        }

        /// <summary>
        /// Raises a CollectionChanged event of type Reset.
        /// </summary>
        public void ResetBindings()
        {
            OnCollectionReset();
        }

        #endregion Public Methods

        #region Public Events

        //------------------------------------------------------
        #region INotifyPropertyChanged implementation
        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }
            remove
            {
                PropertyChanged -= value;
            }
        }
        #endregion INotifyPropertyChanged implementation


        //------------------------------------------------------
        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item.
        /// </summary>
        /// <remarks>
        /// see <seealso cref="INotifyCollectionChanged"/>
        /// </remarks>
#if !FEATURE_NETCORE
        [field: NonSerializedAttribute()]
#endif
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion Public Events

        #region Protected Methods

        /// <summary>
        /// Called by base class KeyedCollection&lt;TValue&gt; when the list is being cleared;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void ClearItems()
        {
            CheckReentrancy();
            base.ClearItems();
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionReset();
        }

        /// <summary>
        /// Called by base class KeyedCollection&lt;TValue&gt; when an item is removed from list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void RemoveItem(int index)
        {
            CheckReentrancy();
            TValue removedItem = this[index];

            base.RemoveItem(index);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }

        /// <summary>
        /// Called by base class KeyedCollection&lt;TValue&gt; when an item is added to list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void InsertItem(int index, TValue item)
        {
            CheckReentrancy();

            var newKey = GetKeyForItem(item);
            if (Dictionary.ContainsKey(newKey))
            {
                throw new ArgumentException("Illegal key repetition", "item");
            }

            base.InsertItem(index, item);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Called by base class KeyedCollection&lt;TKey, TValue&gt; when an item is set in list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void SetItem(int index, TValue item)
        {
            CheckReentrancy();

            if (item != null)
            {
                var newKey = GetKeyForItem(item);
                if (IndexOf(newKey) != index)
                {
                    throw new ArgumentException("Illegal key repetition", "item");
                }
            }

            TValue originalItem = this[index];
            base.SetItem(index, item);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
        }

        /// <summary>
        /// Called by base class ObservableCollection&lt;TValue&gt; when an item is to be moved within the list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            TValue removedItem = this[oldIndex];

            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, removedItem);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        }


        /// <summary>
        /// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
#if !FEATURE_NETCORE
        [field: NonSerializedAttribute()]
#endif
        protected virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise CollectionChanged event to any listeners.
        /// Properties/methods modifying this ObservableCollection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
        /// </remarks>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                using (BlockReentrancy())
                {
                    CollectionChanged(this, e);
                }
            }
        }

        /// <summary>
        /// Disallow reentrant attempts to change this collection. E.g. a event handler
        /// of the CollectionChanged event is not allowed to make changes to this collection.
        /// </summary>
        /// <remarks>
        /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
        /// <code>
        ///         using (BlockReentrancy())
        ///         {
        ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
        ///         }
        /// </code>
        /// </remarks>
        protected IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        /// <summary> Check and assert for reentrant attempts to change this collection. </summary>
        /// <exception cref="InvalidOperationException"> raised when changing the collection
        /// while another collection change is still being notified to other listeners </exception>
        protected void CheckReentrancy()
        {
            if (_monitor.Busy)
            {
                // we can allow changes if there's only one listener - the problem
                // only arises if reentrant changes make the original event args
                // invalid for later listeners.  This keeps existing code working
                // (e.g. Selector.SelectedItems).
                if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
                    throw new InvalidOperationException("ObservableCollection reentrancy not allowed");
            }
        }

        #endregion Protected Methods

        #region Private Methods
        /// <summary>
        /// Helper to raise a PropertyChanged event  />).
        /// </summary>
        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event with action == Reset to any listeners
        /// </summary>
        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override TKey GetKeyForItem(TValue item)
        {
            return _getKeyFromValue(item);
        }

        #endregion Private Methods

        #region Private Types

        // this class helps prevent reentrant calls
#if !FEATURE_NETCORE
        [Serializable()]
        [TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
#endif
        private class SimpleMonitor : IDisposable
        {
            public void Enter()
            {
                ++_busyCount;
            }

            public void Dispose()
            {
                --_busyCount;
            }

            public bool Busy { get { return _busyCount > 0; } }

            int _busyCount;
        }

        #endregion Private Types

        #region Private Fields

        private const string CountString = "Count";

        // This must agree with Binding.IndexerName.  It is declared separately
        // here so as to avoid a dependency on PresentationFramework.dll.
        private const string IndexerName = "Item[]";

        private SimpleMonitor _monitor = new SimpleMonitor();

        private Func<TValue, TKey> _getKeyFromValue;

        #endregion Private Fields
    }
}
