using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image healthImage;
    public Image healthFade;
    public Text instruction;

    private float initialHealth;
    private float healthValue;
    private GameLevels currentLevel;
    public GameObject enemyObject;
    private List<int> usedPoints;

    internal int numberofEnemies;
    
    private AudioSource audioSource;
    private SceneChanger sceneChanger;
    private GameOver gameOver;
    internal bool gameOverFlag = false;

    private void Awake()
    {
        currentLevel = ApplicationUtil.GameLevel;
        usedPoints = new List<int>();

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = ApplicationUtil.GameVolume;

        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOver>();

        Cursor.visible = false;

        if (ApplicationUtil.CurrentSceneIndex != 3) SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        GameObject enemyPoints = GameObject.FindGameObjectWithTag("EnemyPoints");
        int totalEnemyPoints = enemyPoints.transform.childCount;

        numberofEnemies = currentLevel == GameLevels.Easy ? 4 : totalEnemyPoints;

        for (int i = 0; i < numberofEnemies; i++)
        {
            int enemyPoint = Random.Range(0, totalEnemyPoints);

            while (usedPoints.Contains(enemyPoint))
            {
                enemyPoint = Random.Range(0, totalEnemyPoints);
            }
            usedPoints.Add(enemyPoint);

            GameObject enemy = Instantiate(enemyObject, enemyPoints.transform.GetChild(enemyPoint).transform);
            enemy.transform.parent = enemyPoints.transform.GetChild(enemyPoint);
        }
    }

    public void UpdateHealthStatus(float health)
    {
        health /= 50f;

        if (health <= 0.3)
            healthImage.color = Color.red;
        else
            healthImage.color = new Color32(7, 238, 0, 125);

        healthImage.fillAmount = health;

        if (health == 1) initialHealth = health;
        healthValue = health;

        UpdateHealthFadeImage();
    }

    private void UpdateHealthFadeImage()
    {
        if (healthValue >= initialHealth)
        {
            healthFade.color = new Color(healthFade.color.r, healthFade.color.g, healthFade.color.b, 0f);
            return;
        }

        if (healthValue < initialHealth)
            healthFade.color = new Color(healthFade.color.r, healthFade.color.g, healthFade.color.b, 0.25f);
        else if (healthValue < 0.5)
            healthFade.color = new Color(healthFade.color.r, healthFade.color.g, healthFade.color.b, 0.5f);
        else
            healthFade.color = new Color(healthFade.color.r, healthFade.color.g, healthFade.color.b, 0.75f);
    }

    internal void GameOver()
    {
        gameOverFlag = true;
        gameOver.DisplayGameOver();
    }

    internal void YouWin()
    {
        gameOverFlag = true;
        gameOver.DisplayYouWin();
    }

    public void LoadNextScene()
    {
        if (ApplicationUtil.GameLevel != GameLevels.Zombie)
            sceneChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        if (SceneManager.GetSceneAt(0) != null)
        {
            sceneChanger.FadeToMainMenu();
        }
    }

    public void VolumeUpdate()
    {
        if (audioSource != null)
            audioSource.volume = ApplicationUtil.GameVolume;
    }

    public void MusicUpdate()
    {
        if (audioSource != null)
        {
            if (ApplicationUtil.GameMusic)
                audioSource.Play();
            else
                audioSource.Stop();
        }
    }
}
