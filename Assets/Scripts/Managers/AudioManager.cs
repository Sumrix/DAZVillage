using UnityEngine;
using System.Collections;
using ActiveObjects.Triggers;

public class AudioManager :
    MonoBehaviour,
    IGameManager
{
    private TimeUnitTrigger _nightTimer;
    public AudioSource AudioSource;
    public AudioClip[] NightMusics;
    public AudioClip[] DayMusics;
    public int VolumeChangeSpeed = 1;
    public ManagerStatus Status { get; private set; }
    public float EffectsVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }
    public bool EffectsMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }
    private float _musicVolume;
    public float MusicVolume
    {
        get { return _musicVolume; }
        set
        {
            AudioSource.volume = value;
            _musicVolume = value;
        }
    }
    public bool MusicMute
    {
        get { return AudioSource.mute; }
        set { AudioSource.mute = value; }
    }

    public void Startup ()
    {
        _nightTimer = new TimeUnitTrigger
        {
            TimeUnit = TimeUnit.Night
        };
        _nightTimer.Active += NightTimer_Active;
        _nightTimer.Deactive += NightTimer_Deactive;
        _nightTimer.Enable();
        AudioSource.ignoreListenerVolume = true;
        AudioSource.ignoreListenerPause = true;

        Status = ManagerStatus.Started;
        Debug.Log("Audio manager is started.");
    }
    private void NightTimer_Active(object sender, System.EventArgs e)
    {
        AudioSource.Stop();
        if (NightMusics.Length > 0)
        {
            AudioSource.PlayOneShot(NightMusics[Random.Range(0, NightMusics.Length)]);
            StartCoroutine(SmoothlyChangeVolume());
        }
    }
    private void NightTimer_Deactive(object sender, System.EventArgs e)
    {
        AudioSource.Stop();
        if (DayMusics.Length > 0)
        {
            AudioSource.PlayOneShot(DayMusics[Random.Range(0, DayMusics.Length)]);
            StartCoroutine(SmoothlyChangeVolume());
        }
    }
    private IEnumerator SmoothlyChangeVolume()
    {
        float step = VolumeChangeSpeed / 1000.0f;
        step = step > 0 ? step : 0.1f;
        AudioSource.volume = 0;
        while (AudioSource.volume < MusicVolume)
        {
            yield return new WaitForSeconds(0.03f);
            AudioSource.volume += step;
        }
    }
}
