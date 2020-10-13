using UnityEngine;
using Collections;

namespace GameUI
{
    public class AlchemyPage :
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
        public void MakePotion()
        {
            var bag = Managers.Inventory.Bag;
            if (CurrentItem != null)
            {
                var potion = (Potion)CurrentItem;
                int index = bag.IndexOf(null);

                if (index >= 0 && ItemCollection.TryRemove(bag, potion.CraftRecipe.Resources))
                {
                    var rest = ItemCollection.Fill(bag, potion, potion.CraftRecipe.ProducedQuantity);
                    if (rest) Managers.Inventory.Drop(rest);
                }
            }
        }
    }
}