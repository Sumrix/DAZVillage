// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
// <OWNER>Microsoft</OWNER>
// 

namespace System.Collections.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
       
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]        
    public abstract class KeyedCollection1<TKey,TItem>: Collection<TItem>, IDictionary<TKey, TItem>, IDictionary
    {
        const int defaultThreshold = 0;

        IEqualityComparer<TKey> comparer;
        Dictionary<TKey,TItem> dict;
        int keyCount;
        int threshold;

        protected KeyedCollection1(): this(null, defaultThreshold) {}

        protected KeyedCollection1(IEqualityComparer<TKey> comparer): this(comparer, defaultThreshold) {}


        protected KeyedCollection1(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold) {
            if (comparer == null) { 
                comparer = EqualityComparer<TKey>.Default;
            }

            if (dictionaryCreationThreshold == -1) {
                dictionaryCreationThreshold = int.MaxValue;
            }

            if( dictionaryCreationThreshold < -1) {
                throw new ArgumentOutOfRangeException("dictionaryCreationThreshold");
            }

            this.comparer = comparer;
            this.threshold = dictionaryCreationThreshold;
        }

        public IEqualityComparer<TKey> Comparer {
            get {
                return comparer;                
            }               
        }

        public TItem this[TKey key] {
            get
            {
                if( key == null) {
                    throw new ArgumentNullException("key");
                }

                if (dict != null) {
                    return dict[key];
                }

                foreach (TItem item in Items) {
                    if (comparer.Equals(GetKeyForItem(item), key)) return item;
                }

                throw new ArgumentException("Key not found");
                //return default(TItem);
            }

            set
            {
                this[IndexOf(key)] = value;
            }
        }

        public int IndexOf(TKey key)
        {
            int index = 0;
            foreach (var curKey in dict.Keys)
            {
                if (Comparer.Equals(curKey, key)) break;
                index++;
            }
            return index == Items.Count ? -1 : index;
        }

        public bool ContainsKey(TKey key) {
            if( key == null) {
                throw new ArgumentNullException("key");
            }
            
            if (dict != null) {
                return dict.ContainsKey(key);
            }

            if (key != null) {
                foreach (TItem item in Items) {
                    if (comparer.Equals(GetKeyForItem(item), key)) return true;
                }
            }
            return false;
        }

        private bool ContainsItem(TItem item) {                        
            TKey key;
            if( (dict == null) || ((key = GetKeyForItem(item)) == null)) {
                return Items.Contains(item);
            }

            TItem itemInDict;
            bool exist = dict.TryGetValue(key, out itemInDict);
            if( exist) {
                return EqualityComparer<TItem>.Default.Equals(itemInDict, item);
            }
            return false;
        }

        public bool Remove(TKey key) {
            if( key == null) {
                throw new ArgumentNullException("key");
            }
            
            if (dict != null) {
                if (dict.ContainsKey(key)) {
                    return Remove(dict[key]);
                }

                return false;
            }

            if (key != null) {
                for (int i = 0; i < Items.Count; i++) {
                    if (comparer.Equals(GetKeyForItem(Items[i]), key)) {
                        RemoveItem(i);
                        return true;
                    }
                }
            }
            return false;
        }

        protected IDictionary<TKey,TItem> Dictionary {
            get { return dict; }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return dict.Keys;
            }
        }

        public ICollection<TItem> Values
        {
            get
            {
                return Items;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return dict.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return (ICollection)Items;
            }
        }

        bool ICollection<KeyValuePair<TKey, TItem>>.IsReadOnly
        {
            get
            {
                return ((IDictionary<TKey, TItem>)dict).IsReadOnly;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this[(TKey)key];
            }

            set
            {
                this[(TKey)key] = (TItem)value;
            }
        }

        protected void ChangeItemKey(TItem item, TKey newKey) {
            // check if the item exists in the collection
            if( !ContainsItem(item)) {
                throw new ArgumentException("Item not in list");
            }

            TKey oldKey = GetKeyForItem(item);            
            if (!comparer.Equals(oldKey, newKey)) {
                if (newKey != null) {
                    AddKey(newKey, item);
                }

                if (oldKey != null) {
                    RemoveKey(oldKey);
                }
            }
        }

        protected override void ClearItems() {
            base.ClearItems();
            if (dict != null) {
                dict.Clear();
            }

            keyCount = 0;
        }

        protected abstract TKey GetKeyForItem(TItem item);

        protected override void InsertItem(int index, TItem item) {
            TKey key = GetKeyForItem(item);
            if (key != null) {
                AddKey(key, item);
            }
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index) {
            TKey key = GetKeyForItem(Items[index]);
            if (key != null) {
                RemoveKey(key);
            }
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, TItem item) {
            if (item != null)
            {
                TKey newKey = GetKeyForItem(item);
                TKey oldKey = Keys.ElementAt(index);

                if (comparer.Equals(oldKey, newKey))
                {
                    if (newKey != null && dict != null)
                    {
                        dict[newKey] = item;
                    }
                }
                else
                {
                    if (newKey != null)
                    {
                        AddKey(newKey, item);
                    }

                    if (oldKey != null)
                    {
                        RemoveKey(oldKey);
                    }
                }
            }
            else
            {
                var key = Keys.ElementAt(index);
                if (dict != null)
                {
                    dict[key] = item;
                }
            }
            base.SetItem(index, item);
        }

        private void AddKey(TKey key, TItem item) {
            if (dict != null) {
                dict.Add(key, item);
            }
            else if (keyCount == threshold) {
                CreateDictionary();
                dict.Add(key, item);
            }
            else {
                if (ContainsKey(key)) {
                    throw new ArgumentException("Adding duplicate");
                }

                keyCount++;
            }
        }

        private void CreateDictionary() {
            dict = new Dictionary<TKey,TItem>(comparer);
            foreach (TItem item in Items) {
                TKey key = GetKeyForItem(item);
                if (key != null) {
                    dict.Add(key, item);
                }
            }
        }

        private void RemoveKey(TKey key) {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (dict != null) {
                dict.Remove(key);
            }
            else {
                keyCount--;
            }
        }

        public void Add(TKey key, TItem value)
        {
            if (value != null && !Comparer.Equals(key, GetKeyForItem(value)))
            {
                throw new ArgumentException("The key does not match the value", "key");
            }
            if (key != null)
            {
                AddKey(key, value);
            }
            base.InsertItem(Items.Count, value);
        }

        public bool TryGetValue(TKey key, out TItem value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            value = default(TItem);
            return false;
        }

        void ICollection<KeyValuePair<TKey, TItem>>.Add(KeyValuePair<TKey, TItem> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TItem>>.Contains(KeyValuePair<TKey, TItem> item)
        {
            return ContainsKey(item.Key);
        }

        void ICollection<KeyValuePair<TKey, TItem>>.CopyTo(KeyValuePair<TKey, TItem>[] array, int arrayIndex)
        {
            foreach (var item in Items)
            {
                array[arrayIndex] = new KeyValuePair<TKey, TItem>(GetKeyForItem(item), item);
            }
        }

        public bool Remove(KeyValuePair<TKey, TItem> item)
        {
            return Remove(item.Value);
        }

        IEnumerator<KeyValuePair<TKey, TItem>> IEnumerable<KeyValuePair<TKey, TItem>>.GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return new KeyValuePair<TKey, TItem>(GetKeyForItem(item), item);
            }
        }

        void IDictionary.Add(object key, object value)
        {
            Add((TKey)key, (TItem)value);
        }

        bool IDictionary.Contains(object key)
        {
            return ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            Remove((TKey)key);
        }
    }
}
