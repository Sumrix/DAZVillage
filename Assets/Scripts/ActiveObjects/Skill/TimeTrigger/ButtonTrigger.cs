using UnityEngine;
using System;

namespace ActiveObjects
{
    namespace Triggers
    {
        // Кнопка. Включается/выключается по нажатию.
        // Если текст включения/выключения отсутствует,
        // то соответствующее действие будет выполняться автоматический.
        [Serializable]
        public class ButtonTrigger :
            TimeTrigger
        {
            public string Message = "Button";
            [HideInInspector]
            public bool DisableByClick = true;

            public override void Enable()
            {
                if (!IsEnable)
                {
                    IsEnable = true;

                    Managers.UI.Confirm(Message, ActivateConfirmed);
                }
            }
            public override void Disable()
            {
                if (IsEnable)
                {
                    IsEnable = false;

                    Managers.UI.CancelConfirming(ActivateConfirmed);
                    OnDeactive();
                }
            }
            private void ActivateConfirmed()
            {
                if (IsEnable && !IsActive)
                {
                    OnActive();

                    if (DisableByClick)
                    {
                        Disable();
                    }
                    else
                    {
                        OnDeactive();
                        Managers.UI.Confirm(Message, ActivateConfirmed);
                    }
                }
            }
        }
    }
}