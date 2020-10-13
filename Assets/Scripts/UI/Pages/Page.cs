using UnityEngine;
using System;

namespace GameUI
{
    public class Page :
        MonoBehaviour
    {
        public class TitleChangedEventArgs :
            EventArgs
        {
            public string Title;
        }
        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        [SerializeField]
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnTitleChanged(new TitleChangedEventArgs { Title = value });
            }
        }
        
        private void OnTitleChanged(TitleChangedEventArgs e)
        {
            var handler = TitleChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}