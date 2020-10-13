using UnityEngine;
using System;

[Serializable]
public class StatsManager<StatsType>
    where StatsType : Stats, new()
{
    public event EventHandler Changed;

    [HideInInspector]
    public StatsType Result;
    
    public virtual void Reset()
    {
        OnChanged();
    }
    public void ResetBindings()
    {
        OnChanged();
    }
    protected void OnChanged()
    {
        var handler = Changed;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }
}