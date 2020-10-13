using UnityEngine;
using System;
using GameUI;
using System.Collections.Specialized;
using Collections;
using ActiveObjects.GameSkill;
using System.Linq;

public class PlayerManager :
	MonoBehaviour,
	IGameManager,
    IManagerWaiter
{
    #region Fields

    public ManagerStatus Status { get; private set; }
	[SerializeField]
    [RequiredField]
    private Transform _spawnPosition;
    [RequiredField]
    public Character Player;
    [RequiredField]
    public CameraScript Camera;
    [SerializeField]
    [RequiredField]
    private CollectionView _statsView;
    private ObservableCollection<CharacterStat> _stats;
    [SerializeField]
    [RequiredField]
    private HealthBar _healthBar;
    [HideInInspector]
    public bool IsImmortal = false;
    [HideInInspector]
    public bool AutoAiming = true;
    [HideInInspector]
    public Shooting AttackSkill;
    public event EventHandler AttackSkillChange;

    #endregion

    #region Constructor

    public void Startup()
    {
        Debug.Log("Player manager is starting...");

        _spawnPosition.gameObject.SetActive(false);
		InitPlayer ();
        InitStatsView();
        InitHealthBar();
        Managers.AddWaiter(this);
        
        Status = ManagerStatus.Started;
        Debug.Log("Player manager is started.");
    }
    private void InitPlayer()
    {
        Player = Instantiate(Player);
        Player.Dead += Player_Dead;
        Player.transform.position = _spawnPosition.position;
        Camera.target = Player.transform;
    }
    private void InitStatsView()
    {
        _stats = new ObservableCollection<CharacterStat>();
        _statsView.DataSource = _stats;

        var ieMain = Player.Stats.Main.GetEnumerator();
        var ieBonus = Player.Stats.Bonus.GetEnumerator();

        while (ieMain.MoveNext() && ieBonus.MoveNext())
        {
            if (ieMain.Current.Key != "Health.CurrentNormalized")
            {
                _stats.Add(new CharacterStat
                {
                    Name = ieMain.Current.Key,
                    Main = ieMain.Current.Value,
                    Bonus = ieBonus.Current.Value
                });
            }
        }
    }
    private void InitHealthBar()
    {
        _healthBar.SetTeam(Player.Team);
        Player.Stats.Changed += Stats_Changed;
        _healthBar.SetHealthVisual(Player.Stats.Result.Health.CurrentNormalized);
    }
    void IManagerWaiter.Startup()
    {
        Managers.Inventory.Equipment.CollectionChanged += Equipment_CollectionChanged;
    }

    #endregion

    #region Event Callbacks

    private void Stats_Changed(object sender, EventArgs e)
    {
        _healthBar.SetHealthVisual(Player.Stats.Result.Health.CurrentNormalized);

        int index = 0;
        var ieMain = Player.Stats.Main.GetEnumerator();
        var ieBonus = Player.Stats.Bonus.GetEnumerator();

        while (ieMain.MoveNext() && ieBonus.MoveNext())
        {
            if (ieMain.Current.Key != "Health.CurrentNormalized")
            {
                _stats[index].Main = ieMain.Current.Value;
                _stats[index].Bonus = ieBonus.Current.Value;
                _stats.ResetItem(index);
                index++;
            }
        }
    }
    private void Equipment_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // Обновление статов
        var bonus = new CharacterStats();
        foreach (var item in Managers.Inventory.Equipment)
        {
            if (item != null)
            {
                bonus += item.Bonus.Result;
            }
        }
        Player.Stats.Bonus = bonus;
        Player.Stats.Reset();
        // Обновление скила атаки
        if (e.NewItems.OfType<Weapon>().Any() || e.OldItems.OfType<Weapon>().Any())
        {
            var weapon = (Weapon)Managers.Inventory.Equipment[EquipmentType.Weapon];
            if (weapon != null && weapon.Skill != null)
            {
                AttackSkill = (Shooting)Instantiate(weapon.Skill, Player.transform, false);
                AttackSkill.Creator = Player;
                OnAttckSkillChange();
            }
            else
            {
                if (AttackSkill != null)
                {
                    AttackSkill.Destroy();
                    AttackSkill = null;
                    OnAttckSkillChange();
                }
            }
        }
    }
    private void Player_Dead(object sender, DeadEventArgs e)
    {
        Camera.target = null;
    }
    public void SetAutoAiming(bool value)
    {
        AutoAiming = value;
    }

    #endregion

    #region Event Methods

    private void OnAttckSkillChange()
    {
        var handler = AttackSkillChange;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    #endregion
}