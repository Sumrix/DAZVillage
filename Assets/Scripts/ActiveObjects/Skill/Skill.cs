using UnityEngine;
using System;

namespace ActiveObjects
{
    namespace GameSkill
    {
        /// <summary>
        /// Скил
        /// </summary>
        public class Skill :
            MonoBehaviour
        {
            /// <summary>
            /// Активация
            /// </summary>
            public event EventHandler Active;
            /// <summary>
            /// Деактивация
            /// </summary>
            public event EventHandler Deactive;
            /// <summary>
            /// Активен ли скил
            /// </summary>
            public bool IsActive { get; private set; }
            /// <summary>
            /// Описание скила
            /// </summary>
            [Tooltip("Описание скила.")]
            public Description Description;
            /// <summary>
            /// Способ активации скила
            /// </summary>
            [Tooltip("Способ активации скила. Auto - автоматический, Manually - по нажатию кнопки.")]
            public SkillLaunching Launch = SkillLaunching.Auto;
            /// <summary>
            /// Владелец скила, тот на ком висит скил
            /// </summary>
            [HideInInspector]
            public Character Owner;
            /// <summary>
            /// Создатель скила, тот кто применил скил
            /// </summary>
            [HideInInspector]
            public Character Creator;
            /// <summary>
            /// Источник звуков скила
            /// </summary>
            [HideInInspector]
            public AudioSource AudioSource;

            protected virtual void Awake()
            {
                Owner = (Character)this.FindParentObjectOfType(typeof(Character));
                if (Owner == null)
                {
                    this.LogError("Can't find a component of type 'Character' among the parents");
                }
                else
                {
                    AudioSource = Owner.GetComponent<AudioSource>();
                }
            }
            private void Start()
            {
                if (Launch == SkillLaunching.Auto)
                {
                    Enable();
                }
            }
            /// <summary>
            /// Включить
            /// </summary>
            public virtual void Enable()
            {
                if (!IsActive)
                {
                    IsActive = true;
                    Activate();
                    OnActive();
                }
            }
            /// <summary>
            /// Выключить
            /// </summary>
            public virtual void Disable()
            {
                if (IsActive)
                {
                    IsActive = false;
                    Deactivate();
                    OnDeactive();
                }
            }
            /// <summary>
            /// Активация. Метод для переопределения.
            /// </summary>
            protected virtual void Activate()
            {
            }
            /// <summary>
            /// Деактивация. Метод для переопределения.
            /// </summary>
            protected virtual void Deactivate()
            {
            }
            private void OnActive()
            {
                var handler = Active;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            private void OnDeactive()
            {
                var handler = Deactive;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
        public enum SkillLaunching
        {
            Auto,
            Manually,
        }
    }
}