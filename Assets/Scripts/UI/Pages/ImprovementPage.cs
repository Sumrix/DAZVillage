using UnityEngine;
using Collections;

namespace GameUI
{
    public class ImprovementPage :
        ItemPage
    {
        [SerializeField]
        [RequiredField]
        private CollectionView _resourcesView;
        private ObservableCollection<InspectorItem> _resources;
        
        private void Awake()
        {
            _resources = new ObservableCollection<InspectorItem>();
            _resourcesView.DataSource = _resources;
        }
        protected override void ShowItem(Item item, int count)
        {
            _resources.Clear();
            var equipment = item as Equipment;
            if (equipment != null)
            {
                base.ShowItem(item, count);
                foreach (var recipeItem in equipment.ImproveRecipe)
                {
                    _resources.Add(recipeItem);
                }
            }
            else
            {
                base.ShowItem(null, 0);
            }
        }
        public void Improve()
        {
            Equipment equipment = (Equipment)CurrentItem;
            var bag = Managers.Inventory.Bag;
            if (ItemCollection.TryRemove(bag, equipment.ImproveRecipe))
            {
                equipment.LevelUp();
                _itemsPage.SelectedCollectionView.DataSource.ResetItem(_itemsPage.SelectedCollectionView.SelectedIndex);
            }
        }
    }
}