using System;
using ActiveObjects.Triggers;
using UnityEngine;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class Burning :
            Skill
        {
            [Tooltip("Количество наносимого урона.")]
            public float Damage;
            [Tooltip("Периодичность нанесения урона.")]
            public TimerTrigger PeriodTimer;

            protected override void Awake()
            {
                base.Awake();

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
                Owner.ChangeHealth(-Damage, Creator);
            }
        }
    }
}