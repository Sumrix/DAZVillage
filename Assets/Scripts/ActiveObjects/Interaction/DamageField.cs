using UnityEngine;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    public class DamageField :
        MonoBehaviour
    {
        [Tooltip("Количество наносимого урона.")]
        public float Damage;
        [Tooltip("Область нанесения урона.")]
        public FieldTrigger Field;
        [Tooltip("Периодичность нанесения урона.")]
        public TimerTrigger PeriodTimer;
        [Tooltip("Продолжительность действия ауры.")]
        public TimerTrigger DurationTimer;
        /// <summary>
        /// Создатель скила
        /// </summary>
        [HideInInspector]
        public int CreatorTeam;

        private void Start()
        {
            PeriodTimer.Active += PeriodTimer_Active;
            DurationTimer.Active += DurationTimer_Active;
            DurationTimer.IsActiveStart = false;

            Field.Enable();
            PeriodTimer.Enable();
            DurationTimer.Enable();
        }
        private void PeriodTimer_Active(object sender, System.EventArgs e)
        {
            foreach (Character character in Field.SelectList)
            {
                if (character.Team != CreatorTeam)
                {
                    character.ChangeHealth(-Damage, null);
                }
            }
        }
        private void DurationTimer_Active(object sender, System.EventArgs e)
        {
            Field.Disable();
            PeriodTimer.Disable();
            if (this && gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}