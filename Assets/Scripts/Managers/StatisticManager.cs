using UnityEngine;
using System;

public class StatisticManager :
    MonoBehaviour,
    IGameManager
{
    public event EventHandler Changed;
    public ManagerStatus Status { get; private set; }

    public int ZombieKilledCount;

    public void Startup()
    {
        Status = ManagerStatus.Started;
        Debug.Log("Statistic manager is started.");
    }
    public void AddKilledZombie()
    {
        ZombieKilledCount++;
        OnChanged();
    }
    private void OnChanged()
    {
        var handler = Changed;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }
}