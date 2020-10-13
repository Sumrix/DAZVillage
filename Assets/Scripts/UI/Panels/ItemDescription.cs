using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ItemDescription :
        DescriptionPanel
    {
        [SerializeField]
        [RequiredField]
        protected Text _details;
        [SerializeField]
        [RequiredField]
        protected CollectionViewItem _icon;

        public override void SetItem(object obj)
        {
            Item item;
            var inspectorItem = obj as InspectorItem;
            item = inspectorItem != null ? inspectorItem.Prefab : (Item)obj;
            
            _icon.Upd(obj);
            _details.text = item.Description.Common;

        }
        public override void SetDefault()
        {
            _details.text = "";
            _icon.Upd(null);
        }
    }
}