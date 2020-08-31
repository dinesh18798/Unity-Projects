using UnityEngine;

public class GameCentreController : MonoBehaviour
{
    private SceneChanger sceneChanger;

    private void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }
    }

    public void LoadHomeScene()
    {
        sceneChanger.FadeToScene(SceneManagerController.Scenes.MAINGAME);
    }

    public void LoadQuizScene()
    {
        sceneChanger.FadeToScene(SceneManagerController.Scenes.QUIZ);
    }

    public void LoadPuzzleScene()
    {
        sceneChanger.FadeToScene(SceneManagerController.Scenes.PUZZLE);
    }
}
