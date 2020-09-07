using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;

    private float timeCount = 15.0f;
    private GameController gameController;
    private ZombieGameController zombieGameController;
    private bool displayed = false;

    void Start()
    {
        GameObject gameControllerobject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerobject != null)
            gameController = gameControllerobject.GetComponent<GameController>();

        GameObject zombieControllerobject = GameObject.FindGameObjectWithTag("ZombieGameController");
        if (zombieControllerobject != null)
            zombieGameController = zombieControllerobject.GetComponent<ZombieGameController>();
    }

    public void DisplayGameOver()
    {
        gameOverText.text = "Game Over!!!";
        displayed = true;
        gameOverPanel.SetActive(true);
    }

    public void DisplayYouWin()
    {
        gameOverText.text = "You Win!!!";
        displayed = true;
        gameOverPanel.SetActive(true);
    }

    void Update()
    {
        if (displayed)
        {
            timeCount -= Time.deltaTime;
            countdownText.text = string.Format("Return to Main Menu in {0} seconds", (timeCount).ToString("0"));
            if (timeCount < 0)
            {
                if (ApplicationUtil.GameLevel == GameLevels.Zombie)
                    zombieGameController.LoadMainMenu();
                else
                    gameController.LoadMainMenu();
            }
        }
        return;
    }
}
