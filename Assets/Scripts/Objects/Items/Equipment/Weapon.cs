using ActiveObjects.GameSkill;
using ActiveObjects.Triggers;
using UnityEngine;

public class Weapon :
	Equipment
{
    [Tooltip("Скилл атаки.")]
    public Shooting Skill;
    [Tooltip("Вместимость магазина.")]
    public int MagazineCapacity;
    [Tooltip("Время перезарядки в секундах.")]
    public TimerTrigger RechargeTimer;
    [HideInInspector]
    public int AmmoCount;

    public override void LevelUp()
    {
        base.LevelUp();
        // Up Attack Skill
    }
    public Weapon()
    {
        ItemType = ItemType.Weapon;
        EquipmentType = EquipmentType.Weapon;
    }
}