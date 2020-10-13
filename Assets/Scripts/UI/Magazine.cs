using UnityEngine;
using UnityEngine.UI;
using System;
using ActiveObjects.GameSkill;

namespace GameUI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class Magazine :
        MonoBehaviour,
        IManagerWaiter
    {
        public Image AmmoPrefab;
        private GridLayoutGroup _grid;
        private RectTransform _rect;
        private Item _lastAmmo;
        private Shooting _lastSkill;
        
        private void Start()
        {
            Managers.AddWaiter(this);
            _grid = GetComponent<GridLayoutGroup>();
            _rect = GetComponent<RectTransform>();
        }
        void IManagerWaiter.Startup()
        {
            Managers.Player.AttackSkillChange += Player_AttackSkillChange;
            Managers.Inventory.CurrentAmmoChange += Inventory_CurrentAmmoChange;
        }
        private void Player_AttackSkillChange(object sender, EventArgs e)
        {
            if (_lastSkill != null)
            {
                _lastSkill.PeriodTimer.Active -= PeriodTimer_Active;
            }
            _lastSkill = Managers.Player.AttackSkill;
            if (_lastSkill != null)
            {
                _lastSkill.PeriodTimer.Active += PeriodTimer_Active;
            }
            Deactivate();
            Activate();
        }
        private void PeriodTimer_Active(object sender, EventArgs e)
        {
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            }
            if (transform.childCount <= 1)
            {
                var weapon = (Weapon)Managers.Inventory.Equipment[EquipmentType.Weapon];
                int count = Mathf.Min(weapon.MagazineCapacity, Managers.Inventory.CurrentAmmoCount);
                for (int i = 0; i < count; i++)
                {
                    Instantiate(AmmoPrefab, transform, false);
                }
            }
        }
        private void Inventory_CurrentAmmoChange(object sender, EventArgs e)
        {
            if (Managers.Inventory.CurrentAmmoType != _lastAmmo)
            {
                _lastAmmo = Managers.Inventory.CurrentAmmoType;
                Deactivate();
                if (Managers.Inventory.CurrentAmmoType != null)
                {
                    Activate();
                }
            }
            else
            {
                if (transform.childCount > Managers.Inventory.CurrentAmmoCount)
                {
                    Deactivate();
                    Activate();
                }
                else
                {
                    if (transform.childCount == 0 && Managers.Inventory.CurrentAmmoCount > 0)
                    {
                        Activate();
                    }
                }
            }

        }
        private void Activate()
        {
            if (Managers.Player.AttackSkill != null)
            {
                var weapon = (Weapon)Managers.Inventory.Equipment[EquipmentType.Weapon];
                int capacity = weapon.MagazineCapacity;
                int rows = capacity > 10 ? capacity > 22 ? capacity > 36 ? capacity > 57 ? 5 : 4 : 3 : 2 : 1;
                int columns = Mathf.CeilToInt(((float)capacity) / rows);
                float width, height;
                height = _rect.rect.height / rows - _grid.spacing.y * (1 - 1 / rows);
                width = _rect.rect.width / columns - _grid.spacing.x * (1 - 1 / columns);
                _grid.cellSize = new Vector2(width, height);

                int count = Mathf.Min(weapon.MagazineCapacity, Managers.Inventory.CurrentAmmoCount);
                for (int i = 0; i < count; i++)
                {
                    Instantiate(AmmoPrefab, transform, false);
                }
            }
        }
        private void Deactivate()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}