using TMPro;
using UnityEngine;

public class NewGameMenuController : MonoBehaviour
{
    private GameObject nextButton;
    private TMP_InputField playerNameInput;
    private SceneChanger sceneChanger;

    private void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }

        GameInfo.IsNewGame = true;

        playerNameInput = transform.Find("PlayerName").Find("PlayerNameInput").GetComponent<TMP_InputField>();
        if (playerNameInput != null)
        {
            playerNameInput.onValueChanged.AddListener(delegate { PlayerNameValueChange(playerNameInput.text); });
            playerNameInput.onEndEdit.AddListener(delegate { PlayerNameEntered(playerNameInput.text); });
        }

        nextButton = transform.Find("PlayerName").Find("NextButton").gameObject;
    }

    private void PlayerNameEntered(string playerName)
    {
        Debug.Log("Input Edited: " + playerName);
        PlayerInfo.PlayerName = playerName;
    }

    private void PlayerNameValueChange(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            nextButton.SetActive(true);
        else
            nextButton.SetActive(false);
    }

    public void PlayerCharacterSelection(int playerID)
    {
        PlayerInfo.PlayerCharacterID = playerID;
        StartGame();
    }

    public void StartGame()
    {
        SaveAndLoadManager.SavePlayerData();
        if (GameInfo.IsNewGame)
            sceneChanger.FadeToScene(SceneManagerController.Scenes.INTRODUCTION);
    }
}
