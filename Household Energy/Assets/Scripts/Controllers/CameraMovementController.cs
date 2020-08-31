using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    [SerializeField]
    private bool lookAtPlayer = true;

    private Transform player;
    private Vector3 cameraOffset;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            cameraOffset = transform.position - player.position;

            /*player = playerObject.transform.Find("CameraLookObject").transform;
            cameraOffset = transform.position - player.position;*/
        }
    }

    void LateUpdate()
    {
        Vector3 newPos = player.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        /*float desiredYAngle = player.eulerAngles.y;
        float desiredXAngle = player.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);

         Vector3 newPos = player.position - (rotation * cameraOffset);
         transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
         transform.LookAt(player);*/
    }
}
