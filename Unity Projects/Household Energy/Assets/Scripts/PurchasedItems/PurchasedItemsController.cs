using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CategoryID
{
    APPLIANCES = 0,
    UTILITIES
}

public class PurchasedItemsController : MonoBehaviour
{
    [SerializeField]
    private GameObject appliancesContainer;

    [SerializeField]
    private GameObject applianceContainer;

    [SerializeField]
    private GameObject utilitiesContainer;

    [SerializeField]
    private GameObject utilityContainer;

    [SerializeField]
    private Transform descriptionContainer;

    private PurchasedItemsManager itemsManager;

    private void Start()
    {
        itemsManager = new PurchasedItemsManager(this, appliancesContainer, applianceContainer, utilitiesContainer, utilityContainer);
        itemsManager.LoadAppliances();
        itemsManager.LoadUtilities();
    }

    internal void ResetAllColor(CategoryID ctgID)
    {
        itemsManager.ResetAllColor(ctgID);
    }

    internal void DisplayItemDescription(CategoryID categoryID, int itemID)
    {
        descriptionContainer.gameObject.SetActive(true);
        if (categoryID == CategoryID.APPLIANCES)
            DisplayApplianceItem(itemID);
        if (categoryID == CategoryID.UTILITIES)
            DisplayUtilityItem(itemID);
    }

    private void DisplayApplianceItem(int itemID)
    {
        string applType = PlayerInfo.PurchasedAppliances.Keys.ElementAt(itemID);

        string tempType = Regex.Replace(applType, @"\s+", "");
        Sprite tempSprite = Resources.Load<Sprite>("Images/Appliances/" + tempType);
        descriptionContainer.Find("ItemImage").GetComponent<Image>().sprite = tempSprite;

        ApplianceInfo currentApplianceInfo = PlayerInfo.PurchasedAppliances.Values.ElementAt(itemID);

        string fullText = String.Format("Level: {0}", currentApplianceInfo.ApplianceLevel);
        if (!String.IsNullOrEmpty(currentApplianceInfo.ApplianceMaterialType))
        {
            string materialType = String.Format("Material Type: {0}", currentApplianceInfo.ApplianceMaterialType);
            fullText += "\n" + materialType;
        }
        descriptionContainer.Find("ItemLevel").GetComponent<TextMeshProUGUI>().text = fullText;

        int timeDiff = 0;
        if (currentApplianceInfo.ApplianceLifeTimeSpan > 0)
            timeDiff = (int)(DateTime.UtcNow.Subtract(currentApplianceInfo.AppliancePurchasedDate)).TotalHours;

        string energyText = String.Format("Energy Consume: {0} kWh", currentApplianceInfo.ApplianceConsumeEnergy);
        if (currentApplianceInfo.ApplianceEfficiency != 0f)
        {
            float efficiency = timeDiff > currentApplianceInfo.ApplianceLifeTimeSpan ? currentApplianceInfo.ApplianceEfficiency - 10 : currentApplianceInfo.ApplianceEfficiency;
            string energyEfficiency = String.Format("Energy Efficiency: {0} %", efficiency);
            energyText += "\n" + energyEfficiency;
        }
        descriptionContainer.Find("ItemEnergy").GetComponent<TextMeshProUGUI>().text = energyText;

        string timeText;
        Color color;
        if (timeDiff > currentApplianceInfo.ApplianceLifeTimeSpan)
        {
            timeText = "Required to Upgrade";
            color = new Color(1.0f, 0.15f, 0.0f);
        }
        else
        {
            timeText = string.Format("Remaining Span: {0} Hours", currentApplianceInfo.ApplianceLifeTimeSpan - timeDiff);
            color = Color.white;
        }
        descriptionContainer.Find("ItemSpan").GetComponent<TextMeshProUGUI>().text = timeText;
        descriptionContainer.Find("ItemSpan").GetComponent<TextMeshProUGUI>().color = color;
    }

    private void DisplayUtilityItem(int itemID)
    {
        string applType = PlayerInfo.PurchasedUtilities.Keys.ElementAt(itemID);

        string tempType = Regex.Replace(applType, @"\s+", "");
        Sprite tempSprite = Resources.Load<Sprite>("Images/Utilities/" + tempType);
        descriptionContainer.Find("ItemImage").GetComponent<Image>().sprite = tempSprite;

        UtilityInfo currentUtilityInfo = PlayerInfo.PurchasedUtilities.Values.ElementAt(itemID);

        string fullText = String.Format("Level: {0}", currentUtilityInfo.UtilityLevel);
        if (!String.IsNullOrEmpty(currentUtilityInfo.UtilityMaterialType))
        {
            string materialType = String.Format("Material Type: {0}", currentUtilityInfo.UtilityMaterialType);
            fullText += "\n" + materialType;
        }
        descriptionContainer.Find("ItemLevel").GetComponent<TextMeshProUGUI>().text = fullText;

        string energyText = String.Format("Energy Save: {0} kWh", currentUtilityInfo.UtilitySavingEnergy);
        if (currentUtilityInfo.UtilityEfficiency != 0f)
        {
            string energyEfficiency = String.Format("Energy Efficiency: {0} %", currentUtilityInfo.UtilityEfficiency);
            energyText += "\n" + energyEfficiency;
        }
        descriptionContainer.Find("ItemEnergy").GetComponent<TextMeshProUGUI>().text = energyText;

        descriptionContainer.Find("ItemSpan").GetComponent<TextMeshProUGUI>().text = "Remaining Span: Unlimited";
    }
}
