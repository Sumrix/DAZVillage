using UnityEngine;
using ActiveObjects.Triggers;
using System.Linq;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class ToxicExplosion :
            Skill
        {
            [RequiredField]
            public DamageField DamageAura;
            public FieldTrigger FieldTrigger;

            protected override void Awake()
            {
                base.Awake();
                Owner.Dead += Owner_Dead;
                FieldTrigger.Select += FieldTrigger_Select;
                FieldTrigger.Enable();
            }
            private void FieldTrigger_Select(object sender, System.EventArgs e)
            {
                if (FieldTrigger
                    .SelectList
                    .OfType<Character>()
                    .Any(x => x.Team != Owner.Team))
                {
                    Owner.Kill(Owner);
                }
            }
            private void Owner_Dead(object sender, DeadEventArgs e)
            {
                var skill = GameObject.Instantiate(DamageAura);
                skill.transform.position = transform.position;
                skill.CreatorTeam = Owner.Team;
            }
        }
    }
}