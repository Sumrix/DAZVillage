using UnityEngine;

namespace GameUI
{
    public class InventoryPage :
        CollectionPage
    {
        [SerializeField]
        [RequiredField]
        private CollectionView _bagView;
        [SerializeField]
        [RequiredField]
        private CollectionView _equipmentView;
        
        protected override void SubscribleEvents()
        {
            base.SubscribleEvents();
            _bagView.DoubleClick += Bag_DoubleClick;
            _bagView.LongClick += BagView_LongClick;
            _equipmentView.DoubleClick += EquipmentView_DoubleClick;
            _equipmentView.LongClick += EquipmentView_LongClick;
        }
        protected override void UnSubscribleEvents()
        {
            base.UnSubscribleEvents();
            _bagView.DoubleClick -= Bag_DoubleClick;
            _bagView.LongClick -= BagView_LongClick;
            _equipmentView.DoubleClick -= EquipmentView_DoubleClick;
            _equipmentView.LongClick -= EquipmentView_LongClick;
        }
        protected override CollectionView[] GetCollectionViews()
        {
            return new CollectionView[] { _bagView, _equipmentView };
        }
        private void Equip()
        {
            var equipment = (Equipment)SelectedCollectionView.SelectedItem;
            int index = Managers.Inventory.Equipment.IndexOf(equipment.EquipmentType);
            var equippedItem = Managers.Inventory.Equipment[index];
            Managers.Inventory.Bag[_bagView.SelectedIndex] = equippedItem;
            Managers.Inventory.Equipment[index] = equipment;
            _equipmentView.Select(index);
        }
        private void Unequip()
        {
            var equipment = (Item)_equipmentView.SelectedItem;
            int index = Managers.Inventory.Bag.IndexOf(null);
            if (index >= 0)
            {
                Managers.Inventory.Equipment[_equipmentView.SelectedIndex] = null;
                Managers.Inventory.Bag[index] = equipment;
                _bagView.Select(index);
            }
        }
        public void Drop()
        {
            if (SelectedItem != null)
            {
                Item item = (Item)SelectedItem;

                SelectedCollectionView.DataSource[SelectedCollectionView.SelectedIndex] = null;
                Managers.Inventory.DropItems.Add(item);

                SelectDefault();
            }
        }
        private void ApplyPotion()
        {
            var potion = (Potion)SelectedItem;
            if (potion.Skill != null && Managers.Player.Player.AddSkill(potion.Skill, Managers.Player.Player))
            {
                potion.Stack.Current--;
                Managers.Inventory.Bag.ResetItem(SelectedCollectionView.SelectedIndex);
                if (potion.Stack.Current <= 0.8)
                {
                    Managers.Inventory.Bag.RemoveAt(SelectedCollectionView.SelectedIndex);
                    SelectDefault();
                }
            }
        }
        public void Apply()
        {
            if (SelectedCollectionView == _bagView)
            {
                if (SelectedCollectionView.SelectedItem is Equipment)
                {
                    Equip();
                }
                else
                {
                    if (SelectedCollectionView.SelectedItem is Potion)
                    {
                        ApplyPotion();
                    }
                }
            }
        }
        private void EquipmentView_DoubleClick(object sender, CollectionViewClickEventArgs e)
        {
            Unequip();
        }
        private void EquipmentView_LongClick(object sender, CollectionViewClickEventArgs e)
        {
            Unequip();
        }
        private void Bag_DoubleClick(object sender, CollectionViewClickEventArgs e)
        {
            Apply();
        }
        private void BagView_LongClick(object sender, CollectionViewClickEventArgs e)
        {
            Apply();
        }
    }
}