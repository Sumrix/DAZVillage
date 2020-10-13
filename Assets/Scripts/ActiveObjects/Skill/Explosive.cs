using UnityEngine;
using ActiveObjects.Triggers;
using System.Linq;

namespace ActiveObjects
{
    namespace GameSkill
    {
        public class Explosive :
            Skill
        {
            public FieldTrigger FieldTrigger;
            public GameObject Explosion;
            public float Damage;
            public float ExplosionRadius;

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
                var obj = Instantiate(Explosion);
                obj.transform.position = Owner.transform.position;

                var characters = Physics.OverlapSphere(transform.position, ExplosionRadius)
                    .Select(x => x.GetComponent<Character>())
                    .OfType<Character>()
                    .ToList();
                foreach (var character in characters)
                {
                    if (character != Owner)
                    {
                        character.ChangeHealth(-Damage, Owner);
                    }
                }
            }
        }
    }
}