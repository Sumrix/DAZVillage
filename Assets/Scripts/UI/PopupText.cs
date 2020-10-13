using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace GameUI
{
    public class PopupText :
        AssociatedElement
    {
        [SerializeField]
        private Text _text;
        [SerializeField]
        private Animator _animator;

        public static void CreateInstance(GameObject sceneObject, string name, int count)
        {
            var instance = (PopupText)GameObject.Instantiate(Managers.UI.PopupTextPrefab, Managers.UI.AssociatedElements, false);
            instance._sceneObject = sceneObject;
            instance._text.text = name + " x" + count;
        }
        private void Start()
        {
            AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipInfo[0].clip.length);
        }
    }
}