using DialogueEditor;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionGameController : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> playerSprite;
    [SerializeField]
    private List<GameObject> player2DPrefabs;

    private NPCConversation conversation;
    private SceneChanger sceneChanger;
    private GameObject playerGameObject;

    private void Awake()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("DialogueGameController");
        if (gameObject != null)
        {
            conversation = gameObject.GetComponent<NPCConversation>();
        }

        PlayerCreation();
    }

    private void PlayerCreation()
    {
        GameObject tempPrefab = player2DPrefabs[PlayerInfo.PlayerCharacterID];
        playerGameObject = Instantiate(tempPrefab) as GameObject;
    }

    private void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }
    }

    internal void StartConversation()
    {
        if (playerSprite.Count > 0)
            ConversationManager.Instance.StartConversation(conversation, playerSprite);
        else
            ConversationManager.Instance.StartConversation(conversation);
    }

    public void StartGame()
    {
        sceneChanger.FadeToScene(SceneManagerController.Scenes.MAINGAME);
    }
}
