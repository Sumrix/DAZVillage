using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    [RequireComponent(typeof(Image))]
    public class ChangeActiveButton :
        Clickable
    {
        [RequiredField]
        public GameObject Object;
        [RequiredField]
        public Material ActiveMaterial;
        [RequiredField]
        public Material InactiveMaterial;
        private Image _buttonImage;

        private void Start()
        {
            _buttonImage = GetComponent<Image>();
            Click += ChangeActiveButton_Click;
        }
        private void ChangeActiveButton_Click(object sender, System.EventArgs e)
        {
            Object.SetActive(!Object.activeSelf);
            _buttonImage.material = Object.activeSelf ? ActiveMaterial : InactiveMaterial;
        }
    }
}