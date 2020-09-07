using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZombieGameController : MonoBehaviour
{
    public TextMeshProUGUI currentAmmoText;
    public Image healthImage;
    public Image healthFade;
    public GameObject zombieObject;

    private float initialHealth;
    private float healthValue;
    private List<int> usedPoints;

    private AudioSource audioSource;
    private SceneChanger sceneChanger;
    private GameOver gameOver;
    internal int numberofZombies;
    internal bool gameOverFlag = false;

    private void Start()
    {
        ApplicationUtil.GameLevel = GameLevels.Zombie;
        SpawnEnemy();

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = ApplicationUtil.GameVolume;

        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOver>();
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
    }

    private void SpawnEnemy()
    {
        GameObject zombiePoints = GameObject.FindGameObjectWithTag("ZombiePoints");
        numberofZombies = zombiePoints.transform.childCount;

        for (int i = 0; i < numberofZombies; i++)
        {
            GameObject enemy = Instantiate(zombieObject, zombiePoints.transform.GetChild(i).transform);
            enemy.transform.parent = zombiePoints.transform.GetChild(i);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        currentAmmoText.text = ammo.ToString();
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
