using UnityEngine;
using UnityEngine.UI;
using System.Collections.Specialized;
using Collections;
using ActiveObjects.GameSkill;

namespace GameUI
{
    public class AmmoCounter :
        MonoBehaviour,
        IManagerWaiter
    {
        public Text CounterText;
        public Sprite NullAmmoSprite;
        public Image AmmoImage;
        private Item _lastAmmoType = null;

        void Start()
        {
            CounterText.text = "0";
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            Managers.Inventory.CurrentAmmoChange += Inventory_CurrentAmmoChange;
        }
        private void Inventory_CurrentAmmoChange(object sender, System.EventArgs e)
        {
            CounterText.text = Managers.Inventory.CurrentAmmoCount.ToString();
            var ammo = Managers.Inventory.CurrentAmmoType;
            if (ammo != _lastAmmoType)
            {
                AmmoImage.sprite = ammo != null ? ammo.Sprite : NullAmmoSprite;
                _lastAmmoType = ammo;
            }
        }
        //private void Inventory_Changed(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    var weapon = (Weapon)Managers.Inventory.Equipment[EquipmentType.Weapon];
        //    if (weapon != null)
        //    {
        //        var shootingSkill = weapon.Skill as Shooting;
        //        if (shootingSkill != null && shootingSkill.Ammo != null)
        //        {
        //            int count = ItemCollection.Count(Managers.Inventory.Bag, shootingSkill.Ammo);
        //            CounterText.text = count.ToString();
        //            AmmoImage.sprite = shootingSkill.Ammo.Sprite;
        //            return;
        //        }
        //    }
        //    CounterText.text = "0";
        //    AmmoImage.sprite = NullAmmoSprite;
        //}
    }
}