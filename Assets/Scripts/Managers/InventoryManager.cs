using UnityEngine;
using GameUI;
using System;
using Collections;
using System.Collections.Specialized;
using ActiveObjects;
using System.Linq;

[Serializable]
public class InventoryManager :
	MonoBehaviour,
    IGameManager,
    IManagerWaiter
{
    #region Fields

    public event EventHandler<DroppedEventArgs> Dropped;

    public ManagerStatus Status { get; private set; }
    [HideInInspector]
    public ObservableCollection<Item> Bag;
    [HideInInspector]
    public ObservableKeyedCollection<EquipmentType, Equipment> Equipment;
    [HideInInspector]
    public ObservableCollection<Item> DropItems;
    [HideInInspector]
    public ObservableCollection<Item> Chest;

    [SerializeField]
    [Header("Equipment")]
    [RequiredField]
    private CollectionView _equipmentView;
    [SerializeField]
    [RequiredField]
    private GameObjectCollection _equipmentGOItems;

    [SerializeField]
    [Header("Bag")]
    private int _bagSlotCount = 30;

    [SerializeField]
    [RequiredField]
    private CollectionView _bagView;
    [SerializeField]
    [RequiredField]
    private GameObjectCollection _bagGOItems;
    [SerializeField]
    private InspectorItem[] _bagItems;

    [SerializeField]
    [Header("Chest")]
    private int _chestSlotCount = 30;
    [SerializeField]
    [RequiredField]
    private GameObjectCollection _chestGOItems;
    [SerializeField]
    private InspectorItem[] _chestItems;

    public int CurrentAmmoCount;
    public Item CurrentAmmoType;
    public event EventHandler CurrentAmmoChange;
    #endregion

    #region Constructor
    
    public void Startup()
    {
        Status = ManagerStatus.Initializing;
        Debug.Log("Inventory manager is starting...");

        InitBag();
        InitEquipment();
        InitDrop();
        InitChest();

        // Subscribe to events
        Managers.AddWaiter(this);
        // Initializing complete
        Status = ManagerStatus.Started;
        Debug.Log("Inventory manager is started.");
    }
    private void InitBag()
    {
        // Инициализируем сумку массивом _bagSlotCount нулей
        Bag = new ObservableCollection<Item>(new Item[_bagSlotCount]);

        // Добавляем в коллекцию предметы из списка инспектора
        for (int i = 0; i < _bagItems.Length; i++)
        {
            var item = _bagItems[i].Instantiate();
            if (item != null)
            {
                item.gameObject.SetActive(false);
                Bag[i] = item;
            }
        }

        // Связываем коллекцию с представлением
        _bagView.DataSource = Bag;
        _bagGOItems.DataSource = Bag;
        Bag.CollectionChanged += Bag_CollectionChanged;
        //Bag.CollectionChanged += Bag_CollectionChanged;
    }
    // Показать изменение сумки
    //private void Bag_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //{
    //    Func<System.Collections.IEnumerable, string> show = (c) => {
    //        string s = "[";
    //        foreach (var item in c)
    //        {
    //            if (item == null)
    //                s += "null, ";
    //            else
    //                s += item.ToString() + ", ";
    //        }
    //        return s.Substring(0, s.Length-2) + "]";
    //    };
    //    print(string.Format("{0} item in bag from({1}:{2}) to({3}:{4})",
    //        e.Action,
    //        e.NewStartingIndex, show(e.NewItems),
    //        e.OldStartingIndex, show(e.OldItems)));
    //}

    private void InitEquipment()
    {
        // Создаём коллекцию и передаём ей функцию получение ключа из элемента коллекции
        Equipment = new ObservableKeyedCollection<EquipmentType, Equipment>(x => x.EquipmentType);
        
        // Инициализируем коллекцию null-ами
        foreach (var item in Enum.GetValues(typeof(EquipmentType)))
        {
            Equipment.Add((EquipmentType)item, null);
        }

        // Связываем коллекцию с представлением
        _equipmentView.DataSource = Equipment;
        _equipmentGOItems.DataSource = Equipment;
        Equipment.CollectionChanged += Equipment_CollectionChanged;
    }
    private void InitDrop()
    {
        DropItems = new ObservableCollection<Item>();
        DropItems.CollectionChanged += DropItems_CollectionChanged; ;
    }
    private void InitChest()
    {
        // Инициализируем сумку массивом _chestSlotCount нулей
        Chest = new ObservableCollection<Item>(new Item[_chestSlotCount]);

        // Добавляем в коллекцию предметы из списка инспектора
        for (int i = 0; i < _chestItems.Length; i++)
        {
            var item = _chestItems[i].Instantiate();
            if (item != null)
            {
                item.gameObject.SetActive(false);
                Chest[i] = item;
            }
        }

        // Связываем коллекцию с представлением
        _chestGOItems.DataSource = Chest;
    }
    void IManagerWaiter.Startup()
    {
        Managers.Player.Player.GetComponent<PickUper>().PickUpped += PickUper_PickUpped;

    }

    #endregion

    #region Event Methods

    private void DropItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            DelayHelper.DelayedAction(() =>
            {
                foreach (var item in e.NewItems)
                {
                    DropItems.Remove((Item)item);
                }
            }, 3);
            foreach (var item in e.NewItems)
            {
                OnDrop(new DroppedEventArgs { Item = (Item)item });
            }
        }
    }
    private void PickUper_PickUpped (object sender, ActiveObjects.PickUperEventArgs e)
    {
        // Предмет копируется, исходный предмет удаляется в PickUper
        // Потому что метод Fill должен работать и с префабами и с инстансами
        if (!DropItems.Contains(e.Item)) //если объект не был выброшен только что
        {
            var weapon = e.Item as Weapon;
            if (weapon != null && Equipment[EquipmentType.Weapon] == null)
            {
                Equipment[EquipmentType.Weapon] = Instantiate(weapon);
            }
            else
            {
                int index = Bag.IndexOf(null);

                if (index >= 0)
                {
                    var rest = ItemCollection.Fill(Bag, e.Item, Mathf.RoundToInt(e.Item.Stack.Current));
                    if (rest) Drop(rest);
                }
                e.Cancel = index == -1;
            }
        }
        else
        {
            e.Cancel = true;
        }
    }
    private void Equipment_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems.OfType<Weapon>().Any() || e.OldItems.OfType<Weapon>().Any())
        {
            var weapon = (Weapon)Equipment[EquipmentType.Weapon];
            if (weapon != null && weapon.Skill != null && weapon.Skill.Ammo != null)
            {
                CurrentAmmoType = weapon.Skill.Ammo;
                CurrentAmmoCount = ItemCollection.Count(Bag, CurrentAmmoType);
            }
            else
            {
                CurrentAmmoType = null;
                CurrentAmmoCount = 0;
            }
            OnCurrentAmmoChange();
        }
    }
    private void Bag_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (CurrentAmmoType != null)
        {
            int count = ItemCollection.Count(Bag, CurrentAmmoType);
            if (count != CurrentAmmoCount)
            {
                CurrentAmmoCount = count;
                OnCurrentAmmoChange();
            }
        }
    }
    public void Drop(Item item)
    {
        DropItems.Add(item);
        item.transform.position = Managers.Player.Player.transform.position;
    }
    private void OnDrop(DroppedEventArgs e)
    {
		var handler = Dropped;
		if (handler != null)
		{
			handler (this, e);
		}
    }
    private void OnCurrentAmmoChange()
    {
        var handler = CurrentAmmoChange;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    #endregion
}

public class DroppedEventArgs :
    EventArgs
{
    public Item Item { get; set; }
}