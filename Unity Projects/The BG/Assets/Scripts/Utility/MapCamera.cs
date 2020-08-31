using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Transform playerObject;

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            float yPosiiton = playerObject.position.y > 3 ? 6.5f : 3.0f;
            transform.position = new Vector3(playerObject.position.x, yPosiiton, playerObject.position.z);
        }
    }
}
