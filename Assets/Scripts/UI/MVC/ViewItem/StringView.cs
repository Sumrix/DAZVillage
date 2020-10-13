using UnityEngine.UI;

namespace GameUI
{
    public class StringView :
            CollectionViewItem
    {
        public Text Text;
        
        public override void Upd(object value)
        {
            Text.text = value.ToString();
        }
    }

    public class StringObjectPair
    {
        public string String;
        public object Object;
    }
}