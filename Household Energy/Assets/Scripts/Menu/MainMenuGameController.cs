using UnityEngine;

public class MainMenuGameController : MonoBehaviour
{
    private AudioSource backgroundAudioSource;

    private void Awake()
    {
        LoadGameData();

        backgroundAudioSource = gameObject.GetComponent<AudioSource>();
        UpdateBackgroundMusicEnable();
        UpdateBackgroundMusicVolume();
    }

    private void LoadGameData()
    {
        if (SaveAndLoadManager.CheckGameDataExist())
        {
            GameData gameData = SaveAndLoadManager.LoadGameData();
            UpdateGameInfo(gameData);
        }
    }

    private void UpdateGameInfo(GameData gameData)
    {
        GameInfo.BackgroundMusicEnable = gameData.backgroundMusicEnable;
        GameInfo.SoundEffectsEnable = gameData.soundEffectEnable;
        GameInfo.BackgroundMusicVolume = gameData.backgroundMusicVolume;
        GameInfo.SoundEffectsVolume = gameData.soundEffectVolume;
    }

    internal void UpdateBackgroundMusicEnable()
    {
        if (GameInfo.BackgroundMusicEnable)
            backgroundAudioSource.Play();
        else
            backgroundAudioSource.Pause();
        backgroundAudioSource.volume = GameInfo.BackgroundMusicVolume;
    }

    internal void UpdateBackgroundMusicVolume()
    {
        backgroundAudioSource.volume = GameInfo.BackgroundMusicVolume;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
