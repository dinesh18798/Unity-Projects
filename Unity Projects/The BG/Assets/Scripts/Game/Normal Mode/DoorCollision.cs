using UnityEngine;
using UnityEngine.UI;

public class DoorCollision : MonoBehaviour
{
    private Animator animator;
    private GameController gameController;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    }

    private void KickDoor()
    {
        if (ApplicationUtil.FirstLevel && ApplicationUtil.CurrentSceneIndex == 1)
        {
            gameController.instruction.text = "Press K to kick the red door";
            gameController.instruction.enabled = true;
        }
    }

    private void LoadNextScene()
    {
        ApplicationUtil.FirstLevel = false;
        gameController.LoadNextScene();
    }

    private void Update()
    {
        if (gameController.numberofEnemies <= 0)
        {
            KickDoor();
            if (Input.GetKey(KeyCode.K))
            {
                gameController.instruction.enabled = false;
                animator.SetTrigger("Kicked");
            }
        }
    }
}
