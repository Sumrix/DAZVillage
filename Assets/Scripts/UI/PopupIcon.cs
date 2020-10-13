using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class PopupIcon :
        MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Text _text;
        [SerializeField]
        private Animator _animator;

        public static void CreateInstance(GameObject sceneObject, Sprite sprite, string text)
        {
            var instance = (PopupIcon)GameObject.Instantiate(Managers.UI.PopupIconPrefab, Managers.UI.AssociatedElements, false);
            instance._image.sprite = sprite;
            instance._text.text = text;
        }
        private void Start()
        {
            AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipInfo[0].clip.length);
        }
    }
}