using UnityEngine;

public class ZombieEnemyController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float hitAccuracy = 0.5f;

    [SerializeField]
    private float attackRefreshRate = 1f;

    [SerializeField]
    private int damage;
    private GameObject player;
    private ZombieAction zombieAction;
    private ZombieEnemyMovement movement;
    private float AttackDistance = 5.0f;
    private float timer;
    private ZombieHealth targetHealth;
    private ZombieHealth myHealth;
    private float attackTimer;

    public AudioSource attackSound;

    void Awake()
    {
        zombieAction = GetComponentInChildren<ZombieAction>();
        movement = GetComponent<ZombieEnemyMovement>();
        myHealth = GetComponent<ZombieHealth>();

        player = GameObject.FindGameObjectWithTag("Player");
        targetHealth = player.GetComponent<ZombieHealth>();

        damage = Random.Range(6, 10);
    }

    private void Update()
    {
        if (targetHealth != null && targetHealth.isAlive)
        {
            float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if (distance < AttackDistance)
            {
                attackTimer += Time.deltaTime;
                if (CanAttack())
                    Attack();
            }
        }
    }

    private void Attack()
    {
        gameObject.transform.LookAt(player.transform);
        if (movement.moving)
        {
            zombieAction.Stay();
            movement.moving = false;
        }

        attackTimer = 0;
        zombieAction.Attack();
        attackSound.Play();

        float random = Random.Range(0.0f, 1.0f);
        bool isHit = random > 1.0f - hitAccuracy;

        if (isHit && targetHealth.isAlive)
            targetHealth.TakeDamage(damage);
    }

    private bool CanAttack()
    {
        return attackTimer >= attackRefreshRate && myHealth.isAlive;
    }
}
