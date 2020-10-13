using UnityEngine;
using System;

namespace ActiveObjects
{
    namespace Triggers
    {
        /// <summary>
        /// Клон класса TimeTrigger для прикрепления к GameObject
        /// </summary>
        public class TimeTriggerComponent :
            MonoBehaviour
        {
            /// <summary>
            /// Активация триггера
            /// </summary>
            public event EventHandler Active;
            /// <summary>
            /// Деактивация триггера
            /// </summary>
            public event EventHandler Deactive;
            /// <summary>
            /// Активен ли триггер
            /// </summary>
            public bool IsActive { get; private set; }
            /// <summary>
            /// Включён ли триггер
            /// </summary>
            public bool IsEnable { get; protected set; }

            public TimeTriggerComponent()
            {
                IsActive = false;
                IsEnable = false;
            }
            public virtual void Enable()
            {
                if (!IsEnable)
                {
                    IsEnable = true;
                }
            }
            public virtual void Disable()
            {
                if (IsEnable)
                {
                    IsEnable = false;
                }
            }
            protected virtual void OnActive()
            {
                if (!IsActive && IsEnable)
                {
                    IsActive = true;

                    var handler = Active;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
            protected virtual void OnDeactive()
            {
                if (IsActive)
                {
                    IsActive = false;

                    var handler = Deactive;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}