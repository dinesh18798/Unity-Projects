using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (!PlayerCollision.playerInstance.isPlayerAlive)
        {
            gameObject.SetActive(false);
        }

        scoreText.text = GameManager.instance.aliveTime;
    }
}
