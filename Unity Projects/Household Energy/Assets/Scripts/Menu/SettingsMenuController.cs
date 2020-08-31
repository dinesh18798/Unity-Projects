using MaterialUI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    private MainMenuGameController mainMenuGameController;
    private Slider backgroundMusicVolumeSilder;
    private ToggleSwitch backgroundMusicSwitch;
    private Slider soundEffectsVolumeSilder;
    private ToggleSwitch soundEffectsSwitch;

    void Awake()
    {
        GameObject mainMenuGameControllerObject = GameObject.FindGameObjectWithTag("MainMenuGameController");
        if (mainMenuGameControllerObject != null)
            mainMenuGameController = mainMenuGameControllerObject.GetComponent<MainMenuGameController>();

        backgroundMusicVolumeSilder = transform.Find("BackgroundMusic").Find("BackgroundMusicSlider").GetComponent<Slider>();
        backgroundMusicSwitch = transform.Find("BackgroundMusic").Find("BackgroundMusicSwitch").GetComponent<ToggleSwitch>();
       
        if (backgroundMusicVolumeSilder != null)
        {
            backgroundMusicVolumeSilder.value = GameInfo.BackgroundMusicVolume * 100;
            backgroundMusicVolumeSilder.onValueChanged.AddListener(delegate { UpdateBackgroundMusicVolume(backgroundMusicVolumeSilder.value); });
            backgroundMusicVolumeSilder.onValueChanged.AddListener(delegate { backgroundMusicVolumeSilder.GetComponent<SliderConfig>().UpdateText(); });
        }

        if(backgroundMusicSwitch != null)
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

    private void IsSoundEffectsEnable(bool isON)
    {
        GameInfo.SoundEffectsEnable = isON;
    }

    private void UpdateSoundEffectsVolume(float value)
    {
        GameInfo.SoundEffectsVolume = value / 100;
    }

    private void IsBackgroundMusicEnable(bool isON)
    {
        GameInfo.BackgroundMusicEnable = isON;
        mainMenuGameController.UpdateBackgroundMusicEnable();
    }

    private void UpdateBackgroundMusicVolume(float value)
    {
        GameInfo.BackgroundMusicVolume = value / 100;
        mainMenuGameController.UpdateBackgroundMusicVolume();
    }
}
