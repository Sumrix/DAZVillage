using UnityEngine;
using System.Linq;

namespace GameUI
{
    [RequireComponent(typeof(Clickable))]
    [RequireComponent(typeof(ItemView))]
    public class HealButton :
        MonoBehaviour,
        IManagerWaiter
    {
        private InspectorItem _item;
        private ItemView _view;
        [SerializeField]
        [RequiredField]
        private Potion _itemPrefab;

        private void Start()
        {
            _item = new InspectorItem
            {
                Count = 0,
                Prefab = _itemPrefab
            };
            _view = GetComponent<ItemView>();
            GetComponent<Clickable>().Click += HealButton_Click;
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            Managers.Inventory.Bag.CollectionChanged += Bag_CollectionChanged;
            Show();
        }
        private void Show()
        {
            _item.Count = Mathf.RoundToInt(
                Managers.Inventory.Bag
                .OfType<Potion>()
                .Where(x => x.ID == _itemPrefab.ID)
                .Sum(x => x.Stack.Current));
            
            _view.Upd(_item);
        }
        private void Bag_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Show();
        }
        private void HealButton_Click(object sender, System.EventArgs e)
        {
            if (_item.Count > 0)
            {
                if (_itemPrefab.Skill != null && Managers.Player.Player.AddSkill(_itemPrefab.Skill, Managers.Player.Player))
                {
                    Collections.ItemCollection.Remove(Managers.Inventory.Bag, _itemPrefab, 1);
                }
            }
        }
    }
}