using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager :
    MonoBehaviour,
    IGameManager,
    IManagerWaiter
{
    public ManagerStatus Status { get; private set; }
    [SerializeField]
    [RequiredField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private string[] _gameOverMessages;
    [SerializeField]
    private int _gameOverMessageNumber;
    public string GameOverMessage
    {
        get { return _gameOverMessages[_gameOverMessageNumber]; }
    }
    public bool AllowCraftingBaseResources;

    public void Startup()
    {
        LoadSettings();
        Managers.AddWaiter(this);
        Status = ManagerStatus.Started;
        Debug.Log("Game manager is started.");
    }
    public void LoadSettings()
    {
        _gameOverMessageNumber = PlayerPrefs.GetInt("GameOverMessageNumber") % _gameOverMessages.Length;
        Managers.Graphic.FPS30 = PlayerPrefs.GetInt("FPS30", 0) == 1 ? true : false;
        Managers.Player.AutoAiming = PlayerPrefs.GetInt("AutoAiming", 1) == 1 ? true : false;
        Managers.Audio.EffectsMute = PlayerPrefs.GetInt("EffectsMute", 0) == 1 ? true : false;
        Managers.Audio.MusicMute = PlayerPrefs.GetInt("MusicMute", 0) == 1 ? true : false;
        Managers.Audio.EffectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1);
        Managers.Audio.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("GameOverMessageNumber", _gameOverMessageNumber);
        PlayerPrefs.SetInt("FPS30", Managers.Graphic.FPS30 ? 1 : 0);
        PlayerPrefs.SetInt("AutoAiming", Managers.Player.AutoAiming ? 1 : 0);
        PlayerPrefs.SetInt("EffectsMute", Managers.Audio.EffectsMute ? 1 : 0);
        PlayerPrefs.SetInt("MusicMute", Managers.Audio.MusicMute ? 1 : 0);
        PlayerPrefs.SetFloat("EffectsVolume", Managers.Audio.EffectsVolume);
        PlayerPrefs.SetFloat("MusicVolume", Managers.Audio.MusicVolume);
    }
    void IManagerWaiter.Startup()
    {
        Managers.Player.Player.Dead += Player_Dead;
    }
    private void Player_Dead(object sender, DeadEventArgs e)
    {
        _gameOverPanel.SetActive(true);
        _gameOverMessageNumber = (_gameOverMessageNumber + 1) % _gameOverMessages.Length;
        SaveSettings();
    }
    public void RestartGame()
    {
        Managers.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    private void OnDestroy()
    {
        //SaveSettings();
    }
}
