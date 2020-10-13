using UnityEngine;

namespace GameUI
{
    public class ItemPage :
        Page
    {
        [SerializeField]
        public CollectionPage _itemsPage;
        [SerializeField]
        [RequiredField]
        public DescriptionPanel _description;
        [HideInInspector]
        public Item CurrentItem;
        [HideInInspector]
        public int CurrentItemCount;
        
        public void Show(object obj)
        {
            var inspectorItem = obj as InspectorItem;
            if (inspectorItem != null)
            {
                Title = inspectorItem.Prefab.Description.Name;
                ShowItem(inspectorItem.Prefab, inspectorItem.Count);
            }
            else
            {
                var item = (Item)obj;
                if (item != null)
                {
                    Title = item.Description.Name;
                    ShowItem(item, Mathf.RoundToInt(item.Stack.Current));
                }
                else
                {
                    ShowItem(null, 0);
                }
            }
        }
        protected virtual void ShowItem(Item item, int count)
        {
            CurrentItem = item;
            CurrentItemCount = count;
            if (item == null)
            {
                _description.Set(null);
            }
            else
            {
                _description.Set(new InspectorItem { Prefab = item, Count = count });
            }
        }
        private void OnEnable()
        {
            if (_itemsPage != null)
            {
                Show(_itemsPage.SelectedItem);
                _itemsPage.Selected += ItemsPanel_Selected;
            }
        }
        private void OnDisable()
        {
            if (_itemsPage != null)
            {
                _itemsPage.Selected -= ItemsPanel_Selected;
            }
        }
        private void ItemsPanel_Selected(object sender, CollectionPage.CollectionPageEventArgs e)
        {
            Show(e.Object);
        }
    }
}