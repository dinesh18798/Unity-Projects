using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public AudioSource walkingSound;

    internal bool moving = false;

    private EnemyActions enemyActions;
    private NavMeshAgent navMeshAgent;
    private AggroDetection aggroDetection;
    private Transform target;
    private float followDistance = 10f;
    private GameObject player;
    private Health myHealth;
    private float distance;

    private void Awake()
    {
        enemyActions = GetComponentInChildren<EnemyActions>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        myHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (player == null) return;

        if (navMeshAgent.enabled && myHealth.isAlive)
        {
            distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            bool shoot = false;
            bool follow = distance < followDistance;

            if (follow)
            {
                enemyActions.Walk(navMeshAgent.velocity.magnitude);
                navMeshAgent.SetDestination(player.transform.position);
                moving = true;
                gameObject.transform.LookAt(player.transform);
                return;
            }

            if (!follow || shoot)
            {
                navMeshAgent.SetDestination(gameObject.transform.position);
                enemyActions.Stay();
            }
            moving = false;
        }
    }
}
