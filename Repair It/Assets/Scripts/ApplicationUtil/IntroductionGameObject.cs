using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroductionGameObject : MonoBehaviour
{
    public TextMeshProUGUI TxMeshPro;
    private SceneChanger sceneChanger;


    // Start is called before the first frame update
    void Start()
    {
        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }

        TxMeshPro.text = "Hi! " + ApplicationUtil.PlayerName + ", could you please help fix the pipes?";
    }

    public void OnConfirm()
    {
        sceneChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
