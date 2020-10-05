using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 200f;

    private Rigidbody playerRigidbody;
    private float direction;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + transform.forward * moveSpeed * Time.fixedDeltaTime);

        Vector3 yRotation = Vector3.up * direction * rotationSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(yRotation);
        Quaternion targetRotation = playerRigidbody.rotation * deltaRotation;
        playerRigidbody.MoveRotation(Quaternion.Slerp(playerRigidbody.rotation, targetRotation, 50f * Time.deltaTime));
    }
}

