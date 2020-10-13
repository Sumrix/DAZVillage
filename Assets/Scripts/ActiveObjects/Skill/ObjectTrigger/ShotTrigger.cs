using UnityEngine;
using System;

namespace ActiveObjects
{
    namespace Triggers
    {
        /// <summary>
        /// Триггер выпускающий снаряды и реагирующий на попадания
        /// </summary>
        [Serializable]
        public class ShotTrigger
        {
            /// <summary>
            /// Триггер отреагировал на новый объект
            /// </summary>
            public event EventHandler<SelectEventArgs> Hit;
            /// <summary>
            /// Снаряд
            /// </summary>
            [RequiredField]
            [Tooltip("Префаб снаряда.")]
            public Projectile Projectile;
            /// <summary>
            /// Способ выбора цели
            /// </summary>
            [Tooltip("Разброс пуль.")]
            public BulletsScatter BulletsScatter;
            /// <summary>
            /// Объект откуда выпускаются снаряды
            /// </summary>
            [RequiredField]
            [Tooltip("GameObject стрелка.")]
            public GameObject Shooter;

            public void Fire()
            {
                foreach (var target in BulletsScatter.GenerateBulletTargets(Shooter.gameObject))
                {
                    var instance = Projectile.Instantiate(Projectile);
                    instance.transform.position = Shooter.transform.position + Vector3.up;
                    instance.Target = target;

                    var objectTrigger = instance.GetComponent<ObjectTrigger>();
                    objectTrigger.Select += ObjectTrigger_Select;
                    objectTrigger.Enable();
                }
            }
            private void ObjectTrigger_Select(object sender, SelectEventArgs e)
            {
                OnHit(e);
            }
            private void OnHit(SelectEventArgs e)
            {
                var handler = Hit;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }
    }
}