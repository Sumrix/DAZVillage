using System;

[Serializable]
public class EquipmentStats :
    ItemStats
{
    public Points Durability;

    public EquipmentStats()
    {
        Durability = new Points();
    }
}