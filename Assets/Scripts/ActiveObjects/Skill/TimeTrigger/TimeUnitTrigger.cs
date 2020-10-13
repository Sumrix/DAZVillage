using System;

namespace ActiveObjects
{
    namespace Triggers
    {
        /// <summary>
        /// Триггер реагирующий на изменение времени суток
        /// </summary>
        [Serializable]
        public class TimeUnitTrigger :
        TimeTrigger,
        IManagerWaiter
        {
            /// <summary>
            /// Время суток активности
            /// </summary>
            public TimeUnit TimeUnit;

            public override void Enable()
            {
                if (!IsEnable)
                {
                    IsEnable = true;

                    // На случай если Managers.Time ещё не инициализирован
                    Managers.AddWaiter(this);
                }
            }
            public void Startup()
            {
                if (IsEnable)
                {
                    switch (TimeUnit)
                    {
                        case TimeUnit.Day:
                            Managers.Time.DayStart += StartAtTime;
                            Managers.Time.NightStart += EndAtTime;
                            break;
                        case TimeUnit.Night:
                            Managers.Time.NightStart += StartAtTime;
                            Managers.Time.DayStart += EndAtTime;
                            break;
                    }
                    if (Managers.Time.TimeUnit == TimeUnit)
                    {
                        OnActive();
                    }
                }
            }
            public override void Disable()
            {
                if (IsEnable)
                {
                    IsEnable = false;

                    switch (TimeUnit)
                    {
                        case TimeUnit.Day:
                            Managers.Time.DayStart -= StartAtTime;
                            Managers.Time.NightStart -= EndAtTime;
                            break;
                        case TimeUnit.Night:
                            Managers.Time.NightStart -= StartAtTime;
                            Managers.Time.DayStart -= EndAtTime;
                            break;
                    }
                }
            }
            private void StartAtTime(int day)
            {
                OnActive();
            }
            private void EndAtTime(int day)
            {
                OnDeactive();
            }
        }
    }
}