using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class GameOverWindow :
        MonoBehaviour
    {
        [SerializeField]
        [RequiredField]
        public Text Text;

        private void OnEnable()
        {
            Text.text = Managers.Game.GameOverMessage;
        }
    }
}