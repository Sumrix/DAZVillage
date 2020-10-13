using ActiveObjects.GameSkill;

public class Potion :
    Item
{
    public Skill Skill;

    public Potion()
    {
        ItemType = ItemType.Other;
    }
}
