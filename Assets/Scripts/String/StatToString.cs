using System.Collections.Specialized;

public static class StatToString
{
    private static readonly StringDictionary _items;

    static StatToString()
    {
        _items = new StringDictionary
        {
            {"Health.Maximum", "Health"},
            {"Health.Regen", "Health Regen"},
            {"Health.Current", "Current Health"},
            {"Strength", "Strength"},
            {"Agility", "Agility"},
            {"Intelligence", "Intelligence"},
            {"Health", "Health"},
            {"Level", "Level"},
            {"Damage", "Damage"},
            {"Armor", "Armor"},
            {"MoveSpeed", "Move speed"},
            {"IAS", "Attack speed"},
            {"Evasion", "Evasion"},
            {"Accuracy", "Accuracy"}
        };
    }
    public static string Convert(string name)
    {
        return _items[name];
    }
}
