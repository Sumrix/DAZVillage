using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using GameUI;
using ActiveObjects.GameSkill;

/// <summary>
/// Класс персонажа
/// </summary>
public class Character :
	CoreObject
{
    /// <summary>
    /// Событие смерти персонажа
    /// </summary>
    public event EventHandler<DeadEventArgs> Dead;
    /// <summary>
    /// Описание
    /// </summary>
    public Description Description;
    /// <summary>
    /// Полоса жизни
    /// </summary>
    [SerializeField]
    private HealthBar _healthBar;
    /// <summary>
    /// Скилы
    /// </summary>
    [SerializeField]
    public List<Skill> Skills;
    /// <summary>
    /// Статы
    /// </summary>
    public CharacterStatsManager Stats;
    /// <summary>
    /// Номер команды
    /// </summary>
    public int Team;
    /// <summary>
    /// Жив ли персонаж
    /// </summary>
    [HideInInspector]
    public bool IsAlive = true;

    protected override void Start()
    {
        base.Start();
        // Посчитать начальные статы
        Stats.Reset();
        // Инициализируем работу полосы жизни
        if (_healthBar)
        {
            _healthBar = HealthBar.CreateInstance(_healthBar, this, Team);
        }
        // Инициализируем пассивные скилы
        for (int i = 0; i < Skills.Count; i++)
        {
            if (Skills[i] != null)
            {
                string name = Skills[i].name;
                Skills[i] = (Skill)Instantiate(Skills[i], transform, false);
                Skills[i].name = name;
                Skills[i].Creator = this;
            }
        }
        // Запускаем процесс регенерации
        InvokeRepeating("Regenerate", 0, 1f);
    }
    private void Regenerate()
    {
        if (Stats.Result.Health.CurrentNormalized < 1)
        {
            Stats.Result.Health.Current += Stats.Result.Health.Regen;
            Stats.ResetBindings();
        }
    }
    /// <summary>
    /// Убить персонажа
    /// </summary>
    /// <param name="Killer">Убийца</param>
    public void Kill(Character killer)
    {
        // Проверка чтобы предотвратить множественные вызовы и бесконечные циклы
        if (IsAlive)
        {
            IsAlive = false;
            // Должно выполняться в главном потоке
            MainThread.Invoke(() =>
            {
                // При каждом Disable скил удаляется
                for (int i = 0; i < Skills.Count;)
                {
                    if (Skills[i].IsActive)
                    {
                        Skills[i].Disable();
                    }
                    else
                    {
                        i++;
                    }
                }
                // Если убит зомби и убил игрок
                if (Team == 0 && killer == Managers.Player.Player)
                {
                    Managers.Statistic.AddKilledZombie();
                }
                OnDead(new DeadEventArgs { Dead = this, Killer = killer });
                Destroy();
            });
        }
    }
    /// <summary>
    /// Изменение жизни персонажа
    /// </summary>
    /// <param name="Value">Значение на которое измениться жизнь</param>
    /// <param name="changer">Персонаж который изменяет жизнь этого персонажа</param>
    public void ChangeHealth(float value, Character changer)
    {
        // Должно выполняться в главном потоке
        MainThread.Invoke(() =>
        {
            if (!(this == Managers.Player.Player && Managers.Player.IsImmortal && value < 0))
            {
                Stats.ChangeHealth(value);

                if (Stats.Result.Health.CurrentNormalized < Mathf.Epsilon)
                {
                    Kill(changer);
                }
            }
        });
    }
    /// <summary>
    /// Повесить баф на персонажа
    /// </summary>
    /// <param name="skill">скил бафа</param>
    /// <returns>true в случае успеха, иначе - false</returns>
    public bool AddSkill(Skill skill, Character creator)
    {
        if (skill.Launch == SkillLaunching.Auto && !Skills.Any(x => x.name == skill.name))
        {
            var instance = (Skill)Instantiate(skill, transform, false);
            instance.name = skill.name;
            instance.Creator = creator;
            Skills.Add(instance);
            instance.Deactive += Skill_Deactive;
            instance.Enable();
            return true;
        }
        return false;
    }
    /// <summary>
    /// Удаление бафа с персонажа
    /// </summary>
    /// <param name="skill">Скил бафа</param>
    public void RemoveSkill(Skill skill)
    {
        if (skill.Launch == SkillLaunching.Auto)
        {
            var instance = Skills.FirstOrDefault(x => x.name == skill.name);
            if (instance != null)
            {
                Skills.Remove(instance);
            }
        }
    }
    private void OnDestroy()
    {
        if (_healthBar)
        {
            _healthBar.Destroy();
        }
    }
    private void OnDead(DeadEventArgs e)
    {
        var handler = Dead;
        if (handler != null)
        {
            handler(this, e);
        }
    }
    private void Skill_Deactive(object sender, EventArgs e)
    {
        Skill skill = (Skill)sender;
        Skills.Remove(skill);
        skill.Destroy();
    }
}

public class DeadEventArgs :
    EventArgs
{
    public Character Dead;
    public Character Killer;
}