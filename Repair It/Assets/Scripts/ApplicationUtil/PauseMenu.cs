using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private GameController gameController;

    public Slider soundSlider;
    public Slider musicSlider;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerobject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerobject != null)
            gameController = gameControllerobject.GetComponent<GameController>();

        soundSlider.value = ApplicationUtil.GameSoundVolume;
        musicSlider.value = ApplicationUtil.GameMusicVolume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pause()
    {
        ApplicationUtil.GamePaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        ApplicationUtil.GamePaused = false;
    }

    public void OnClickHome()
    {
        ApplicationUtil.GamePaused = false;
        gameController.LoadMainMenu();
    }

    public void Restart()
    {
        Resume();
        ApplicationUtil.GamePaused = false;
        gameController.ReloadScene();
    }

    public void OnSoundChange()
    {
        ApplicationUtil.GameSoundVolume = soundSlider.value;
        gameController.VolumeUpdate();
    }

    public void OnMusicChange()
    {
        ApplicationUtil.GameMusicVolume = musicSlider.value;
        gameController.VolumeUpdate();
    }
}
