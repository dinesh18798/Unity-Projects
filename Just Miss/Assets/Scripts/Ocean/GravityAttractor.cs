using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -10f;
    public float scaleChange = 0.0015f;

    private SphereCollider sphereCollider;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    internal void Attract(Rigidbody body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        body.AddForce(gravityUp * gravity);
        Rotate(body);
    }

    internal void PlaceOnSurface(Rigidbody body)
    {
        body.MovePosition((body.position - transform.position).normalized * (transform.localScale.x * sphereCollider.radius));
        Rotate(body);
    }

    private void Rotate(Rigidbody body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(body.transform.up, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50f * Time.deltaTime);
    }

    void Update()
    {
        if (transform.localScale.x > 3f)
        {
            if (PlayerCollision.playerInstance.isPlayerAlive)
                transform.localScale *= 1f - scaleChange * Time.deltaTime;
            else
                transform.localScale *= 1f - 0.025f * Time.deltaTime;
        }
    }
}
