using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemyMovement : MonoBehaviour
{
    internal bool moving = false;

    private ZombieAction zombieAction;
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private float followDistance;
    private GameObject player;
    private ZombieHealth myHealth;

    private void Awake()
    {
        zombieAction = GetComponentInChildren<ZombieAction>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        myHealth = GetComponent<ZombieHealth>();

        followDistance = Random.Range(30f, 50f);
    }

    private void Update()
    {

        if (navMeshAgent.enabled && myHealth.isAlive)
        {
            float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            bool shoot = false;
            bool follow = distance < followDistance;

            if (follow)
            {
                zombieAction.Walk(navMeshAgent.velocity.magnitude);
                navMeshAgent.SetDestination(player.transform.position);
                moving = true;
                gameObject.transform.LookAt(player.transform);
                return;
            }

            if (!follow || shoot)
            {
                navMeshAgent.SetDestination(gameObject.transform.position);
                zombieAction.Stay();
            }
            moving = false;
        }
    }
}
