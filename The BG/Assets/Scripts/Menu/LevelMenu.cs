using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public GameObject loadButton;
    private SceneChanger sceneChanger;

    private void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }
    }

    public void PlayGame(int gameLevel)
    {
        ApplicationUtil.GameLevel = (GameLevels)gameLevel;

#if (DEBUG)
        Debug.Log("Set current game level: " + ApplicationUtil.GameLevel.ToString());
#endif

        if (ApplicationUtil.GameLevel != GameLevels.Zombie)
            sceneChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        else
            sceneChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void LoadGame()
    {
        GameData gameData = SaveLoadSystem.LoadGame();
        ApplicationUtil.GameLevel = (GameLevels)gameData.gameLevel;
        Cursor.visible = false;
        sceneChanger.FadeToLevel(gameData.sceneIndex);
    }

    private void Update()
    {
        loadButton.SetActive(SaveLoadSystem.CheckLoadFile());
    }
}
