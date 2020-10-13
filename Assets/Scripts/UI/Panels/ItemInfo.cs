using UnityEngine;
using System.Collections;
using System.Linq;

namespace GameUI
{
    public class ItemInfo :
        DescriptionPanel
    {
        [SerializeField]
        [RequiredField]
        protected ItemDescription _itemDescription;
        [SerializeField]
        [RequiredField]
        protected WeaponDescription _weaponDescription;
        [SerializeField]
        [RequiredField]
        protected ArmorDescription _armorDescription;
        protected DescriptionPanel currentDescription;
        
        public override void SetItem(object obj)
        {
            if (currentDescription != null)
            {
                Destroy(currentDescription.gameObject);
            }

            Item item;
            var inspectorItem = obj as InspectorItem;
            if (inspectorItem != null)
            {
                item = inspectorItem.Prefab;
            }
            else
            {
                item = (Item)obj;
            }

            if (item is Weapon)
            {
                var description = Instantiate(_weaponDescription);
                description.transform.SetParent(transform, false);
                description.Set(obj);
                currentDescription = description;
            }
            else if (item is Armor)
            {
                var description = Instantiate(_armorDescription);
                description.transform.SetParent(transform, false);
                description.Set(obj);
                currentDescription = description;
            }
            else
            {
                var description = Instantiate(_itemDescription);
                description.transform.SetParent(transform, false);
                description.Set(obj);
                currentDescription = description;
            }
        }
        public override void SetDefault()
        {
            if (currentDescription != null)
            {
                currentDescription.SetDefault();
            }
        }
    }
}