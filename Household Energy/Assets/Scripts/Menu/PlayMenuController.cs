using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class PlayMenuController : MonoBehaviour
{
    private GameObject continueButton;
    private GameObject loadStatus;
    private TextMeshProUGUI statusText;
    private LoadFromXML loadAppliances;
    private bool isGameInfoUpdated = false;
    private SceneChanger sceneChanger;

    private Thread updatePlayerInfoThread;

    void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }

        LoadGameData();
        continueButton = transform.Find("ContinueGameButton").gameObject;
        loadStatus = transform.Find("LoadStatus").gameObject;
        if (loadStatus != null) statusText = loadStatus.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!continueButton.activeSelf)
        {
            continueButton.SetActive(SaveAndLoadManager.CheckPlayerDataExist());
        }
        if (isGameInfoUpdated) ContinueGame();
    }

    private void ContinueGame()
    {
        GameInfo.IsNewGame = false;
        sceneChanger.FadeToScene(SceneManagerController.Scenes.MAINGAME);
    }

    private void LoadGameData()
    {
        //-- Load all appliances
        loadAppliances = new LoadFromXML();
        loadAppliances.LoadAllAppliances();
        loadAppliances.LoadAllUtilities();
    }

    public void LoadPlayerData()
    {
        PlayerData playerData = SaveAndLoadManager.LoadPlayerData();
        if (playerData != null)
        {
            updatePlayerInfoThread = new Thread(new ParameterizedThreadStart(UpdatePlayerInfo));
            updatePlayerInfoThread.Start(playerData);
        }
        else
        {
            statusText.text = "Unable to load data, the file has been corrupted!!! \n Try New Game";
            loadStatus.SetActive(true);
        }

    }

    private void UpdatePlayerInfo(object data)
    {
        if (data == null) return;

        PlayerData playerData = (PlayerData)data as PlayerData;

        PlayerInfo.PlayerCharacterID = playerData.playerCharcterId;
        PlayerInfo.PlayerName = playerData.playerName == null ? String.Empty : playerData.playerName;
        PlayerInfo.Coins = playerData.coins;
        PlayerInfo.PuzzleCurrentLevel = playerData.puzzleLevel;
        PlayerInfo.QuizCurrentLevel = playerData.quizLevel;

        try
        {
            foreach (AssetInfo asset in playerData.purchasedAppliancesInfos)
            {
                foreach (Appliance appliance in PlayerInfo.AllAppliancesList)
                {
                    if (asset.assetType == appliance.ApplianceType)
                    {
                        appliance.ApplianceCurrentLevel = asset.assetLevel;
                        ApplianceInfo applianceInfo = appliance.ApplianceInfoList[asset.assetLevel - 1];
                        applianceInfo.AppliancePurchasedDate = Convert.ToDateTime(asset.assetPurchasedDate);
                        PlayerInfo.PurchasedAppliances.Add(appliance.ApplianceType, applianceInfo);
                    }
                }
            }

            foreach (AssetInfo asset in playerData.purchasedUtilitiesInfo)
            {
                foreach (Utility utility in PlayerInfo.AllUtilitiesList)
                {
                    if (asset.assetType == utility.UtilityType)
                    {
                        utility.UtilityCurrentLevel = asset.assetLevel;
                        UtilityInfo utilityInfo = utility.UtilityInfoList[asset.assetLevel - 1];
                        PlayerInfo.PurchasedUtilities.Add(utility.UtilityType, utilityInfo);
                    }
                }
            }

            isGameInfoUpdated = true;
        }
        catch (Exception exp)
        {
            Debug.LogError("Unable to update game info" + exp.StackTrace);
        }
    }
}
