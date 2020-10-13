using UnityEngine;

namespace GameUI
{
    public class PauseMenu :
        MonoBehaviour
    {
        private static int _activeMenuCount = 0;

        private void OnEnable()
        {
            _activeMenuCount++;
            if (_activeMenuCount == 1)
            {
                Time.timeScale = 0;
            }
        }
        private void OnDisable()
        {
            _activeMenuCount--;
            if (_activeMenuCount == 0)
            {
                Time.timeScale = 1;
            }
        }
    }
}