using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppliancesStoreManager : MonoBehaviour
{
    private StoreGameController storeGameController;
    private RectTransform allAppliancesRectTrans;
    private RectTransform applianceContainerRectTrans;

    public AppliancesStoreManager(StoreGameController storeGameController, GameObject appliancesContainer, GameObject applianceContainer)
    {
        this.storeGameController = storeGameController;
        allAppliancesRectTrans = appliancesContainer.GetComponent<RectTransform>();
        applianceContainerRectTrans = applianceContainer.GetComponent<RectTransform>();
    }

    internal void LoadAppliancesToStore()
    {
        for (int i = 0; i < PlayerInfo.AllAppliancesList.Count; i++)
        {
            Appliance appliance = PlayerInfo.AllAppliancesList.ElementAt(i);

            RectTransform applianceContainer = Instantiate(applianceContainerRectTrans);
            applianceContainer.gameObject.transform.SetParent(allAppliancesRectTrans, false);
            applianceContainer.gameObject.SetActive(true);

            int moduloValue = i % 3;
            float xPosition = 70 + ((140 + applianceContainer.sizeDelta.x) * moduloValue);

            int divisionValue = i / 3;
            float yPosition = -(70 + (140 + applianceContainer.sizeDelta.y) * divisionValue);
            applianceContainer.anchoredPosition = new Vector2(xPosition, yPosition);

            applianceContainer.Find("ApplianceName").GetComponent<TextMeshProUGUI>().text = appliance.ApplianceType;

            string tempType = Regex.Replace(appliance.ApplianceType, @"\s+", "");
            Sprite tempSprite = Resources.Load<Sprite>("Images/Appliances/" + tempType);
            applianceContainer.Find("ApplianceImage").GetComponent<Image>().sprite = tempSprite;

            UpdateDisplayApplianceInfo(appliance, applianceContainer);
        }

        float containerHeight = 640 * (PlayerInfo.AllAppliancesList.Count / 3);
        allAppliancesRectTrans.sizeDelta = new Vector2(allAppliancesRectTrans.sizeDelta.x, containerHeight);
    }

    //--- Need to check when the appliance level reach maximum
    private void TaskOnClick(Appliance appliance, RectTransform applianceContainer)
    {
        ApplianceInfo applianceInfo = appliance.ApplianceInfoList[appliance.ApplianceCurrentLevel];

        if (appliance.ApplianceCurrentLevel == applianceInfo.ApplianceLevel) return;
        int currentCoins = applianceInfo.AppliancePrice;
        if (currentCoins > PlayerInfo.Coins) return;

        PlayerInfo.Coins -= currentCoins;
        appliance.ApplianceCurrentLevel = applianceInfo.ApplianceLevel;
        storeGameController.UpdateCoin();

        applianceInfo.AppliancePurchasedDate = DateTime.UtcNow;

        if (PlayerInfo.PurchasedAppliances.ContainsKey(appliance.ApplianceType))
        {
            PlayerInfo.PurchasedAppliances[appliance.ApplianceType] = applianceInfo;
        }
        else
            PlayerInfo.PurchasedAppliances.Add(appliance.ApplianceType, applianceInfo);

        SaveAndLoadManager.SavePlayerData();
        UpdateDisplayApplianceInfo(appliance, applianceContainer);
    }

    private void UpdateDisplayApplianceInfo(Appliance appliance, RectTransform applianceContainer)
    {
        if (appliance.ApplianceCurrentLevel < appliance.ApplianceInfoList.Count)
        {
            ApplianceInfo currentApplianceInfo = appliance.ApplianceInfoList[appliance.ApplianceCurrentLevel];

            string diffConsume = string.Empty;
            string diffEffciency = string.Empty;
            if(appliance.ApplianceCurrentLevel > 0)
            {
                ApplianceInfo prevApplianceInfo = appliance.ApplianceInfoList[appliance.ApplianceCurrentLevel - 1];
                double tempConsume = Math.Round(Convert.ToDouble(currentApplianceInfo.ApplianceConsumeEnergy - prevApplianceInfo.ApplianceConsumeEnergy), 2);
                diffConsume = tempConsume < 0 ? string.Format("<#40ff80>{0}</color>", tempConsume) : string.Format("<#D75454>+{0}</color>", tempConsume);

                double tempEff = Math.Round(Convert.ToDouble(currentApplianceInfo.ApplianceEfficiency - prevApplianceInfo.ApplianceEfficiency), 2);
                diffEffciency = tempEff > 0 ? string.Format("<#40ff80>+{0}</color>", tempEff) : string.Format("<#D75454>{0}</color>", tempEff);
            }

            string energyText = String.Format("Energy Consume: {0} kWh  {1}", currentApplianceInfo.ApplianceConsumeEnergy, diffConsume);
            if (currentApplianceInfo.ApplianceEfficiency != 0f)
            {
                string energyEfficiency = String.Format("Energy Efficiency: {0} %  {1}", currentApplianceInfo.ApplianceEfficiency, diffEffciency);
                energyText += "\n" + energyEfficiency;
            }
            applianceContainer.Find("ApplianceEnergy").GetComponent<TextMeshProUGUI>().text = energyText;

            string buttonText = appliance.ApplianceCurrentLevel == 0 ? "Purchase" : "Upgrade";
            string price = String.Format("{0} coins", currentApplianceInfo.AppliancePrice);

            applianceContainer.Find("ApplianceButton").Find("ApplianceButtonText").GetComponent<TextMeshProUGUI>().text = buttonText + "\n" + price;

            string fullText = String.Format("Level: {0}", currentApplianceInfo.ApplianceLevel);
            if (!String.IsNullOrEmpty(currentApplianceInfo.ApplianceMaterialType))
            {
                string materialType = String.Format("Material Type: {0}", currentApplianceInfo.ApplianceMaterialType);
                fullText += "\n" + materialType;
            }
            applianceContainer.Find("ApplianceLevelAndType").GetComponent<TextMeshProUGUI>().text = fullText;
        }
        else
        {

            ApplianceInfo currentApplianceInfo = appliance.ApplianceInfoList[appliance.ApplianceInfoList.Count - 1];

            string energyText = String.Format("Energy Consume: {0} kWh", currentApplianceInfo.ApplianceConsumeEnergy);
            if (currentApplianceInfo.ApplianceEfficiency != 0f)
            {
                string energyEfficiency = String.Format("Energy Efficiency: {0} %", currentApplianceInfo.ApplianceEfficiency);
                energyText += "\n" + energyEfficiency;
            }
            applianceContainer.Find("ApplianceEnergy").GetComponent<TextMeshProUGUI>().text = energyText;

            string maxText = String.Format("Maximum Level Reached");
            applianceContainer.Find("ApplianceLevelAndType").GetComponent<TextMeshProUGUI>().text = maxText;
        }

        bool enableButton = appliance.ApplianceCurrentLevel != appliance.ApplianceInfoList.Count;

        Button applianceButton = applianceContainer.Find("ApplianceButton").GetComponent<Button>();
        applianceButton.interactable = enableButton;

        applianceButton.onClick.RemoveAllListeners();
        applianceButton.onClick.AddListener(delegate
        {
            TaskOnClick(appliance, applianceContainer);
        });
    }
}
