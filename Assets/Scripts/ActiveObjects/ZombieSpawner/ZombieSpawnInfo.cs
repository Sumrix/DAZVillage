using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveObjects
{
    [Serializable]
    public class ZombieSpawnInfo
    {
        [Tooltip("Префаб зомби.")]
        [RequiredField]
        public Character Prefab;
        [Tooltip("Количество зомби данного типа в начале ночи.")]
        public int Min;
        [Tooltip("Количество зомби данного типа в конце ночи.")]
        public int Max;
        [FloatRange(0,1)]
        public FloatRange Time;
        [HideInInspector]
        public List<Character> Zombies = new List<Character>();

        public void Spawn(Vector3[] spawnPoints, Transform parent)
        {
            // Убираем null
            Zombies = Zombies.OfType<Character>().ToList();
            // Если время спауна пришло
            if (Time.InRange(Managers.Time.NightProgress))
            {
                // Вычисляем нужное количество зомби
                float spawnProgress = (Managers.Time.NightProgress - Time.RangeStart) / Time.Size;
                int requiredCount = Mathf.RoundToInt((Max - Min) * spawnProgress + Min);
                // Спауним недостающих зомби
                for (int count = Zombies.Count; count < requiredCount; count++)
                {
                    int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                    var spawnPoint = spawnPoints[spawnPointIndex];
                    var go = GameObject.Instantiate(Prefab, spawnPoint, Quaternion.identity, parent);
                    Zombies.Add((Character)go);
                }
            }
        }
    }
}