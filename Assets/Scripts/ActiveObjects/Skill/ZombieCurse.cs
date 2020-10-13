using UnityEngine;
using System;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class ZombieCurse :
            Skill
        {
            [RequiredField]
            [Tooltip("Префаб огня.")]
            public Transform FirePrefab;
            [Tooltip("Урон.")]
            public float Damage;
            [Tooltip("Время дня при котором работает скил.")]
            public TimeUnitTrigger DayTime;
            [Tooltip("Периодичность нанесения урона в секундах.")]
            public TimerTrigger Timer;

            protected override void Awake()
            {
                base.Awake();

                DayTime.Active += DayTime_Active;
                DayTime.Deactive += DateTime_Deactive;
                Timer.Active += Timer_Active;
            }
            protected override void Activate()
            {
                DayTime.Enable();
            }
            protected override void Deactivate()
            {
                DayTime.Disable();
                Timer.Disable();
            }
            private void DayTime_Active(object sender, EventArgs e)
            {
                FirePrefab.gameObject.SetActive(true);
                Timer.Enable();
            }
            private void DateTime_Deactive(object sender, EventArgs e)
            {
                FirePrefab.gameObject.SetActive(false);
                Timer.Disable();
            }
            private void Timer_Active(object sender, EventArgs e)
            {
                Owner.ChangeHealth(-Damage, Creator);
            }
        }
    }
}