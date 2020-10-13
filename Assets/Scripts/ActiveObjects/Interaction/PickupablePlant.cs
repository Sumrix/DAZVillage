using UnityEngine;
using System;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    [RequireComponent(typeof(FieldTrigger))]
    public class PickupablePlant :
        MonoBehaviour
    {
        [RequiredField]
        public DropItem SpawnItem;
        [RequiredField]
        public Transform PlantModel;
        public TimeUnitTrigger DayTime;
        public FieldTrigger Field;
        public ButtonTrigger Button;

        private void Start()
        {
            DayTime.Active += DayTime_Active;
            Field.Active += Field_Active;
            Field.Deactive += Field_Deactive;
            Button.Active += Button_Active;
            
            Field.Enable();
            DayTime.Enable();
        }
        private void DayTime_Active(object sender, EventArgs e)
        {
            if (!PlantModel.gameObject.activeSelf)
            {
                PlantModel.gameObject.SetActive(true);
                Field.Enable();
            }
        }
        private void Field_Active(object sender, EventArgs e)
        {
            Button.Enable();
        }
        private void Field_Deactive(object sender, EventArgs e)
        {
            Button.Disable();
        }
        private void Button_Active(object sender, EventArgs e)
        {
            Field.Disable();
            PlantModel.gameObject.SetActive(false);
            SpawnItem.Instantiate(transform, false);
        }
    }
}