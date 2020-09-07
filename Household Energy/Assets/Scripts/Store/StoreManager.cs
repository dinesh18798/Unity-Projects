using UnityEngine;

public class StoreManager : MonoBehaviour
{
    private StoreGameController storeGameController;
    private AppliancesStoreManager appliancesStoreManager;
    private UtilitiesStoreManager utilitiesStoreManager;

    public StoreManager(StoreGameController storeGameController, GameObject appliancesContainer, GameObject applianceContainer, GameObject utilitiesContainer, GameObject utilityContainer)
    {
        this.storeGameController = storeGameController;
        appliancesStoreManager = new AppliancesStoreManager(storeGameController, appliancesContainer, applianceContainer);
        utilitiesStoreManager = new UtilitiesStoreManager(storeGameController, utilitiesContainer, utilityContainer);
    }

    internal void LoadToStore()
    {
        appliancesStoreManager.LoadAppliancesToStore();
        utilitiesStoreManager.LoadUtilitiesToStore();
    }
}