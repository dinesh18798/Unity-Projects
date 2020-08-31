using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizGameController : MonoBehaviour
{
    [SerializeField]
    private Button[] levelButtons;
    private SceneChanger sceneChanger;

    private void Awake()
    {
        foreach (Button button in levelButtons)
        {
            button.interactable = false;
        }
    }

    private void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        for (int i = 0; i < PlayerInfo.QuizCurrentLevel; i++)
        {
            levelButtons[i].interactable = true;
        }
    }

    public void ReturnToGameCenter()
    {
        sceneChanger.FadeToScene(SceneManagerController.Scenes.GAMECENTRE);
    }
}
