using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private Animator animator;
    private int sceneToLoadIndex;
    public Slider loadingBar;

    private AsyncOperation async;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeToLevel(int sceneIndex)
    {
        sceneToLoadIndex = sceneIndex;
        ApplicationUtil.CurrentSceneIndex = sceneToLoadIndex;
        SaveLoadSystem.SaveGame();
        animator.SetTrigger("FadeOut");
    }

    public void FadeToMainMenu()
    {
        sceneToLoadIndex = 0;
        if (Time.timeScale == 0) Time.timeScale = 1;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        StartCoroutine(LoadSceneWithBar());
    }

    IEnumerator LoadSceneWithBar()
    {
        async = SceneManager.LoadSceneAsync(sceneToLoadIndex);
        while (!async.isDone)
        {

#if (DEBUG)
            Debug.Log("Async: " + async.progress);
#endif
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}
