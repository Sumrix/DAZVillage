using UnityEngine;
using Collections;
using System;

namespace GameUI
{
    public class CategoriesPage :
        CollectionPage
    {
        [RequiredField]
        public CollectionView StringsView;
        private ObservableCollection<string> _strings;

        protected override CollectionView[] GetCollectionViews()
        {
            return new CollectionView[] { StringsView };
        }
        protected override void Awake()
        {
            base.Awake();

            _strings = new ObservableCollection<string>();
            StringsView.DataSource = _strings;

            foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
            {
                _strings.Add(ItemTypeToString.Convert(item));
            }
        }
    }
}