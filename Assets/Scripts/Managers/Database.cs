using UnityEngine;
using System.Collections;

public class Database :
    MonoBehaviour,
    IGameManager
{
    public ManagerStatus Status { get; private set; }

    public static Item[] Items;
    public static Character[] Characters;

    public void Startup()
    {
        Status = ManagerStatus.Initializing;
        Debug.Log("Database manager is starting...");
        StartCoroutine(StartupDatabases());
    }
    private IEnumerator StartupDatabases()
    {
        Characters = Resources.LoadAll<Character>("Prefabs/Characters");
        yield return null;
        Items = Resources.LoadAll<Item>("Prefabs/Items");
        yield return null;
        Status = ManagerStatus.Started;
        Debug.Log("Database manager is started.");
    }
}