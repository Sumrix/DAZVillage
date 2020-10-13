using UnityEngine;
using UnityEngine.UI;
using Collections;
using System;

namespace GameUI
{
    public class CraftWindow :
        PageView
    {
        public CategoriesPage CategoriesPage;
        public ItemsPage ItemsPage;
        public ItemPage ItemPage;

        private void Awake()
        {
            CategoriesPage.Click += CategoriesPage_Click;
            ItemsPage.Click += ItemsPage_Click;
        }
        private void CategoriesPage_Click(object sender, CollectionPage.CollectionPageEventArgs e)
        {
            OpenPage(ItemsPage);
            var itemType = (ItemType)CategoriesPage.StringsView.SelectedIndex;
            ItemsPage.ShowItemsOfType(itemType);
        }
        private void ItemsPage_Click(object sender, CollectionPage.CollectionPageEventArgs e)
        {
            OpenPage(ItemPage);
            ItemPage.Show(e.Object);
        }
    }
}