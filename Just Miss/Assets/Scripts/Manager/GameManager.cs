using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public static GameManager instance;
    internal string aliveTime;
    private float timer;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (PlayerCollision.playerInstance.isPlayerAlive)
        {
            timer += Time.deltaTime;
            float mins = Mathf.Floor(timer / 60);
            string minutes = mins > 0 ? mins.ToString("00") + "m:" : "";
            string seconds = (timer % 60).ToString("00");

            aliveTime = string.Format("{0}{1}s", minutes, seconds);
        }
        else
        {
            gameOverUI.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
