using System.Collections;
using System;

namespace GameUI
{
    /// <summary>
    /// The delegate type for handling a selection changed event
    /// </summary>
    public delegate void SelectionChangedEventHandler(
        object sender,
        SelectionChangedEventArgs e);

    /// <summary>
    /// The inputs to a selection changed event handler
    /// </summary>
    public class SelectionChangedEventArgs
    {
        /// <summary>
        /// The constructor for selection changed args
        /// </summary>
        /// <param name="removedItems">The items that were unselected during this event</param>
        /// <param name="addedItems">The items that were selected during this event</param>
        public SelectionChangedEventArgs(
            IList removedItems,
            IList addedItems)
        {
            if (removedItems == null)
                throw new ArgumentNullException("removedItems");
            if (addedItems == null)
                throw new ArgumentNullException("addedItems");

            _removedItems = new object[removedItems.Count];
            removedItems.CopyTo(_removedItems, 0);

            _addedItems = new object[addedItems.Count];
            addedItems.CopyTo(_addedItems, 0);
        }

        /// <summary>
        /// An IList containing the items that were unselected during this event
        /// </summary>
        public IList RemovedItems
        {
            get { return _removedItems; }
        }

        /// <summary>
        /// An IList containing the items that were selected during this event
        /// </summary>
        public IList AddedItems
        {
            get { return _addedItems; }
        }
        
        private object[] _addedItems;
        private object[] _removedItems;
    }
}
