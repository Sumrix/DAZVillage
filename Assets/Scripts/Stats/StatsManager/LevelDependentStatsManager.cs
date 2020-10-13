using UnityEngine;
using System;

[Serializable]
public class LevelDependentStatsManager <StatsType> :
    StatsManager<StatsType>
    where StatsType : Stats, new()
{
    public float Level = 1;
    [SerializeField]
    protected StatsType _base;
    [HideInInspector]
    public StatsType Bonus;
    [SerializeField]
    protected StatsType _levelBonus;
    [HideInInspector]
    public StatsType Main;

    public LevelDependentStatsManager()
    {
        Bonus = new StatsType();
    }
    public override void Reset()
    {
        Calc();
        OnChanged();
    }
    protected virtual void Calc()
    {
        Main = (StatsType)(_levelBonus * Level + _base);
        Result = (StatsType)(Main + Bonus);
    }
}