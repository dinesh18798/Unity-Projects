using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmartMeterController : MonoBehaviour
{
    [SerializeField]
    private GameObject smartMeterPanel;

    private GameObject smartMeterInfoPanel;
    private GameObject claimText;
    private GameObject claimButton;
    private MainGameController mainGameController;

    private void OnEnable()
    {
        try
        {
            GetGameObject();
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void GetGameObject()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameObject != null)
        {
            mainGameController = gameObject.GetComponent<MainGameController>();
        }

        if (smartMeterPanel != null)
        {
            smartMeterInfoPanel = smartMeterPanel.transform.Find("SmartMeterInfoPanel").gameObject;
            if (smartMeterInfoPanel != null)
            {
                UpdateValues();

                smartMeterInfoPanel.transform.Find("InfoText").GetComponent<TextMeshProUGUI>().text =
                    string.Format("Always, try to maintain the overall energy efficiency more than {0}%", GameInfo.CurrentTargetEfficiency);

                claimText = smartMeterInfoPanel.transform.Find("ClaimText").gameObject;
                claimButton = smartMeterInfoPanel.transform.Find("ClaimButton").gameObject;
                claimButton.GetComponent<Button>().onClick.AddListener(delegate { CoinsClaimed(); });
                claimText.SetActive(false);
                claimButton.SetActive(false);   
            }
        }
    }

    private void CoinsClaimed()
    {
        GameInfo.CurrentTargetEfficiency = GameInfo.MaxTargetEfficiency > GameInfo.CurrentTargetEfficiency
               ? GameInfo.CurrentTargetEfficiency + 5 : GameInfo.CurrentTargetEfficiency;

        claimText.SetActive(false);
        claimButton.SetActive(false);
        PlayerInfo.Coins += 200;
        mainGameController.UpdateCoins();

        SaveAndLoadManager.SaveGameData();
        SaveAndLoadManager.SavePlayerData();
    }

    private void UpdateValues()
    {

        double overallEnergy = 0;
        double overallEffectiveEnergy = 0;
        double overallEnergyLoss = 0;
        double overallSavingEnergy = 0;

        foreach (ApplianceInfo applianceInfo in PlayerInfo.PurchasedAppliances.Values)
        {
            float tempEnergy = applianceInfo.ApplianceConsumeEnergy;

            int timeDiff = 0;
            float tempEfficiency = applianceInfo.ApplianceEfficiency;
            if (applianceInfo.ApplianceLifeTimeSpan > 0)
            {
                timeDiff = (int)(DateTime.UtcNow.Subtract(applianceInfo.AppliancePurchasedDate)).TotalHours;
                if (timeDiff > applianceInfo.ApplianceLifeTimeSpan)
                {
                    tempEfficiency -= 10;
                }
            }

            float tempEffective = (tempEnergy * tempEfficiency) / 100;
            float tempLoss = tempEnergy - tempEffective;

            overallEnergy += tempEnergy;
            overallEffectiveEnergy += tempEffective;
            overallEnergyLoss += tempLoss;
        }

        foreach (UtilityInfo utilityInfo in PlayerInfo.PurchasedUtilities.Values)
        {
            float tempEnergy = utilityInfo.UtilitySavingEnergy;
            float tempEfficiency = utilityInfo.UtilityEfficiency;
            float tempEffective = (tempEnergy * tempEfficiency) / 100;

            overallSavingEnergy += tempEffective;
        }

        double overallEfficiency = overallEffectiveEnergy / (overallEnergy - overallSavingEnergy) * 100;

        while (overallEfficiency > GameInfo.CurrentTargetEfficiency)
        {
            EnableClaim();
        }

        AssignValues(overallEnergy, overallEffectiveEnergy, overallEnergyLoss, overallSavingEnergy, overallEfficiency);
    }

    private void EnableClaim()
    {
        claimText.SetActive(true);
        claimButton.SetActive(true);
    }

    private void AssignValues(double overallEnergy, double overallEffectiveEnergy, double overallEnergyLoss,
        double overallSavingEnergy, double overallEfficiency)
    {
        smartMeterInfoPanel.transform.Find("InfoText").GetComponent<TextMeshProUGUI>().text =
            string.Format("Always, try to maintain the overall energy efficiency more than {0}%", GameInfo.CurrentTargetEfficiency);

        smartMeterInfoPanel.transform.Find("OverallConsumeEnergyText").GetComponent<TextMeshProUGUI>().text =
            string.Format("Overall Consume Energy: {0} kWh", Math.Round(overallEnergy, 2));

        smartMeterInfoPanel.transform.Find("ConsumeEffectivelyText").GetComponent<TextMeshProUGUI>().text =
           string.Format("Energy Consume Effectively: {0} kWh", Math.Round(overallEffectiveEnergy, 2));

        smartMeterInfoPanel.transform.Find("EnergyWasteText").GetComponent<TextMeshProUGUI>().text =
          string.Format("Energy Waste or Loss: {0} kWh", Math.Round(overallEnergyLoss, 2));

        smartMeterInfoPanel.transform.Find("EnergySavingText").GetComponent<TextMeshProUGUI>().text =
         string.Format("Energy Produced or Save: {0} kWh", Math.Round(overallSavingEnergy, 2));

        smartMeterInfoPanel.transform.Find("OverallEnergyEfficiencyText").GetComponent<TextMeshProUGUI>().text =
         string.Format("Overall Energy Efficiency: {0}%", Math.Round(overallEfficiency, 2));
    }
}