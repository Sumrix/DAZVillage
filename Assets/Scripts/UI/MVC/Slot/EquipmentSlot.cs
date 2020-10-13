using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameUI
{
	[RequireComponent(typeof(Image))]
	public class EquipmentSlot :
		Slot
	{
        [SerializeField]
        private EquipmentType _equipmentType;
        [SerializeField]
        [RequiredField]
        public Image _image;

        public override void Upd(object value)
        {
            _equipmentType = (EquipmentType)value;
            _image.sprite = Managers.Sprites.GetEquipmentSprite(_equipmentType);
        }
    }
}