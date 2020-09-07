using TMPro;
using UnityEngine;

public class StoreGameController : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    [SerializeField]
    private GameObject appliancesContainer;

    [SerializeField]
    private GameObject applianceContainer;

    [SerializeField]
    private GameObject utilitiesContainer;

    [SerializeField]
    private GameObject utilityContainer;

    private StoreManager storeManager;
    private SceneChanger sceneChanger;

    void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }

        UpdateCoin();

        storeManager = new StoreManager(this, appliancesContainer, applianceContainer, utilitiesContainer, utilityContainer);
        storeManager.LoadToStore();
    }

    public void UpdateCoin()
    {
        coinText.text = string.Format("Coins: {0}", PlayerInfo.Coins);
    }

    public void LoadHomeScene()
    {
       sceneChanger.FadeToScene(SceneManagerController.Scenes.MAINGAME);
    }
}
