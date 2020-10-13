using UnityEngine;
using System.Collections.Generic;
using System;

namespace ActiveObjects
{
    namespace Triggers
    {
        /// <summary>
        /// Разброс пуль
        /// </summary>
        [Serializable]
        public class BulletsScatter
        {
            [Tooltip("Среднее расстояние полёта пули.")]
            public float AverageRange;
            [Tooltip("Разброс по дальности.")]
            public float RangeScatter;
            [Tooltip("Разброс по углу.")]
            public float AngleScatter;
            [Tooltip("Количество снарядов за выстрел.")]
            public int ProjectileCount;

            /// <summary>
            /// Сгенерировать конечные точки полёта пуль
            /// </summary>
            /// <param name="shooter">GameObject стреляющего объекта</param>
            /// <returns>Массив конечных точек полёта пуль</returns>
            public Vector3[] GenerateBulletTargets(GameObject shooter)
            {
                List<Vector3> targets = new List<Vector3>();
                for (int i = 0; i < ProjectileCount; i++)
                {
                    float angle = UnityEngine.Random.Range(-AngleScatter / 2, AngleScatter / 2);
                    float range = UnityEngine.Random.Range(AverageRange - RangeScatter / 2, AverageRange + RangeScatter / 2);
                    Vector3 shootDirection = Quaternion.AngleAxis(angle, shooter.transform.up) * shooter.transform.forward;
                    Vector3 target = shootDirection * range + shooter.transform.position + Vector3.up;
                    targets.Add(target);
                }
                return targets.ToArray();
            }
        }
    }
}