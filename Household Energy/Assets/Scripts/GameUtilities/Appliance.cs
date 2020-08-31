using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.UI;

public class Appliance
{
    public Appliance(string type) : this(type, 0, 0)
    {

    }

    public Appliance(string type, int currentLevel = 0, int count = 0)

    {
        this.ApplianceType = type;
        this.ApplianceCurrentLevel = currentLevel;
        this.ApplianceCount = count;
        this.ApplianceInfoList = new List<ApplianceInfo>();
    }

    [XmlElement(elementName: "ApplianceType")]
    public string ApplianceType { get; set; }

    public int ApplianceCurrentLevel { get; set; }

    public Image ApplianceImage { get; set; }

    public int ApplianceCount { get; set; }

    public List<ApplianceInfo> ApplianceInfoList { get; set; }
}

[Serializable()]
public class ApplianceInfo
{
    [XmlElement(elementName: "ApplianceLevel")]
    public int ApplianceLevel { get; set; }

    [XmlElement(elementName: "AppliancePrice")]
    public int AppliancePrice { get; set; }

    [XmlElement(elementName: "ApplianceConsumeEnergy")]
    // In kilo-watt hour kWh unit
    public float ApplianceConsumeEnergy { get; set; }

    [XmlElement(elementName: "ApplianceEfficiency")]
    public float ApplianceEfficiency { get; set; }

    [XmlElement(elementName: "ApplianceMaterialType")]
    public string ApplianceMaterialType { get; set; }

    [XmlElement(elementName: "ApplianceLifeTimeSpan")]
    public int ApplianceLifeTimeSpan { get; set; }

    public DateTime AppliancePurchasedDate { get; set; }
}
