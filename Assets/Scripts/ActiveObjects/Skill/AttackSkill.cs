using UnityEngine;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class AttackSkill :
            Skill
        {
            [Tooltip("Время между выстрелами в секундах.")]
            public TimerTrigger PeriodTimer;
            [Tooltip("Время между выстрелами в секундах.")]
            public TimerTrigger RechargeTimer;
        }
    }
}