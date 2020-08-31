using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleResultController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resultText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI coinsText;

    [SerializeField]
    private GameObject backButton;

    private int tempCoins;
    private SceneChanger sceneChanger;

    private void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }
    }

    public void UpdateScore(int correctAnswers)
    {
        int correctScore = correctAnswers * 100;
        scoreText.text = string.Format("+{0}", correctScore);
        tempCoins = PlayerInfo.Coins;
        PlayerInfo.Coins += correctScore;
        StartCoroutine(TotalCoinsUpdator());
    }

    internal void RepeatedLevel()
    {
        resultText.text = "You have been already passed this level";
        resultText.enableAutoSizing = true;
        scoreText.text = String.Empty;
        coinsText.text = String.Format("Total Coins: {0}", PlayerInfo.Coins);
        backButton.SetActive(true);
    }

    private IEnumerator TotalCoinsUpdator()
    {
        while (tempCoins < PlayerInfo.Coins)
        {
            tempCoins += 2;
            coinsText.text = String.Format("Total Coins: {0}", tempCoins);
            yield return new WaitForSeconds(0.005f); // Used 0.005f secs to update it
        }
        backButton.SetActive(true);
        PlayerInfo.PuzzleCurrentLevel = PlayerInfo.PuzzleCurrentLevel < PlayerInfo.MaxPuzzleLevel ? PlayerInfo.PuzzleCurrentLevel + 1 : PlayerInfo.PuzzleCurrentLevel;
        SaveAndLoadManager.SavePlayerData();
    }

    public void ReloadPuzzleScene()
    {
       sceneChanger.FadeToScene(SceneManagerController.Scenes.PUZZLE);
    }
}
