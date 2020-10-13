using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class KilledCounter :
        MonoBehaviour,
        IManagerWaiter
    {
        public Text CounterText;

        void Start()
        {
            CounterText.text = "0";
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            Managers.Statistic.Changed += Statistic_Changed;
        }

        private void Statistic_Changed(object sender, System.EventArgs e)
        {
            CounterText.text = Managers.Statistic.ZombieKilledCount.ToString();
        }
    }
}