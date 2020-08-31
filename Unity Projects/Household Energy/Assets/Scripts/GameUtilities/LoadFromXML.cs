using System;
using System.Xml;
using UnityEngine;

public class LoadFromXML
{
    public void LoadAllAppliances()
    {
        if (PlayerInfo.AllAppliancesList != null && PlayerInfo.AllAppliancesList.Count != 0) return;

        TextAsset textAsset = (TextAsset)Resources.Load("Appliances");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList itemList = xmlDoc.GetElementsByTagName("ApplianceItem");

        try
        {
            foreach (XmlNode item in itemList)
            {
                string applianceType = item["ApplianceType"].InnerText;
                Appliance appliance = new Appliance(applianceType);
                XmlNodeList childList = item.ChildNodes;

                foreach (XmlNode childNode in childList)
                {
                    if (childNode.Name == "ApplianceInfo")
                    {
                        ApplianceInfo applianceInfo = new ApplianceInfo();
                        applianceInfo.ApplianceLevel = int.Parse(childNode["ApplianceLevel"].InnerText);
                        applianceInfo.AppliancePrice = int.Parse(childNode["AppliancePrice"].InnerText);
                        applianceInfo.ApplianceConsumeEnergy = float.Parse(childNode["ApplianceConsumeEnergy"].InnerText);
                        applianceInfo.ApplianceEfficiency = float.Parse(childNode["ApplianceEfficiency"].InnerText);
                        applianceInfo.ApplianceMaterialType = childNode["ApplianceMaterialType"].InnerText;
                        applianceInfo.ApplianceLifeTimeSpan = int.Parse(childNode["ApplianceLifeTimeSpan"].InnerText);

                        appliance.ApplianceInfoList.Add(applianceInfo);
                    }
                }
                PlayerInfo.AllAppliancesList.Add(appliance);
            }
        }
        catch (Exception exp)
        {
            Debug.LogError("Unable to load all aplliances: " + exp.StackTrace);
            throw;
        }
    }

    public void LoadAllUtilities()
    {
        if (PlayerInfo.AllUtilitiesList != null && PlayerInfo.AllUtilitiesList.Count != 0) return;

        TextAsset textAsset = (TextAsset)Resources.Load("Utilities");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList itemList = xmlDoc.GetElementsByTagName("UtilityItem");

        try
        {
            foreach (XmlNode item in itemList)
            {
                string utilityType = item["UtilityType"].InnerText;
                Utility utility = new Utility(utilityType);
                XmlNodeList childList = item.ChildNodes;

                foreach (XmlNode childNode in childList)
                {
                    if (childNode.Name == "UtilityInfo")
                    {
                        UtilityInfo utilityInfo = new UtilityInfo();
                        utilityInfo.UtilityLevel = int.Parse(childNode["UtilityLevel"].InnerText);
                        utilityInfo.UtilityPrice = int.Parse(childNode["UtilityPrice"].InnerText);
                        utilityInfo.UtilitySavingEnergy = float.Parse(childNode["UtilitySavingEnergy"].InnerText);
                        utilityInfo.UtilityEfficiency = float.Parse(childNode["UtilityEfficiency"].InnerText);
                        utilityInfo.UtilityMaterialType = childNode["UtilityMaterialType"].InnerText;

                        utility.UtilityInfoList.Add(utilityInfo);
                    }
                }
                PlayerInfo.AllUtilitiesList.Add(utility);
            }
        }
        catch (Exception exp)
        {
            Debug.LogError("Unable to load all utility: " + exp.StackTrace);
            throw;
        }
    }
}
