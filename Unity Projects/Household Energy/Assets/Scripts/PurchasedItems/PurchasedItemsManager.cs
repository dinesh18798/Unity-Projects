using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PurchasedItemsManager : MonoBehaviour
{
    private PurchasedItemsController purchasedItemsController;
    private GameObject applianceContainer;
    private GameObject utilityContainer;
    private Transform appliancesContainerTransfrom;
    private Transform utilitiesContainerTransform;

    internal List<GameObject> applianceContainerList;
    internal List<GameObject> utilityContainerList;

    public PurchasedItemsManager(PurchasedItemsController controller, GameObject appliancesContainer, GameObject applianceContainer, GameObject utilitiesContainer, GameObject utilityContainer)
    {
        purchasedItemsController = controller;
        this.applianceContainer = applianceContainer;
        this.utilityContainer = utilityContainer;

        appliancesContainerTransfrom = appliancesContainer.transform;
        utilitiesContainerTransform = utilitiesContainer.transform;

        applianceContainerList = new List<GameObject>();
        utilityContainerList = new List<GameObject>();
    }

    internal void LoadAppliances()
    {
        applianceContainerList.Clear();
        for (int i = 0; i < PlayerInfo.PurchasedAppliances.Count; i++)
        {
            string applianceType = PlayerInfo.PurchasedAppliances.Keys.ElementAt(i);

            GameObject tempApplianceContainer = Instantiate(applianceContainer);
            tempApplianceContainer.transform.SetParent(appliancesContainerTransfrom, false);
            tempApplianceContainer.gameObject.SetActive(true);

            tempApplianceContainer.transform.Find("ApplianceType").GetComponent<TextMeshProUGUI>().text = applianceType;

            ItemController tempItemController = tempApplianceContainer.AddComponent<ItemController>();
            tempItemController.PurchasedItemsController = purchasedItemsController;
            tempItemController.CtgID = CategoryID.APPLIANCES;
            tempItemController.ItemID = i;

            applianceContainerList.Add(tempApplianceContainer);
        }
    }

    internal void LoadUtilities()
    {
        utilityContainerList.Clear();
        for (int i = 0; i < PlayerInfo.PurchasedUtilities.Count; i++)
        {
            string utilityType = PlayerInfo.PurchasedUtilities.Keys.ElementAt(i);

            GameObject tempUtilityContainer = Instantiate(utilityContainer);
            tempUtilityContainer.transform.SetParent(utilitiesContainerTransform, false);
            tempUtilityContainer.gameObject.SetActive(true);

            tempUtilityContainer.transform.Find("UtilityType").GetComponent<TextMeshProUGUI>().text = utilityType;

            ItemController tempItemController = tempUtilityContainer.AddComponent<ItemController>();
            tempItemController.PurchasedItemsController = purchasedItemsController;
            tempItemController.CtgID = CategoryID.UTILITIES;
            tempItemController.ItemID = i;

            utilityContainerList.Add(tempUtilityContainer);
        }
    }

    internal void ResetAllColor(CategoryID categoryID)
    {
        if (categoryID == CategoryID.APPLIANCES)
        {
            foreach (GameObject appliance in applianceContainerList)
            {
                appliance.GetComponent<ItemController>().ResetColor();
            }
        }

        if (categoryID == CategoryID.UTILITIES)
        {
            foreach (GameObject utility in utilityContainerList)
            {
                utility.GetComponent<ItemController>().ResetColor();
            }
        }
    }
}
