using UnityEngine;

namespace GameUI
{
    public class ChestItemsPage :
        ItemsPage,
        IManagerWaiter
    {
        private void Start()
        {
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            _itemsView.DataSource = Managers.Inventory.Chest;
        }
    }
}