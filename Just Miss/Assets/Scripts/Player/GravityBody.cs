using UnityEngine;

public class GravityBody : MonoBehaviour
{   
    public bool placeOnSurface = false;
    private GravityAttractor gravityAttractor;
    private Rigidbody rb;
   
    void Start()
    {
        GameObject globeGameObject = GameObject.FindGameObjectWithTag("Globe");

        if (globeGameObject != null)
        {
            gravityAttractor = globeGameObject.GetComponent<GravityAttractor>();
        }

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (placeOnSurface)
            gravityAttractor.PlaceOnSurface(rb);
        else
            gravityAttractor.Attract(rb);
    }
}
