using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        if (camera != null)
        {
            Animator cameraAnimator = camera.GetComponent<Animator>();
            cameraAnimator.SetTrigger("StartGame");
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
