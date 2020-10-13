using System.Linq;
using System;
using UnityEngine;

namespace GameUI
{
    public class CollectionPage :
        Page
    {
        public class CollectionPageEventArgs :
            EventArgs
        {
            public object Object;
        }
        public event EventHandler<CollectionPageEventArgs> Selected;
        public event EventHandler<CollectionPageEventArgs> Click;
        public event EventHandler<CollectionPageEventArgs> LongClick;
        public event EventHandler<CollectionPageEventArgs> DoubleClick;
        [HideInInspector]
        public CollectionView[] CollectionViews;
        [HideInInspector]
        public CollectionView SelectedCollectionView;
        public object SelectedItem
        {
            get
            {
                return SelectedCollectionView != null
                    ? SelectedCollectionView.SelectedItem
                    : null;
            }
        }

        protected virtual void SelectDefault()
        {
            if (SelectedCollectionView == null || SelectedCollectionView.SelectedItem == null)
            {
                SelectedCollectionView = CollectionViews
                    .FirstOrDefault(x => x.SelectedItem != null);

                if (SelectedCollectionView == null)
                {
                    Select(null);
                }
                else
                {
                    Select(SelectedCollectionView.SelectedItem);
                }
            }
        }
        protected virtual CollectionView[] GetCollectionViews()
        {
            throw new NotImplementedException();
        }
        protected virtual void Awake()
        {
            CollectionViews = GetCollectionViews();
        }
        protected virtual void OnEnable()
        {
            SubscribleEvents();
            SelectDefault();
        }
        protected virtual void OnDisable()
        {
            UnSubscribleEvents();
        }
        protected virtual void SubscribleEvents()
        {
            foreach (var collectionView in CollectionViews)
            {
                collectionView.SelectionChanged += CollectionView_SelectionChanged;
                collectionView.Click += CollectionView_Click;
                collectionView.LongClick += CollectionView_LongClick;
                collectionView.DoubleClick += CollectionView_DoubleClick;
            }
        }
        protected virtual void UnSubscribleEvents()
        {
            foreach (var collectionView in CollectionViews)
            {
                collectionView.SelectionChanged -= CollectionView_SelectionChanged;
                collectionView.Click -= CollectionView_Click;
                collectionView.LongClick -= CollectionView_LongClick;
                collectionView.DoubleClick -= CollectionView_DoubleClick;
            }
        }
        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                SelectedCollectionView = (CollectionView)sender;
                Select(e.AddedItems[0]);
            }
            else
            {
                SelectDefault();
            }
        }
        private void CollectionView_Click(object sender, CollectionViewClickEventArgs e)
        {
            SelectedCollectionView = (CollectionView)sender;
            OnClick(new CollectionPageEventArgs { Object = e.Object });
        }
        private void CollectionView_LongClick(object sender, CollectionViewClickEventArgs e)
        {
            SelectedCollectionView = (CollectionView)sender;
            OnLongClick(new CollectionPageEventArgs { Object = e.Object });
        }
        private void CollectionView_DoubleClick(object sender, CollectionViewClickEventArgs e)
        {
            OnDoubleClick(new CollectionPageEventArgs { Object = e.Object });
        }
        protected virtual void Select(object obj)
        {
            OnSelected(new CollectionPageEventArgs { Object = obj });
        }
        private void OnSelected(CollectionPageEventArgs e)
        {
            var handler = Selected;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void OnClick(CollectionPageEventArgs e)
        {
            var handler = Click;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void OnLongClick(CollectionPageEventArgs e)
        {
            var handler = LongClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void OnDoubleClick(CollectionPageEventArgs e)
        {
            var handler = DoubleClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}