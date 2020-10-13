using Collections;
using System;
using ActiveObjects.Triggers;
using UnityEngine;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class Shooting :
            Skill
        {
            [Tooltip("Префаб расходуемых патронов.")]
            public Item Ammo;
            [Tooltip("Наносимый урон.")]
            public float Damage;
            [Tooltip("Опции выстрела.")]
            public ShotTrigger shot;
            [Tooltip("Время между выстрелами в секундах.")]
            public TimerTrigger PeriodTimer;
            [Tooltip("Модификаторы накладываемые на цель при попадании.")]
            public Skill[] Modifiers;
            [Tooltip("Звук выстрела")]
            public AudioClip ShotSound;

            protected override void Awake()
            {
                base.Awake();

                shot.Hit += Shot_Hit;
                PeriodTimer.Active += PeriodTimer_Active;
            }
            protected override void Activate()
            {
                PeriodTimer.Enable();
            }
            protected override void Deactivate()
            {
                PeriodTimer.Disable();
            }
            private void PeriodTimer_Active(object sender, EventArgs e)
            {
                if (!Ammo || ItemCollection.TryRemove(Managers.Inventory.Bag, Ammo, 1))
                {
                    shot.Fire();
                    if (ShotSound != null)
                    {
                        AudioSource.PlayOneShot(ShotSound);
                    }
                }
            }
            private void Shot_Hit(object sender, SelectEventArgs e)
            {
                foreach (Character character in e.TargetList)
                {
                    if (character != Owner)
                    {
                        character.ChangeHealth(-Damage, Creator);

                        foreach (var skill in Modifiers)
                        {
                            character.AddSkill(skill, Creator);
                        }
                    }
                }
            }
        }
    }
}