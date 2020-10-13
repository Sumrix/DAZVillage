using System.Linq;

namespace GameUI
{
    public class AlchemyItemsPage :
        ItemsPage,
        IManagerWaiter
    {
        private void Start()
        {
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            foreach (var item in Database.Items
                .OfType<Potion>()
                .Where(x => x.CraftRecipe.Resources.Length > 0
                || Managers.Game.AllowCraftingBaseResources))
            {
                _items.Add(new InspectorItem
                {
                    Prefab = item,
                    Count = item.CraftRecipe.ProducedQuantity
                });
            }
        }
    }
}