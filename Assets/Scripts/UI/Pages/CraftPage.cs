using UnityEngine;
using Collections;

namespace GameUI
{
    public class CraftPage :
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
            _resourcesView.Click += ResourcesView_Click;
        }
        private void ResourcesView_Click(object sender, CollectionViewClickEventArgs e)
        {
            var inspectorItem = (InspectorItem)e.Object;
            Show(new InspectorItem
            {
                Prefab = inspectorItem.Prefab,
                Count = inspectorItem.Prefab.CraftRecipe.ProducedQuantity
            });
        }
        protected override void ShowItem(Item item, int count)
        {
            base.ShowItem(item, count);
            
            _resources.Clear();
            if (item != null)
            {
                foreach (var recipeItem in item.CraftRecipe.Resources)
                {
                    _resources.Add(recipeItem);
                }
            }
        }
        public void Craft()
        {
            var bag = Managers.Inventory.Bag;
            if (CurrentItem != null)
            {
                int index = bag.IndexOf(null);

                if (index >= 0
                    && (CurrentItem.CraftRecipe.Resources.Length > 0
                        || Managers.Game.AllowCraftingBaseResources)
                    && ItemCollection.TryRemove(bag, CurrentItem.CraftRecipe.Resources))
                {
                    var rest = ItemCollection.Fill(bag, CurrentItem, CurrentItem.CraftRecipe.ProducedQuantity);
                    if (rest) Managers.Inventory.Drop(rest);
                }
            }
        }
    }
}