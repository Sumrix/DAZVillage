using UnityEngine;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class Heal :
            Skill
        {
            [Tooltip("Количество восстанавливаемой жизни.")]
            public int Health;

            protected override void Activate()
            {
                Owner.ChangeHealth(Health, Creator);
                Disable();
            }
        }
    }
}