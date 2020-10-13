using UnityEngine;
using System.Collections.Generic;
using ActiveObjects.Triggers;
using System.Linq;

namespace ActiveObjects
{
    public class ZombieSpawner :
        MonoBehaviour
    {
        [RequiredField]
        public Character ZombiePrefab;
        public TimerTrigger PeriodTimer;
        public ZombieDay[] SpawnDays;
        private List<Character> _zombies = new List<Character>();

        private TimeUnitTrigger _timeUnitTrigger = new TimeUnitTrigger();

        [HideInInspector]
        public bool IsEnable = true;

        public GameObject[] spawners;
        private Vector3[] spawnPoints;
        private void Start()
        {
            _timeUnitTrigger.TimeUnit = TimeUnit.Night;

            _timeUnitTrigger.Active += TimeUnitTrigger_Active;
            _timeUnitTrigger.Deactive += TimeUnitTrigger_Deactive;
            PeriodTimer.Active += PeriodTimer_Active;

            _timeUnitTrigger.Enable();

            spawnPoints = new Vector3[spawners.Length];
            for (int i = 0; i < spawners.Length; i++)
            {
                spawnPoints[i] = spawners[i].transform.position;
            }
        }
        // Включение и выключение спаунера извне (меню читов)
        public void Enable()
        {
            if (!IsEnable)
            {
                IsEnable = true;

                _timeUnitTrigger.Enable();
            }
        }
        public void Disable()
        {
            if (IsEnable)
            {
                IsEnable = false;

                _timeUnitTrigger.Disable();
                PeriodTimer.Disable();
            }
        }
        private void Spawn()
        {
            // Сперва спаунится по расписанию, потом по последнему дню расписания
            var currentDay = Mathf.Min(Managers.Time.Day, SpawnDays.Length - 1);
            foreach (var zombieType in SpawnDays[currentDay].DayZombies)
            {
                zombieType.Spawn(spawnPoints, transform);
            }
        }
        private Character InstantiateZombie()
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            var go = Instantiate(ZombiePrefab, spawnPoints[spawnPointIndex], Quaternion.identity, transform);
            return go as Character;
        }
        private void TimeUnitTrigger_Active(object sender, System.EventArgs e)
        {
            PeriodTimer.Enable();
        }
        private void TimeUnitTrigger_Deactive(object sender, System.EventArgs e)
        {
            PeriodTimer.Disable();
        }
        private void PeriodTimer_Active(object sender, System.EventArgs e)
        {
            Spawn();
        }
    }
}