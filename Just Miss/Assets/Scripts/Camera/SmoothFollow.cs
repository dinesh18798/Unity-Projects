using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    private Transform target;
    private Transform globeTransform;
    public float smoothness = 50f;
    public float rotationSmoothness = 60f;
    public float defaultDistance = 12f;

    private float planetDefaultScale;

    private void Awake()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");

        if (playerGameObject != null)
        {
            target = playerGameObject.GetComponent<Transform>();
        }

        GameObject globeGameObject = GameObject.FindGameObjectWithTag("Globe");

        if (globeGameObject != null)
        {
            globeTransform = globeGameObject.GetComponent<Transform>();
            planetDefaultScale = globeTransform.localScale.x;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            float tempDistance = (defaultDistance * globeTransform.localScale.x) / planetDefaultScale;

            Vector3 diffVector = target.position - globeTransform.position;
            diffVector += diffVector.normalized * tempDistance;

            Vector3 targetPosition = globeTransform.position + diffVector;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.fixedDeltaTime);

            Quaternion targetRot = Quaternion.LookRotation(-transform.position.normalized, target.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSmoothness);
        }
    }
}
