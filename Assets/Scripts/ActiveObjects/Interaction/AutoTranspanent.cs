using UnityEngine;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    public class AutoTranspanent :
        MonoBehaviour
    {
        [RequiredField]
        public FieldTrigger Field;
        public Renderer Model;
        public float Transparency;

        void Start()
        {
            Field.Active += Field_Active;
            Field.Deactive += Field_Deactive;

            Field.Enable();
        }
        private void Field_Active(object sender, System.EventArgs e)
        {
            var color = Model.material.color;
            color.a = Transparency;
            Model.material.color = color;
        }
        private void Field_Deactive(object sender, System.EventArgs e)
        {
            var color = Model.material.color;
            color.a = 1f;
            Model.material.color = color;
        }
    }
}