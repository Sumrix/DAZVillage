using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections.Generic;

namespace GameUI
{
    public class CharacterStatView :
        CollectionViewItem
    {
        [SerializeField]
        [RequiredField]
        private Text _mainField;
        [SerializeField]
        [RequiredField]
        private Text _bonusField;
        [SerializeField]
        [RequiredField]
        public Image _image;
        [SerializeField]
        [RequiredField]
        public Material _positiveMaterial;
        [SerializeField]
        [RequiredField]
        public Material _negativeMaterial;
        private string _lastStat;

        public override void Upd (object value)
        {
            var stat = (CharacterStat)value;

            _image.sprite = Managers.Sprites.GetCharacterStatSprite(stat.Name);
            _mainField.text = Mathf.Round(stat.Main).ToString();
            if (Mathf.Approximately(stat.Bonus, 0))
            {
                _bonusField.text = " ";
            }
            else if (stat.Bonus > 0)
            {
                _bonusField.text = '+' + stat.Bonus.ToString();
                _bonusField.material = _positiveMaterial;
            }
            else
            {
                _bonusField.text = stat.Bonus.ToString();
                _bonusField.material = _negativeMaterial;
            }
        }
    }
}