using UnityEngine;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    namespace GameSkill
    {
        [RequireComponent(typeof(FieldTrigger))]
        public class DamageAura :
            Skill
        {
            [Tooltip("Количество наносимого урона.")]
            public float Damage;
            [Tooltip("Область нанесения урона.")]
            public FieldTrigger Field;
            [Tooltip("Периодичность нанесения урона.")]
            public TimerTrigger PeriodTimer;

            protected override void Awake()
            {
                base.Awake();

                PeriodTimer.Active += Timer_Active;
            }
            protected override void Activate()
            {
                Field.Enable();
                PeriodTimer.Enable();
            }
            protected override void Deactivate()
            {
                Field.Disable();
                PeriodTimer.Disable();
            }
            private void Timer_Active(object sender, System.EventArgs e)
            {
                foreach (Character character in Field.SelectList)
                {
                    if (character.Team != Owner.Team)
                    {
                        character.ChangeHealth(-Damage, Creator);
                    }
                }
            }
        }
    }
}