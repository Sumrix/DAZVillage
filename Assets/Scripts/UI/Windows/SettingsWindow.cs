using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace GameUI
{
    public class SettingsWindow :
        MonoBehaviour,
        IManagerWaiter
    {
        public Toggle AutoAiming;
        public Toggle FPS30;
        public Slider MusicVolume;
        public Toggle MusicMute;
        public Slider EffectsVolume;
        public Toggle EffectsMute;
        
        private void Start()
        {
            Managers.AddWaiter(this);
        }
        void IManagerWaiter.Startup()
        {
            AutoAiming.isOn = Managers.Player.AutoAiming;
            FPS30.isOn = Managers.Graphic.FPS30;
            MusicVolume.value = Managers.Audio.MusicVolume;
            MusicMute.isOn = Managers.Audio.MusicMute;
            EffectsVolume.value = Managers.Audio.EffectsVolume;
            EffectsMute.isOn = Managers.Audio.EffectsMute;
        }
        private void OnDisable()
        {
            Managers.Game.SaveSettings();
        }
    }
}