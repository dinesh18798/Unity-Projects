using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Actions actions;
    private float moveSpeed = 1.0f;
    private float rotateSpeed = 50f;
    private float turnSmoothVelocity;

    public float smoothTime = 0.1f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        actions = GetComponentInChildren<Actions>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis(Axis.HORIZONTAL);
        float vertical = Input.GetAxis(Axis.VERTICAL);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(Time.deltaTime * moveSpeed * moveDir.normalized);

            actions.Walk(moveSpeed * moveDir.normalized.magnitude);
        }
        else
            actions.Stay();
    }
}
