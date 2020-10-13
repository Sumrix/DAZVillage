using UnityEngine;
using System.Collections;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    public class AutoDestroy :
        MonoBehaviour
    {
        public TimerTrigger Timer;

        private void Awake()
        {
            Timer.Active += Timer_Active;
            Timer.IsActiveStart = false;
            Timer.IsRepetitive = false;
            Timer.Enable();
        }
        private void Timer_Active(object sender, System.EventArgs e)
        {
            if (gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}