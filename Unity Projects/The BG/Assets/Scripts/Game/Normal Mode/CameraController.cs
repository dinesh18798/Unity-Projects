using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineComposer composer;
    private float sensivity = 0.15f;

    void Start()
    {
        composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        if (!ApplicationUtil.GamePaused)
        {
            float vertical = Input.GetAxis("Mouse Y") * sensivity;
            composer.m_TrackedObjectOffset.y += vertical;
            composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, 1.0f, 2.5f);
        }
    }
}
