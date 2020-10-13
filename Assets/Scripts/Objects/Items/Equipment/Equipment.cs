using System;
using System.Collections.Generic;
using GameUI;

public abstract class Equipment :
	Item
{
    public EquipmentType EquipmentType;
    public InspectorItem[] ImproveRecipe;
    // Passive Skill

    public EquipmentStatsManager Stats;
    public CharacterStatsManager Bonus;
    
    private void Awake()
    {
        Stats.Reset();
        Bonus.Reset();
    }
    public virtual void LevelUp()
    {
        Stats.Level++;
        Bonus.Level++;
        Stats.Reset();
        Bonus.Reset();
        // Up Passive Skill
    }
}