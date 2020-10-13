using System;
using ActiveObjects.Triggers;
using UnityEngine;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class Treatment :
            Skill
        {
            [Tooltip("Количество восстанавливаемой жизни.")]
            public float Health;
            [Tooltip("Время работы скила.")]
            public TimerTrigger DurationTimer;
            [Tooltip("Периодичность лечения.")]
            public TimerTrigger PeriodTimer;

            protected override void Awake()
            {
                base.Awake();

                DurationTimer.Active += DurationTimer_Active;
                DurationTimer.IsActiveStart = false;
                DurationTimer.IsRepetitive = false;
                PeriodTimer.Active += PeriodTimer_Active;
            }
            protected override void Activate()
            {
                DurationTimer.Enable();
                PeriodTimer.Enable();
            }
            protected override void Deactivate()
            {
                PeriodTimer.Disable();
                DurationTimer.Disable();
            }
            private void DurationTimer_Active(object sender, EventArgs e)
            {
                Disable();
            }
            private void PeriodTimer_Active(object sender, EventArgs e)
            {
                Owner.ChangeHealth(Health, Creator);
            }
        }
    }
}