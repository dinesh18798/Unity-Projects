using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct AssetInfo
{
    public string assetType;
    public int assetLevel;
    public string assetPurchasedDate;
}

public enum PlayerCharacters
{
    Amy = 0,
    AJ = 1
}

[Serializable]
public class PlayerCharacterPrefab
{
    public int characterID;
    public GameObject characterPrefab;
}

[Serializable]
public class PlayerData
{
    internal string playerName;
    internal int playerCharcterId;
    internal int coins;
    internal int quizLevel;
    internal int puzzleLevel;
    internal AssetInfo[] purchasedAppliancesInfos;
    internal AssetInfo[] purchasedUtilitiesInfo;

    public PlayerData()
    {
        playerName = PlayerInfo.PlayerName;
        playerCharcterId = PlayerInfo.PlayerCharacterID;
        coins = PlayerInfo.Coins;
        quizLevel = PlayerInfo.QuizCurrentLevel;
        puzzleLevel = PlayerInfo.PuzzleCurrentLevel;
        List<AssetInfo> purchasedAppliancesList = new List<AssetInfo>();
        List<AssetInfo> purchasedUtilitiesList = new List<AssetInfo>();

        for (int i = 0; i < PlayerInfo.PurchasedAppliances.Count; i++)
        {
            AssetInfo assetInfo = new AssetInfo();
            assetInfo.assetType = PlayerInfo.PurchasedAppliances.ElementAt(i).Key;
            assetInfo.assetLevel = PlayerInfo.PurchasedAppliances.ElementAt(i).Value.ApplianceLevel;
            assetInfo.assetPurchasedDate = PlayerInfo.PurchasedAppliances.ElementAt(i).Value.AppliancePurchasedDate.ToString();
            purchasedAppliancesList.Add(assetInfo);
        }

        for (int i = 0; i < PlayerInfo.PurchasedUtilities.Count; i++)
        {
            AssetInfo assetInfo = new AssetInfo();
            assetInfo.assetType = PlayerInfo.PurchasedUtilities.ElementAt(i).Key;
            assetInfo.assetLevel = PlayerInfo.PurchasedUtilities.ElementAt(i).Value.UtilityLevel;
            purchasedUtilitiesList.Add(assetInfo);
        }

        purchasedAppliancesInfos = purchasedAppliancesList.ToArray();
        purchasedUtilitiesInfo = purchasedUtilitiesList.ToArray();
    }
}
