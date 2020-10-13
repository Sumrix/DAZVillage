using UnityEngine;
using Collections;

namespace GameUI
{
    public class ItemsPage :
        CollectionPage
    {
        [SerializeField]
        [RequiredField]
        protected CollectionView _itemsView;
        protected ObservableCollection<InspectorItem> _items;

        protected override void Awake()
        {
            base.Awake();

            _items = new ObservableCollection<InspectorItem>();
            _itemsView.DataSource = _items;
        }
        protected override CollectionView[] GetCollectionViews()
        {
            return new CollectionView[] { _itemsView };
        }
        public void ShowItemsOfType(ItemType itemType)
        {
            Title = ItemTypeToString.Convert(itemType);

            _items.Clear();
            foreach (var item in Database.Items)
            {
                if (item.ItemType == itemType
                    && !(item is Potion)
                    && (item.CraftRecipe.Resources.Length > 0
                        || Managers.Game.AllowCraftingBaseResources))
                {
                    var inspectorItem = new InspectorItem
                    {
                        Prefab = item,
                        Count = item.CraftRecipe.ProducedQuantity
                    };
                    _items.Add(inspectorItem);
                }
            }
        }
    }
}