using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Slider volumeSlider;
    public Toggle musicOnToggle;
    public Toggle musicOffToggle;

    private GameController gameController;
    private ZombieGameController zombieGameController;

    void Start()
    {
        GameObject gameControllerobject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerobject != null)
            gameController = gameControllerobject.GetComponent<GameController>();

        GameObject zombieControllerobject = GameObject.FindGameObjectWithTag("ZombieGameController");
        if (zombieControllerobject != null)
            zombieGameController = zombieControllerobject.GetComponent<ZombieGameController>();

        volumeSlider.value = ApplicationUtil.GameVolume;
        musicOnToggle.isOn = ApplicationUtil.GameMusic;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if ((gameController != null && gameController.gameOverFlag) ||
                (zombieGameController != null && zombieGameController.gameOverFlag)) return;

            if (!ApplicationUtil.GamePaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Pause();
            }
            return;
        }
    }

    private void Pause()
    {
        Cursor.visible = true;
        ApplicationUtil.GamePaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        ApplicationUtil.GamePaused = false;
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

    public void OnClickBackMenu()
    {
        ApplicationUtil.GamePaused = false;
        if (ApplicationUtil.GameLevel == GameLevels.Zombie)
            zombieGameController.LoadMainMenu();
        else
            gameController.LoadMainMenu();
    }
}
