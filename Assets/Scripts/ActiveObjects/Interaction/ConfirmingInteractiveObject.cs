using UnityEngine;
using UnityEngine.UI;
using ActiveObjects.Triggers;

namespace ActiveObjects
{
    public class ConfirmingInteractiveObject :
        MonoBehaviour
    {
        [SerializeField]
        private Image _window;
        public ButtonTrigger Button;
        public FieldTrigger Field;

        private void Start()
        {
            Button.DisableByClick = false;

            Button.Active += Button_Active;
            Field.Active += Field_Active;
            Field.Deactive += Field_Deactive;

            Field.Enable();
        }

        private void Button_Active(object sender, System.EventArgs e)
        {
            _window.gameObject.SetActive(true);
        }
        private void Field_Active(object sender, System.EventArgs e)
        {
            Button.Enable();
        }
        private void Field_Deactive(object sender, System.EventArgs e)
        {
            Button.Disable();
        }
    }
}