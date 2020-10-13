using UnityEngine;
using System;

namespace GameUI
{
    public class TabView :
        MonoBehaviour
    {
        [RequiredField]
        public TabHeader TabHeaderPrefab;
        [RequiredField]
        public Transform HeaderList;
        [Serializable]
        public class TabControl
        {
            public string Title;
            [RequiredField]
            public GameObject Page;
        }
        public TabControl[] Pages;
        private GameObject CurrentPage;
        private TabHeader CurrentTabHeader;

        private void Start()
        {
            if (Pages.Length > 0)
            {
                foreach (var page in Pages)
                {
                    var tabHeader = (TabHeader)GameObject.Instantiate(TabHeaderPrefab, HeaderList, false);
                    tabHeader.transform.lossyScale.Set(1,1,1);
                    tabHeader.SetText(page.Title);
                    tabHeader.Click += TabHeader_Click;
                }

                CurrentTabHeader = HeaderList.GetComponentInChildren<TabHeader>();
                CurrentTabHeader.Activate();

                CurrentPage = Pages[0].Page;
                CurrentPage.SetActive(true);
            }
        }
        private void TabHeader_Click(object sender, EventArgs e)
        {
            CurrentTabHeader.Deactivate();
            CurrentTabHeader = (TabHeader)sender;
            CurrentTabHeader.Activate();

            CurrentPage.SetActive(false);
            CurrentPage = Pages[CurrentTabHeader.transform.GetSiblingIndex()].Page;
            CurrentPage.SetActive(true);
        }
    }
}