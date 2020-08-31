using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyCentreController : MonoBehaviour
{
    [SerializeField]
    private GameObject graphContainer;

    [SerializeField]
    private Sprite dotSprite;

    [SerializeField]
    private TMP_Dropdown graphDropdown;

    [SerializeField]
    private TextMeshProUGUI graphTitle;

    private GraphGenerator graphGenerator;
    private static Dictionary<string, float> graphValueList;
    private GraphType currentGraphType = GraphType.ENERGY_CONSUMPTION;
    private bool isBarChart = true;
    private int maxVisibleAmount = 0;

    private void Awake()
    {
        graphGenerator = new GraphGenerator(graphContainer, dotSprite);
        graphValueList = new Dictionary<string, float>();

        graphDropdown.onValueChanged.AddListener(delegate
        {
            UpdateGraphType(graphDropdown);
        });
    }

    public void UpdateToBarChart()
    {
        isBarChart = true;
        graphGenerator.GenerateGraph(graphValueList, isBarChart, maxVisibleAmount);
    }

    public void UpdateToLineChart()
    {
        isBarChart = false;
        graphGenerator.GenerateGraph(graphValueList, isBarChart, maxVisibleAmount);
    }

    public void IncreaseVisibleAmount()
    {
        if (maxVisibleAmount < graphValueList.Count) maxVisibleAmount += 1;
        graphGenerator.GenerateGraph(graphValueList, isBarChart, maxVisibleAmount);
    }
    public void DecreaseVisibleAmount()
    {
        if (maxVisibleAmount > 1) maxVisibleAmount -= 1;
        graphGenerator.GenerateGraph(graphValueList, isBarChart, maxVisibleAmount);
    }

    public void UpdateGraph()
    {
        UpadateValues();
        maxVisibleAmount = graphValueList.Count;

        if (graphValueList.Count >= 0)
            graphGenerator.GenerateGraph(graphValueList, isBarChart, maxVisibleAmount);
    }

    public void CleanUpGraph()
    {
        graphValueList.Clear();
        graphGenerator.ClearGraphGameObjects();
    }

    private void UpadateValues()
    {
        switch (currentGraphType)
        {
            case GraphType.ENERGY_CONSUMPTION:
            case GraphType.ENERGY_CONSUMING_EFFICIENCY:
                AppliancesRelatedGraph();
                break;
            case GraphType.ENERGY_SAVING:
            case GraphType.ENERGY_SAVING_EFFICIENCY:
                UtilitiesRelatedGraph();
                break;
            case GraphType.OVERALL_ENERGY:
                OverallEnergyGraph();
                break;
            default:
                break;
        }
    }

    private void OverallEnergyGraph()
    {
        graphTitle.text = "Overall view of energy consuming and saving at home in kilowatt-hour (kWh)";

        graphValueList.Clear();
        float consumingEnergy = 0f;
        foreach (KeyValuePair<string, ApplianceInfo> appliance in PlayerInfo.PurchasedAppliances)
        {
            consumingEnergy += (float)Math.Round(appliance.Value.ApplianceConsumeEnergy, 3);
        }
        graphValueList.Add("Energy Consume", consumingEnergy);

        float savingEnergy = 0f;
        foreach (KeyValuePair<string, UtilityInfo> utility in PlayerInfo.PurchasedUtilities)
        {
            savingEnergy += (float)Math.Round(utility.Value.UtilitySavingEnergy, 3);
        }
        graphValueList.Add("Energy Save", savingEnergy);
    }

    private void AppliancesRelatedGraph()
    {
        graphTitle.text = currentGraphType == GraphType.ENERGY_CONSUMPTION ? "Consumption of energy in kilowatt-hour (kWh)" :
           "Efficiency of energy consume by each appliance, where high efficiency gives less energy waste";

        graphValueList.Clear();
        foreach (KeyValuePair<string, ApplianceInfo> appliance in PlayerInfo.PurchasedAppliances)
        {
            string applianceType = appliance.Key;

            float value = 0f;
            if (currentGraphType == GraphType.ENERGY_CONSUMPTION)
            {
                value = (float)Math.Round(appliance.Value.ApplianceConsumeEnergy, 3);
            }
            else if (currentGraphType == GraphType.ENERGY_CONSUMING_EFFICIENCY)
            {
                value = (float)Math.Round(appliance.Value.ApplianceEfficiency);
            }
            graphValueList.Add(applianceType, value);
        }
    }

    private void UtilitiesRelatedGraph()
    {
        graphTitle.text = currentGraphType == GraphType.ENERGY_SAVING ? "Saving of energy in kilowatt-hour (kWh)" :
            "Efficiency of energy saving by each utility, where high efficiency gives less energy waste";

        graphValueList.Clear();
        foreach (KeyValuePair<string, UtilityInfo> utility in PlayerInfo.PurchasedUtilities)
        {
            string utilityType = utility.Key;

            float value = 0f;
            if (currentGraphType == GraphType.ENERGY_SAVING)
            {
                value = (float)Math.Round(utility.Value.UtilitySavingEnergy, 3);
            }
            else if (currentGraphType == GraphType.ENERGY_SAVING_EFFICIENCY)
            {
                value = (float)Math.Round(utility.Value.UtilityEfficiency, 3);
            }
            graphValueList.Add(utilityType, value);
        }
    }

    private void UpdateGraphType(TMP_Dropdown graphDropdown)
    {
        currentGraphType = (GraphType)graphDropdown.value;
        UpdateGraph();
    }
}

public enum GraphType
{
    ENERGY_CONSUMPTION,
    ENERGY_CONSUMING_EFFICIENCY,
    ENERGY_SAVING,
    ENERGY_SAVING_EFFICIENCY,
    OVERALL_ENERGY
}