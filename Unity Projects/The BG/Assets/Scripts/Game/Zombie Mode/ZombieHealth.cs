using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float initialHealth = 10f;

    internal bool isAlive = false;

    private float currentHealth;
    private ZombieGameController gameController;
    private Animator animator = null;
    private ZombieAction zombieAction;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("ZombieGameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<ZombieGameController>();
        }

        if (gameObject.CompareTag("Enemy")) animator = GetComponentInChildren<Animator>();

        zombieAction = gameObject.GetComponentInChildren<ZombieAction>();
    }

    private void OnEnable()
    {
        currentHealth = initialHealth;
        isAlive = true;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 && isAlive)
        {
            Die();
            isAlive = false;
        }

        if (gameObject.CompareTag("Player"))
        {
            gameController.UpdateHealthStatus(currentHealth);
        }
    }

    private void Die()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            gameController.numberofZombies -= 1;
#if (DEBUG)
            Debug.Log("Enemny: " + gameController.numberofZombies);
#endif

            if (gameController.numberofZombies == 0)
                gameController.YouWin();

            zombieAction.Death();
            Destroy(gameObject, 4f);
        }

        if (gameObject.CompareTag("Player"))
        {
            gameController.GameOver();
            Destroy(gameObject, 15f);
        }
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (currentHealth < initialHealth)
                currentHealth += Time.deltaTime;
            gameController.UpdateHealthStatus(currentHealth);
        }
    }
}
