using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI finalScore;

    void Start()
    {
        finalScore.text = string.Format("Escape Time: {0}", GameManager.instance.aliveTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.Restart();
        }
    }
}
