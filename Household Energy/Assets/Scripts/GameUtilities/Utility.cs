using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.UI;

public class Utility
{
    public Utility(string type) : this(type, 0, 0)
    {

    }

    public Utility(string type, int currentLevel = 0, int count = 0)

    {
        this.UtilityType = type;
        this.UtilityCurrentLevel = currentLevel;
        this.UtilityCount = count;
        this.UtilityInfoList = new List<UtilityInfo>();
    }

    [XmlElement(elementName: "UtilityType")]
    public string UtilityType { get; set; }

    public int UtilityCurrentLevel { get; set; }

    public Image UtilityImage { get; set; }

    public int UtilityCount { get; set; }

    public List<UtilityInfo> UtilityInfoList { get; set; }
}

[Serializable()]
public class UtilityInfo
{
    [XmlElement(elementName: "UtilityLevel")]
    public int UtilityLevel { get; set; }

    [XmlElement(elementName: "UtilityPrice")]
    public int UtilityPrice { get; set; }

    [XmlElement(elementName: "UtilitySavingEnergy")]
    // In kilo-watt hour kWh unit
    public float UtilitySavingEnergy { get; set; }

    [XmlElement(elementName: "UtilityEfficiency")]
    public float UtilityEfficiency { get; set; }

    [XmlElement(elementName: "UtilityMaterialType")]
    public string UtilityMaterialType { get; set; }
}
