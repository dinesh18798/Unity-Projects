using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Arsenal[] arsenals;
    public Transform enemyWeaponHand;

    [Range(0.0f, 1.0f)]
    public float hitAccuracy = 0.5f;
    public AudioSource gunFireSound;

    private Animator animator;
    private EnemyActions enemyActions;
    private EnemyMovement movement;

    private float AttackDistance = 3.0f;
    private float attackRefreshRate = 1f;
    private int damage;
    private GameObject player;
    private float timer;
    private ParticleSystem muzzleParticle;
    private Health targetHealth;
    private Health myHealth;
    private float attackTimer;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyActions = GetComponentInChildren<EnemyActions>();
        movement = GetComponent<EnemyMovement>();
        myHealth = GetComponent<Health>();

        InstantiateWeapon(arsenals[0].name);

        player = GameObject.FindGameObjectWithTag("Player");
        targetHealth = player.GetComponent<Health>();

        //--Set some values based on the game level
        damage = ApplicationUtil.GameLevel == GameLevels.Easy ? 2 : 5;
        attackRefreshRate = ApplicationUtil.GameLevel == GameLevels.Easy ? 1.0f : 0.75f;

    }

    public void InstantiateWeapon(string name)
    {
        foreach (Arsenal arsenal in arsenals)
        {
            if (arsenal.name == name)
            {
                if (enemyWeaponHand.childCount > 0)
                    Destroy(enemyWeaponHand.GetChild(0).gameObject);

                if (arsenal.weapon != null)
                {
                    GameObject newGun = (GameObject)Instantiate(arsenal.weapon);
                    muzzleParticle = newGun.GetComponentInChildren<ParticleSystem>();
                    newGun.transform.parent = enemyWeaponHand;
                    newGun.layer = enemyWeaponHand.gameObject.layer;
                    newGun.transform.localPosition = new Vector3(-0.008f, 0.0085f, -0.02f);
                    newGun.transform.localRotation = Quaternion.Euler(-90f, 0, 90f);
                }
                animator.runtimeAnimatorController = arsenal.controller;
                return;
            }
        }
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
            enemyActions.Stay();
            movement.moving = false;
        }

        attackTimer = 0;

        muzzleParticle.Stop();
        muzzleParticle.Play();
        gunFireSound.Play();

        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        bool isHit = random > 1.0f - hitAccuracy;

        if (isHit)
            targetHealth.TakeDamage(damage);
    }

    private bool CanAttack()
    {
        return attackTimer >= attackRefreshRate && myHealth.isAlive;
    }

    [Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject weapon;
        public RuntimeAnimatorController controller;
    }
}
