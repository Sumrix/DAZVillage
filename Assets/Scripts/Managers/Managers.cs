using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(GraphicSettings))]

public class Managers : MonoBehaviour
{
	public static ManagerStatus Status { get; private set; }

    public static SpriteManager Sprites { get; private set; }
    public static Database Database { get; private set; }
    public static InventoryManager Inventory { get; private set; }
	public static PlayerManager Player { get; private set; }
	public static UIManager UI { get; private set; }
    public static GameManager Game { get; private set; }
    public static DayTime Time { get; private set; }
    public static AudioManager Audio { get; private set; }
    public static GraphicSettings Graphic { get; private set; }
    public static StatisticManager Statistic { get; private set; }

    private List<IGameManager> _startSequence;
	private static List<IManagerWaiter> _managerWaiters;
    
    void Awake()
    {
        Status = ManagerStatus.Initializing;
        _managerWaiters = new List<IManagerWaiter>();

        Random.InitState(System.DateTime.Now.Second);
        Time = GameObject.Find("Game Managers").GetComponent<DayTime>();
        Graphic = GetComponent<GraphicSettings>();

        Sprites = GetComponent<SpriteManager>();
        Database = GetComponent<Database>();
        Inventory = GetComponent<InventoryManager>();
		Player = GetComponent<PlayerManager>();
		UI = GetComponent<UIManager>();
        Game = GetComponent<GameManager>();
        Statistic = GetComponent<StatisticManager>();
        Audio = GetComponent<AudioManager>();
        _startSequence = new List<IGameManager>();
        _startSequence.Add(Sprites);
        _startSequence.Add(Database);
        _startSequence.Add(Player);
        _startSequence.Add(Inventory);
        _startSequence.Add(UI);
        _startSequence.Add(Game);
        _startSequence.Add(Statistic);
        _startSequence.Add(Audio);
        StartCoroutine(StartupManagers());
    }
    public static void Shutdown()
    {
        Status = ManagerStatus.Shutdown;
    }
    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;
            foreach (IGameManager manager in _startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }
            if (numReady > lastReady)
                Debug.Log("Managers progress: " + numReady + "/" + numModules);
            yield return null;
        }

		Status = ManagerStatus.Started;
        Debug.Log("All managers started up");
		StartWaiters ();
    }
	public void StartWaiters()
	{
		foreach (var item in _managerWaiters) {
			item.Startup ();
		}
	}
	public static void AddWaiter(IManagerWaiter waiter)
	{
		if (Status == ManagerStatus.Started) {
			waiter.Startup ();
		} else {
			_managerWaiters.Add (waiter);
		}
	}
}