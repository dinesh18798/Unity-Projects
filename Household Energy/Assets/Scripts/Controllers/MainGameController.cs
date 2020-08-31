using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class MainGameController : MonoBehaviour
{
    [SerializeField]
    private List<PlayerCharacterPrefab> playerCharacterPrefabs;

    [SerializeField]
    private TextMeshProUGUI coinText;

    [SerializeField]
    private GameObject houseBuilding;

    [SerializeField]
    private List<GameObject> appliancesPrefabs;

    [SerializeField]
    private GameObject dialogueCanvas;

    [SerializeField]
    private GameObject welcomeNote;

    private GameObject playerGameObject;
    private SceneChanger sceneChanger;

    //Enable to test loading from main scene directly
    //private LoadFromXML loadAppliances;

    private NPCConversation conversation;

    private void Awake()
    {
        PlayerCharacterCreation();
    }

    void Start()
    {
        UpdateCoins();
        UpdateAppliances();

        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }

        if (GameInfo.IsNewGame)
        {
            dialogueCanvas.SetActive(true);
            welcomeNote.SetActive(false);

            GameObject gameObject = GameObject.FindGameObjectWithTag("DialogueGameController");
            if (gameObject != null)
            {
                conversation = gameObject.GetComponent<NPCConversation>();
                if (conversation != null) StartCoroutine(StartIntroduction());
            }
        }
        else
        {
            dialogueCanvas.SetActive(false);
            welcomeNote.SetActive(true);

            GameObject welcomeNoteText = welcomeNote.transform.Find("WelcomeNoteText").gameObject;
            if (welcomeNoteText != null)
            {
                welcomeNoteText.GetComponent<TextMeshProUGUI>().text = string.Format("Welcome back, {0}!", PlayerInfo.PlayerName);
                StartCoroutine(HideWelcomeNote());
            }
        }
    }


    private IEnumerator StartIntroduction()
    {
        yield return new WaitForSeconds(3.0f);
        ConversationManager.Instance.StartConversation(conversation);
    }


    private IEnumerator HideWelcomeNote()
    {
        yield return new WaitForSeconds(4.0f);
        Animator welcomeNoteAnimator = welcomeNote.GetComponent<Animator>();
        if (welcomeNoteAnimator != null)
        {
            welcomeNoteAnimator.SetTrigger("Exit");
            yield return new WaitForSeconds(1.0f);
            welcomeNote.SetActive(false);
        }
    }

    public PlayerData PlayerData { get; private set; }

    private void PlayerCharacterCreation()
    {
        //--Create Character
        GameObject tempPrefab = playerCharacterPrefabs[PlayerInfo.PlayerCharacterID].characterPrefab;
        playerGameObject = Instantiate(tempPrefab) as GameObject;
    }

    internal void UpdateCoins()
    {
        coinText.text = String.Format("Coins: {0}", PlayerInfo.Coins);
    }

    private void UpdateAppliances()
    {
        foreach (KeyValuePair<string, ApplianceInfo> appliance in PlayerInfo.PurchasedAppliances)
        {
            string appliancesType = Regex.Replace(appliance.Key, @"\s+", "");
            GameObject gameObject = GameObject.Find(appliancesType);
            if (appliancesType != "LightBulb")
            {
                if (gameObject == null)
                {
                    foreach (GameObject prefab in appliancesPrefabs)
                    {
                        if (prefab.CompareTag(appliancesType))
                        {
                            GameObject go = Instantiate(prefab) as GameObject;
                            go.transform.parent = houseBuilding.transform;
                            break;
                        }
                    }

                }
                else if (!gameObject.activeInHierarchy)
                    gameObject.SetActive(true);
            }
        }
    }

    public void LoadStoreScene()
    {
        sceneChanger.FadeToScene(SceneManagerController.Scenes.STORE);
    }

    public void LoadGameCentreScene()
    {
        sceneChanger.FadeToScene(SceneManagerController.Scenes.GAMECENTRE);
    }

    public void CloseDialogueCanvas()
    {
        if (GameInfo.IsNewGame && dialogueCanvas != null)
        {
            GameInfo.IsNewGame = false;
            dialogueCanvas.SetActive(false);
        }
    }
}
