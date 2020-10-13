using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameUI
{
	[RequireComponent(typeof(Image))]
	public class ItemView :
		CollectionViewItem
	{
        [SerializeField]
        [RequiredField]
        private Text _countLabel;
        [SerializeField]
        [RequiredField]
        private Image _countBackground;
        [SerializeField]
        [RequiredField]
        private Image _front;
        [SerializeField]
        [RequiredField]
        private Material _inactiveBackground;
        [SerializeField]
        [RequiredField]
        private Material _inactiveFront;
        [SerializeField]
        [RequiredField]
        private Material _activeBackground;
        [SerializeField]
        [RequiredField]
        private Material _activeFront;
        private Image _background
        {
            get
            {
                return GetComponent<Image>();
            }
        }

        protected void Awake()
        {
            _activeBackground = _background.material;
            _activeFront = _front.material;
        }
		public override void Upd (object value)
        {
            var inspectorItem = value as InspectorItem;
            if (inspectorItem != null)
            {
                ShowItem(inspectorItem.Prefab, inspectorItem.Count);
            }
            else if (value != null)
            {
                var item = (Item)value;
                ShowItem(item, Mathf.RoundToInt(item.Stack.Current));
            }
            else
            {
                _front.sprite = null;
                _countBackground.gameObject.SetActive(false);
            }
        }

        private void ShowItem(Item item, int count)
        {
            _front.sprite = item.Sprite;
            if (item.IsStackable)
            {
                _countLabel.text = count.ToString();
                _countBackground.gameObject.SetActive(true);
            }
            else
            {
                _countBackground.gameObject.SetActive(false);
            }
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                _background.material = _activeBackground;
                _front.material = _activeFront;
            }
            else
            {
                _background.material = _inactiveBackground;
                _front.material = _inactiveFront;
            }
        }
	}
}