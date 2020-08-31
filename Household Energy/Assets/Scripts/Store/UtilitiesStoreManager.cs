using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UtilitiesStoreManager : MonoBehaviour
{

    private StoreGameController storeGameController;
    private RectTransform allUtilitiesRectTrans;
    private RectTransform utilityContainerRectTrans;

    public UtilitiesStoreManager(StoreGameController storeGameController, GameObject utilitiesContainer, GameObject utilityContainer)
    {
        this.storeGameController = storeGameController;
        allUtilitiesRectTrans = utilitiesContainer.GetComponent<RectTransform>();
        utilityContainerRectTrans = utilityContainer.GetComponent<RectTransform>();
    }

    internal void LoadUtilitiesToStore()
    {
        for (int i = 0; i < PlayerInfo.AllUtilitiesList.Count; i++)
        {
            Utility utility = PlayerInfo.AllUtilitiesList.ElementAt(i);

            RectTransform utilityContainer = Instantiate(utilityContainerRectTrans);
            utilityContainer.gameObject.transform.SetParent(allUtilitiesRectTrans, false);
            utilityContainer.gameObject.SetActive(true);

            int moduloValue = i % 3;
            float xPosition = 70 + ((140 + utilityContainer.sizeDelta.x) * moduloValue);

            int divisionValue = i / 3;
            float yPosition = -(70 + (140 + utilityContainer.sizeDelta.y) * divisionValue);
            utilityContainer.anchoredPosition = new Vector2(xPosition, yPosition);

            utilityContainer.Find("UtilityName").GetComponent<TextMeshProUGUI>().text = utility.UtilityType;

            string tempType = Regex.Replace(utility.UtilityType, @"\s+", "");
            Sprite tempSprite = Resources.Load<Sprite>("Images/Utilities/" + tempType);
            utilityContainer.Find("UtilityImage").GetComponent<Image>().sprite = tempSprite;

            UpdateDisplayUtilityInfo(utility, utilityContainer);
        }

        float containerHeight = 640 + (640 * (PlayerInfo.AllUtilitiesList.Count / 3));
        allUtilitiesRectTrans.sizeDelta = new Vector2(allUtilitiesRectTrans.sizeDelta.x, containerHeight);
    }

    //--- Need to check when the utility level reach maximum
    private void TaskOnClick(Utility utility, RectTransform utilityContainer)
    {
        UtilityInfo utilityInfo = utility.UtilityInfoList[utility.UtilityCurrentLevel];

        if (utility.UtilityCurrentLevel == utilityInfo.UtilityLevel) return; // Need to work on this
        utility.UtilityCurrentLevel = utilityInfo.UtilityLevel;

        int currentCoins = utilityInfo.UtilityPrice;
        if (currentCoins > PlayerInfo.Coins) return;

        PlayerInfo.Coins -= currentCoins;
        storeGameController.UpdateCoin();

        if (PlayerInfo.PurchasedUtilities.ContainsKey(utility.UtilityType))
        {
            PlayerInfo.PurchasedUtilities[utility.UtilityType] = utilityInfo;
        }
        else
            PlayerInfo.PurchasedUtilities.Add(utility.UtilityType, utilityInfo);

        SaveAndLoadManager.SavePlayerData();
        UpdateDisplayUtilityInfo(utility, utilityContainer);
    }

    private void UpdateDisplayUtilityInfo(Utility utility, RectTransform utilityContainer)
    {

        if (utility.UtilityCurrentLevel < utility.UtilityInfoList.Count)
        {
            UtilityInfo currentUtilityInfo = utility.UtilityInfoList[utility.UtilityCurrentLevel];

            string diffSaving = string.Empty;
            string diffEffciency = string.Empty;
            if (utility.UtilityCurrentLevel > 0)
            {
                UtilityInfo prevUtilityInfo = utility.UtilityInfoList[utility.UtilityCurrentLevel - 1];
                double tempSave = Math.Round(Convert.ToDouble(currentUtilityInfo.UtilitySavingEnergy - prevUtilityInfo.UtilitySavingEnergy), 2);
                diffSaving = tempSave > 0 ? string.Format("<#40ff80>+{0}</color>", tempSave) : string.Format("<#D75454>{0}</color>", tempSave);

                double tempEff = Math.Round(Convert.ToDouble(currentUtilityInfo.UtilityEfficiency - prevUtilityInfo.UtilityEfficiency), 2);
                diffEffciency = tempEff > 0 ? string.Format("<#40ff80>+{0}</color>", tempEff) : string.Format("<#D75454>{0}</color>", tempEff);
            }

            string energyText = String.Format("Energy Save: {0} kWh  {1}", currentUtilityInfo.UtilitySavingEnergy, diffSaving);

            if (currentUtilityInfo.UtilityEfficiency != 0f)
            {
                string energyEfficiency = String.Format("Energy Efficiency: {0} %  {1}", currentUtilityInfo.UtilityEfficiency, diffEffciency);
                energyText += "\n" + energyEfficiency;
            }
            utilityContainer.Find("UtilityEnergy").GetComponent<TextMeshProUGUI>().text = energyText;

            string buttonText = utility.UtilityCurrentLevel == 0 ? "Purchase" : "Upgrade";
            string price = String.Format("{0} coins", currentUtilityInfo.UtilityPrice);

            utilityContainer.Find("UtilityButton").Find("UtilityButtonText").GetComponent<TextMeshProUGUI>().text = buttonText + "\n" + price;

            string fullText = String.Format("Level: {0}", currentUtilityInfo.UtilityLevel);
            if (!String.IsNullOrEmpty(currentUtilityInfo.UtilityMaterialType))
            {
                string materialType = String.Format("Type: {0}", currentUtilityInfo.UtilityMaterialType);
                fullText += "\n" + materialType;
            }
            utilityContainer.Find("UtilityLevelAndType").GetComponent<TextMeshProUGUI>().text = fullText;
        }
        else
        {
            UtilityInfo currentUtilityInfo = utility.UtilityInfoList[utility.UtilityInfoList.Count - 1];

            string energyText = String.Format("Energy Save: {0} kWh", currentUtilityInfo.UtilitySavingEnergy);

            if (currentUtilityInfo.UtilityEfficiency != 0f)
            {
                string energyEfficiency = String.Format("Energy Efficiency: {0} %", currentUtilityInfo.UtilityEfficiency);
                energyText += "\n" + energyEfficiency;
            }
            utilityContainer.Find("UtilityEnergy").GetComponent<TextMeshProUGUI>().text = energyText;

            string maxText = String.Format("Maximum Level Reached");
            utilityContainer.Find("UtilityLevelAndType").GetComponent<TextMeshProUGUI>().text = maxText;
        }

        bool enableButton = utility.UtilityCurrentLevel != utility.UtilityInfoList.Count;

        Button utilityButton = utilityContainer.Find("UtilityButton").GetComponent<Button>();
        utilityButton.interactable = enableButton;

        utilityButton.onClick.RemoveAllListeners();
        utilityButton.onClick.AddListener(delegate
        {
            TaskOnClick(utility, utilityContainer);
        });
    }
}
