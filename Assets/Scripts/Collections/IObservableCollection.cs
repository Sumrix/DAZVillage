using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Collections
{
    /// <summary>
    /// Represents a dynamic data collection based on generic IList&lt;T&gt;,
    /// implementing INotifyCollectionChanged to notify listeners
    /// when items get added, removed or the whole list is refreshed.
    /// </summary>
    public interface IObservableCollection<T> :
        IList<T>,
        INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        void ResetItem(int position);
        void ResetBindings();
    }

    public interface IObservableCollection :
        IList,
        INotifyCollectionChanged,
        INotifyPropertyChanged
    {
        void ResetItem(int position);
        void ResetBindings();
    }
}
