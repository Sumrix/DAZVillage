using System.Collections.Generic;

public static class EquipmentTypeToString
{
    private static readonly Dictionary<EquipmentType, string> _items;

    static EquipmentTypeToString()
    {
        _items = new Dictionary<EquipmentType, string>
        {
            {EquipmentType.Weapon, "Weapon"},
            {EquipmentType.Hat, "Hat"},
            {EquipmentType.Armor, "Armor"},
            {EquipmentType.Gloves, "Gloves"},
            {EquipmentType.Pants, "Pants"},
            {EquipmentType.Boots, "Boots"},
        };
    }
    public static string Convert(EquipmentType equipmenType)
    {
        return _items[equipmenType];
    }
}
