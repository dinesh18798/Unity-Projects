using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResult : MonoBehaviour
{
    private GameController gameController;
    public GameObject resultUI;
    public TextMeshProUGUI resultText;
    public GameObject[] starsObjects;
    public GameObject NextButton;
    private int levelTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerobject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerobject != null)
            gameController = gameControllerobject.GetComponent<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        levelTime = (int)Time.timeSinceLevelLoad;
        //Debug.Log("Level load time: " + levelTime);
        if (gameController.showResult) Result();
    }


    public void Result()
    {
        if (gameController.isLevelCompleted)
        {
            resultText.text = "You Win";
            StarsUpdate();
            if (ApplicationUtil.CurrentLevel < ApplicationUtil.MaximumLevel) NextButton.SetActive(true);
        }
        else
            resultText.text = "Game Over";

        resultUI.SetActive(true);
    }

    public void OnClickHome()
    {
        gameController.LoadMainMenu();
    }

    public void Restart()
    {
        gameController.ReloadScene();
    }

    public void OnClickNextlevel()
    {
        gameController.LoadNextLevel();
    }

    private void StarsUpdate()
    {
        int str = GetStars();

        for (int i = 0; i < str; i++)
        {
            GameObject star = starsObjects[i];
            GameObject children = star.transform.GetChild(0).gameObject;
            children.SetActive(true);
        }
    }

    private int GetStars()
    {
        int star = 1;
        if (0 < levelTime && levelTime <= 120)
        {
            star = 3;
        }
        else if (120 < levelTime && levelTime <= 240)
        {
            star = 2;
        }

        return star;
    }

}
