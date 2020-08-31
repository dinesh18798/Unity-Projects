using UnityEngine;

public class Introduction : MonoBehaviour
{
    private IntroductionGameController gameController;

    private void Start()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
        if(gameObject!=null)
        {
            gameController = gameObject.GetComponent<IntroductionGameController>();
        }
    }

    void BeginConversation()
    {
        gameController.StartConversation();
    }
}
