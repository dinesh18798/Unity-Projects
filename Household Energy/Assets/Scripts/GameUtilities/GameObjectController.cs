using UnityEngine;

public class GameObjectController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScreenshotHandler.TakeTheShot(1920, 1080);
        }
    }
}
