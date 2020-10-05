using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f);
    }
}
