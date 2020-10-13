using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUI
{
    public class TabHeader :
        Clickable
    {
        [RequiredField]
        public Image Underline;
        [RequiredField]
        public Material ActiveMaterial;
        [RequiredField]
        public Material InactiveMaterial;
        [RequiredField]
        public Text Text;
        
        public void Activate()
        {
            Underline.material = ActiveMaterial;
        }
        public void Deactivate()
        {
            Underline.material = InactiveMaterial;
        }
        public void SetText(string text)
        {
            Text.text = text;
        }
    }
}