using UnityEngine;
using System;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    public class Mine :
        MonoBehaviour
    {
        public DropItem[] Items;
        [RequiredField]
        public Transform SpawnTarget;
        [RequiredField]
        public FieldTrigger Field;
        public ButtonTrigger Button;
        public TimerTrigger DelayTimer;

        private void Start()
        {
            Field.Active += Field_Active;
            Field.Deactive += Field_Deactive;
            Button.Active += Button_Active;
            DelayTimer.Active += DelayTimer_Active;
            DelayTimer.IsActiveStart = false;

            Field.Enable();
        }
        private void Field_Active(object sender, EventArgs e)
        {
            Button.Enable();
        }
        private void Field_Deactive(object sender, EventArgs e)
        {
            Button.Disable();
            DelayTimer.Disable();
        }
        private void Button_Active(object sender, EventArgs e)
        {
            Button.Disable();
            DelayTimer.Enable();
        }
        private void DelayTimer_Active(object sender, EventArgs e)
        {
            DelayTimer.Disable();
            Button.Enable();
            foreach (var item in Items)
            {
                if (UnityEngine.Random.value < item.Probability)
                {
                    item.Instantiate(SpawnTarget.transform, false);
                }
            }
        }
    }
}