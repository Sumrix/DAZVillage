using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class HealthBar :
        AssociatedElement
    {
        [SerializeField]
        [RequiredField]
        private Image _healthLine;
        [SerializeField]
        private Color[] _teamColors;

        private Character _character;

        public static HealthBar CreateInstance(HealthBar prefab, Character character, int team)
        {
            HealthBar instance = (HealthBar)Instantiate(prefab, Managers.UI.AssociatedElements, false);
            instance._character = character;
            instance._sceneObject = character.gameObject;
            instance._character.Stats.Changed += instance.Stats_Changed;
            instance.SetHealthVisual(instance._character.Stats.Result.Health.CurrentNormalized);
            instance.SetTeam(team);
            instance.gameObject.SetActive(false);
            return instance;
        }
        private void Stats_Changed(object sender, System.EventArgs e)
        {
            SetHealthVisual(_character.Stats.Result.Health.CurrentNormalized);
        }
        public void SetTeam(int team)
        {
            _healthLine.color = _teamColors[team];
        }
        public void SetHealthVisual(float healthNormalized)
        {
            if (_healthLine)
            {
                _healthLine.transform.localScale = new Vector3(healthNormalized,
                                                             _healthLine.transform.localScale.y,
                                                             _healthLine.transform.localScale.z);

                if (gameObject.activeSelf && _character)
                {
                    if (Mathf.Approximately(healthNormalized, 1))
                    {
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (!Mathf.Approximately(healthNormalized, 1))
                    {
                        gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}