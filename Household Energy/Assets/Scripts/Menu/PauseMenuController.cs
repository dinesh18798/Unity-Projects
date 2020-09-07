using MaterialUI;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    private AudioController audioController;

    private Slider backgroundMusicVolumeSilder;
    private ToggleSwitch backgroundMusicSwitch;
    private Slider soundEffectsVolumeSilder;
    private ToggleSwitch soundEffectsSwitch;
    private SceneChanger sceneChanger;

    void Awake()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
            audioController = gameControllerObject.GetComponent<AudioController>();

        backgroundMusicVolumeSilder = transform.Find("BackgroundMusic").Find("BackgroundMusicSlider").GetComponent<Slider>();
        backgroundMusicSwitch = transform.Find("BackgroundMusic").Find("BackgroundMusicSwitch").GetComponent<ToggleSwitch>();

        if (backgroundMusicVolumeSilder != null)
        {
            backgroundMusicVolumeSilder.value = GameInfo.BackgroundMusicVolume * 100;
            backgroundMusicVolumeSilder.onValueChanged.AddListener(delegate { UpdateBackgroundMusicVolume(backgroundMusicVolumeSilder.value); });
            backgroundMusicVolumeSilder.onValueChanged.AddListener(delegate { backgroundMusicVolumeSilder.GetComponent<SliderConfig>().UpdateText(); });
        }

        if (backgroundMusicSwitch != null)
        {
            backgroundMusicSwitch.IsON = GameInfo.BackgroundMusicEnable;
            backgroundMusicSwitch.onValueChanged.AddListener(delegate { IsBackgroundMusicEnable(backgroundMusicSwitch.IsON); });
        }

        soundEffectsVolumeSilder = transform.Find("SoundEffects").Find("SoundEffectsSlider").GetComponent<Slider>();
        soundEffectsSwitch = transform.Find("SoundEffects").Find("SoundEffectsSwitch").GetComponent<ToggleSwitch>();

        if (soundEffectsVolumeSilder != null)
        {
            soundEffectsVolumeSilder.value = GameInfo.SoundEffectsVolume * 100;
            soundEffectsVolumeSilder.onValueChanged.AddListener(delegate { UpdateSoundEffectsVolume(soundEffectsVolumeSilder.value); });
            soundEffectsVolumeSilder.onValueChanged.AddListener(delegate { soundEffectsVolumeSilder.GetComponent<SliderConfig>().UpdateText(); });
        }

        if (soundEffectsSwitch != null)
        {
            soundEffectsSwitch.IsON = GameInfo.SoundEffectsEnable;
            soundEffectsSwitch.onValueChanged.AddListener(delegate { IsSoundEffectsEnable(soundEffectsSwitch.IsON); });
        }
    }
    private void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }
    }

    public void Pause()
    {
        GameInfo.GamePaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameInfo.GamePaused = false;
    }

    private void IsSoundEffectsEnable(bool isON)
    {
        GameInfo.SoundEffectsEnable = isON;
    }

    private void UpdateSoundEffectsVolume(float value)
    {
        GameInfo.SoundEffectsVolume = value / 100;
        audioController.UpdateSoundEffectsVolume();
    }

    private void IsBackgroundMusicEnable(bool isON)
    {
        GameInfo.BackgroundMusicEnable = isON;
        audioController.UpdateBackgroundMusicEnable();
    }

    private void UpdateBackgroundMusicVolume(float value)
    {
        GameInfo.BackgroundMusicVolume = value / 100;
        audioController.UpdateBackgroundMusicVolume();
    }

    public void ReturnMainMenu()
    {
        sceneChanger.FadeToMainMenu();
    }
}
