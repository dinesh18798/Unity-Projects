using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Actions actions;
    private CharacterController characterController;
    private Animator animator;
    private float moveSpeed = 1.5f;
    private float rotateSpeed = 200f;
    private float turnSpeed;
    private GameController gameController;
    private GameObject character;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        actions = GetComponentInChildren<Actions>();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (ApplicationUtil.FirstLevel && ApplicationUtil.CurrentSceneIndex == 1)
            gameController.instruction.text = "Your enemies are hiding in some places. Take them down!!! Use W and S key to move, mouse to look around.";
    }

    void Update()
    {
        if (gameController.gameOverFlag) return;

        if (!ApplicationUtil.GamePaused)
        {
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis(Axis.VERTICAL);

            moveSpeed = Input.GetKey(KeyCode.LeftShift) ? 2.5f : 1.5f;

            var movement = new Vector3(horizontal, 0, vertical);

            transform.Rotate(transform.up, horizontal * Time.deltaTime * rotateSpeed);

            if (vertical != 0)
            {
                characterController.SimpleMove(transform.forward * moveSpeed * vertical);

                if (vertical < 0)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        actions.RunBackward();
                    else
                        actions.WalkBackward();
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        actions.Run();
                    else
                        actions.Walk();
                }
            }
            else
                actions.Stay();
        }
    }
}
