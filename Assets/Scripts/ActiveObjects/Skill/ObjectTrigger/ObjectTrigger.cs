using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ActiveObjects
{
    namespace Triggers
    {
        /// <summary>
        /// Триггер реагирующий на объекты
        /// </summary>
        public class ObjectTrigger :
            TimeTriggerComponent
        {
            /// <summary>
            /// Триггер отреагировал на новый объект
            /// </summary>
            public event EventHandler<SelectEventArgs> Select;
            /// <summary>
            /// Объект больше не соответствует условиям триггера
            /// </summary>
            public event EventHandler<SelectEventArgs> Deselect;
            /// <summary>
            /// Список объектов соответствующих условиям триггера
            /// </summary>
            //[HideInInspector]
            public List<Component> SelectList = new List<Component>();
            /// <summary>
            /// Фильтр по типу компонента
            /// </summary>
            [SerializeField]
            protected string _targetTypeName = "Character";
            private Type _validType;

            protected virtual void Awake()
            {
                _validType = Type.GetType(_targetTypeName);
                if (_validType == null)
                {
                    this.LogError("The type '" + _targetTypeName + "' does not exist");
                }
            }
            public override void Enable()
            {
                if (!IsEnable)
                {
                    IsEnable = true;

                    if (SelectList.Count > 0)
                    {
                        OnSelect(SelectList);
                    }
                }
            }
            public override void Disable()
            {
                if (IsEnable)
                {
                    if (SelectList.Count > 0)
                    {
                        OnDeselect(SelectList);
                    }
                    IsEnable = false;
                }
            }
            public void OnSelect(List<Component> targetList)
            {
                // Удаляем null; Да пущай пока копится, потом подумаем.
                //_seletcList = _seletcList.OfType<Component>().ToList();

                List<Component> addList = new List<Component>();
                foreach (var target in targetList)
                {
                    Component addTarget = target;
                    // Удаляем не проходящие фильтр
                    if ((addTarget.GetType() == _validType ||
                        (addTarget = target.GetComponent(_validType)) != null)
                        // Удаляем повторы
                        && !SelectList.Any(x => x == addTarget))
                    {
                        addList.Add(addTarget);
                    }
                }
                if (addList.Count > 0)
                {
                    // Потом обновляем данные, чтобы не было повторных обновлений
                    SelectList.AddRange(addList);
                    // Сперва включаем триггеры
                    if (IsEnable)
                    {
                        if (!IsActive)
                        {
                            OnActive();
                        }
                    }

                    if (IsEnable)
                    {
                        var handler = Select;
                        if (handler != null)
                        {
                            handler(this, new SelectEventArgs { TargetList = addList });
                        }
                    }
                }
            }
            public void OnDeselect(List<Component> targetList)
            {
                List<Component> removeList = new List<Component>();
                foreach (var target in targetList)
                {
                    Component removeTarget = target;
                    if ((removeTarget.GetType() == _validType ||
                        (removeTarget = target.GetComponent(_validType)) != null)
                        && SelectList.Any(x => x == removeTarget))
                    {
                        removeList.Add(removeTarget);
                    }
                }
                // Удаляем совпадения
                SelectList = SelectList.Except(removeList).ToList();
                // Если удаляемый список не пуст, то вызываем событие
                if (IsEnable && removeList.Count > 0)
                {
                    var handler = Deselect;
                    if (handler != null)
                    {
                        handler(this, new SelectEventArgs { TargetList = targetList });
                    }

                    if (SelectList.Count == 0)
                    {
                        OnDeactive();
                    }
                }
            }
            public void OnSelect(Component component)
            {
                OnSelect(new List<Component> { component });
            }
            public void OnDeselect(Component component)
            {
                OnDeselect(new List<Component> { component });
            }
        }
        public class SelectEventArgs :
            EventArgs
        {
            public List<Component> TargetList;
        }
    }
}