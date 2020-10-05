using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;

    private Transform globeTransform;
    private Rigidbody rb;

    private bool isCollideWithOcean = false;
    private float gravity = -10f;

    private void Awake()
    {
        GameObject globeGameObject = GameObject.FindGameObjectWithTag("Globe");

        if (globeGameObject != null)
        {
            globeTransform = globeGameObject.GetComponent<Transform>();
        }

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (isCollideWithOcean) return;

        Vector3 gravityUp = (transform.position - globeTransform.position).normalized;
        rb.AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityUp) * rb.rotation;
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Globe")
        {
            isCollideWithOcean = true;
            Invoke("BombBlast", 2f);
        }
    }

    internal void BombBlast()
    {
        // Play blast effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
