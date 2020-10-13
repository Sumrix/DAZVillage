using System.Collections.Generic;

public static class ItemTypeToString
{
    private static readonly Dictionary<ItemType, string> _items;

    static ItemTypeToString()
    {
        _items = new Dictionary<ItemType, string>
        {
            //{ItemType.HunterSet, "Hunter Set"},
            //{ItemType.MedicSet, "Medic Set"},
            //{ItemType.PoacherSet, "Poacher Set"},
            {ItemType.WarriorSet, "Warrior Set"},
            {ItemType.Ammunition, "Ammunition"},
            {ItemType.Weapon, "Weapon"},
            {ItemType.Other, "Other"}
        };
    }
    public static string Convert(ItemType itemType)
    {
        return _items[itemType];
    }
}
