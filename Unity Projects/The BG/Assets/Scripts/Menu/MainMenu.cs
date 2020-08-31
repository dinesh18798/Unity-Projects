using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle musicOnToggle;
    public Toggle musicOffToggle;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        volumeSlider.value = ApplicationUtil.GameVolume;
        musicOnToggle.isOn = ApplicationUtil.GameMusic;
    }

    public void QuitGame()
    {
        if(ApplicationUtil.CurrentSceneIndex !=0) SaveLoadSystem.SaveGame();
        Application.Quit();
    }

    public void OnMusicOnUpdate()
    {
        ApplicationUtil.GameMusic = musicOnToggle.isOn;
        musicOffToggle.isOn = !musicOnToggle.isOn;
    }

    public void OnMusicOffUpdate()
    {
        musicOnToggle.isOn = !musicOffToggle.isOn;
    }

    public void OnVolumeChange()
    {
        ApplicationUtil.GameVolume = volumeSlider.value;
        volumeSlider.value = ApplicationUtil.GameVolume;
    }
}
