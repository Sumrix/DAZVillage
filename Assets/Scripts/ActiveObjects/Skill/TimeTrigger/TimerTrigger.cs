using UnityEngine;
using System.Collections;
using System;

namespace ActiveObjects
{
    namespace Triggers
    {
        /// <summary>
        /// Таймер
        /// </summary>
        [Serializable]
        public class TimerTrigger :
            TimeTrigger
        {
            /// <summary>
            /// Колдаун в секундах
            /// </summary>
            public float Period = 0;
            /// <summary>
            /// Запускается ли таймер периодично
            /// </summary>
            public bool IsRepetitive = true;
            /// <summary>
            /// Нужно ли активировать триггер в начале если колдаун уже прошёл
            /// </summary>
            public bool IsActiveStart = true;
            /// <summary>
            /// Последнее время применения скила
            /// </summary>
            protected float _lastApplyedTime;
            /// <summary>
            /// Оставшееся до конца колдауна время
            /// </summary>
            public float LastTime
            {
                get { return Mathf.Max(Period - Time.time + _lastApplyedTime, 0); }
            }
            public float Progress
            {
                get { return Period > 0 ? (Period - LastTime) / Period : 1; }
            }
            /// <summary>
            /// Ссылка на исполняемый скрипт
            /// </summary>
            private Coroutine coroutine;

            protected void Start()
            {
                _lastApplyedTime = -Period;
            }
            public override void Enable()
            {
                if (!IsEnable)
                {
                    IsEnable = true;

                    coroutine = DelayHelper.StartHelperCoroutine(InvokeRepeating());
                }
            }
            IEnumerator InvokeRepeating()
            {
                float delay = IsActiveStart ? LastTime : LastTime + Period;
                yield return new WaitForSeconds(delay);
                if (IsEnable)
                {
                    do
                    {
                        _lastApplyedTime = Time.time;
                        OnActive();
                        OnDeactive();
                        yield return new WaitForSeconds(Period);
                    } while (IsEnable && IsRepetitive);
                }
                else
                {
                    // Если TimerTrigger уже удалили, а вызов метода ещё идёт.
                    DelayHelper.StopHelperCoroutine(coroutine);
                }
            }
            public override void Disable()
            {
                if (IsEnable)
                {
                    IsEnable = false;
                    DelayHelper.StopHelperCoroutine(coroutine);
                    coroutine = null;
                }
            }
            ~TimerTrigger()
            {
                // Остановить явно DelayHelper отсюда нельзя
                // Невозможно понять был ли запущен DelayHelper или нет
                IsEnable = false;
            }
        }
    }
}