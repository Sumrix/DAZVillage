using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace GameUI
{
    public class PageView :
        MonoBehaviour
    {
        [RequiredField]
        public Text Title;
        public Clickable BackButton;
        public Text BackButtonText;
        [RequiredField]
        public Transform Content;
        private Stack<Page> _history = new Stack<Page>();
        [Space(10)]
        [RequiredField]
        public Page CurrentPage;

        private void Start()
        {
            if (BackButton != null)
            {
                BackButton.gameObject.SetActive(false);
                BackButton.Click += BackButton_Click;
            }
            SetTitle(CurrentPage.Title);
            CurrentPage.TitleChanged += Page_TitleChanged;
            CurrentPage.gameObject.SetActive(true);
        }
        protected virtual void OnEnable()
        {
            CurrentPage.gameObject.SetActive(true);
            CurrentPage.transform.SetParent(Content.transform);
        }
        protected virtual void OnDisable()
        {
            CurrentPage.gameObject.SetActive(false);
        }
        public void OpenPage(Page newPage)
        {
            if (CurrentPage == null)
            {
                throw new ArgumentNullException("newPage");
            }

            _history.Push(CurrentPage);

            if (BackButton != null && BackButtonText != null)
            {
                if (_history.Count == 1)
                {
                    BackButton.gameObject.SetActive(true);
                }
                BackButtonText.text = CurrentPage.Title;
            }

            ShowPage(newPage);
        }
        public void Back()
        {
            ShowPage(_history.Peek());

            CurrentPage = _history.Pop();

            if (BackButton != null && BackButtonText != null)
            {
                if (_history.Count == 0)
                {
                    BackButton.gameObject.SetActive(false);
                }
                else
                {
                    BackButtonText.text = _history.Peek().Title;
                }
            }
        }
        private void ShowPage(Page newPage)
        {
            if (newPage == null)
            {
                throw new ArgumentNullException("newPage");
            }

            CurrentPage.gameObject.SetActive(false);
            CurrentPage.TitleChanged -= Page_TitleChanged;
            CurrentPage = newPage;
            CurrentPage.TitleChanged += Page_TitleChanged;
            CurrentPage.transform.SetParent(Content.transform);
            SetTitle(CurrentPage.Title);
            CurrentPage.gameObject.SetActive(true);
        }
        private void SetTitle(string text)
        {
            if (text.Length == 0)
            {
                Title.gameObject.SetActive(false);
            }
            else
            {
                Title.gameObject.SetActive(true);
                Title.text = text;
            }
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            Back();
        }
        private void Page_TitleChanged(object sender, Page.TitleChangedEventArgs e)
        {
            SetTitle(e.Title);
        }
    }
}