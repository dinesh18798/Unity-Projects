using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject clickEfffect;

    void Start()
    {
        Cursor.visible = false;
    }

    private void OnMouseEnter()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        transform.position = mousePos;
    }
}
