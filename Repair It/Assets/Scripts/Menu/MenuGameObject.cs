using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuGameObject : MonoBehaviour
{

    public Slider soundSlider;
    public Slider musicSlider;
    public TMP_InputField playerInput;
    private AudioSource audioSource;
    private SceneChanger sceneChanger;

    // Start is called before the first frame update
    void Start()
    {

        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }

        audioSource = GetComponent<AudioSource>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        musicSlider.value = ApplicationUtil.GameMusicVolume;
        soundSlider.value = ApplicationUtil.GameSoundVolume;
    }

    public void OnMusicVolumeChange()
    {
        ApplicationUtil.GameMusicVolume = musicSlider.value;
    }

    public void OnSoundVolumeChange()
    {
        ApplicationUtil.GameSoundVolume = soundSlider.value;
        audioSource.volume = ApplicationUtil.GameSoundVolume;
    }

    public void QuitGame()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }

    public void OnPlayerNameEnter()
    {
        Debug.Log("Player Name: " + playerInput.text);
        ApplicationUtil.PlayerName = playerInput.text;
    }

    public void PlayGame()
    {
        sceneChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
