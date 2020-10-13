using UnityEngine;

namespace GameUI
{
    public class CheatsWindow :
        MonoBehaviour
    {
        [RequiredField]
        public ActiveObjects.ZombieSpawner ZombieSpawner;

        public void SetImmortal(bool value)
        {
            Managers.Player.IsImmortal = value;
            print("Immortal: " + value);
        }
        public void SetOffensiveMorning(bool value)
        {
            Managers.Time.IsOffensiveMorning = value;
            print("Offensive Morning: " + value);
        }
        public void SetOffensiveNight(bool value)
        {
            Managers.Time.IsOffensiveNight = value;
            print("Offensive Night: " + value);
        }
        public void SetZombieSpawn(bool value)
        {
            print("Zombie Spawn: " + value);
            if (value)
            {
                ZombieSpawner.Enable();
            }
            else
            {
                ZombieSpawner.Disable();
            }
        }
        public void SetCraftBaseResources(bool value)
        {
            Managers.Game.AllowCraftingBaseResources = value;
            print("Craft Base Resources: " + value);
        }
    }
}