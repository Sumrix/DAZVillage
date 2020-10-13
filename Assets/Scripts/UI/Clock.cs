using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class Clock :
        MonoBehaviour,
        IManagerWaiter
    {
        [RequiredField]
        public Image SunImage;
        [RequiredField]
        public Image MoonImage;
        [RequiredField]
        public Image DayProgress;
        [RequiredField]
        public Image NightProgress;

        private bool _inited = false;
        
        private void Start()
        {
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            _inited = true;
            Managers.Time.DayStart += Time_DayStart;
            Managers.Time.NightStart += Time_NightStart;
            if (Managers.Time.TimeUnit == TimeUnit.Day)
            {
                Time_DayStart(0);
            }
            else
            {
                Time_NightStart(0);
            }
        }
        private void Time_DayStart(int day)
        {
            SetDay();
        }
        private void Time_NightStart(int day)
        {
            SetNight();
        }
        private void Update()
        {
            if (_inited)
            {
                //Text.text = string.Format("{0}d{1}h", Managers.Time.Day, Mathf.RoundToInt(Managers.Time.Hours));
                if (Managers.Time.TimeUnit == TimeUnit.Day)
                {
                    SetDayTime();
                }
                else
                {
                    SetNightTime();
                }
            }
        }
        private void SetDay()
        {
            SunImage.gameObject.SetActive(true);
            MoonImage.gameObject.SetActive(false);
            DayProgress.fillClockwise = true;
            NightProgress.fillClockwise = false;
        }
        private void SetNight()
        {
            SunImage.gameObject.SetActive(false);
            MoonImage.gameObject.SetActive(true);
            DayProgress.fillClockwise = false;
            NightProgress.fillClockwise = true;
        }
        private void SetDayTime()
        {
            float time = Managers.Time.DayProgress;

            DayProgress.fillAmount = 1 - time / 2;
            NightProgress.fillAmount = time / 2;
        }
        private void SetNightTime()
        {
            float time = Managers.Time.NightProgress;

            DayProgress.fillAmount = time / 2;
            NightProgress.fillAmount = 1 - time / 2;
        }
    }
}