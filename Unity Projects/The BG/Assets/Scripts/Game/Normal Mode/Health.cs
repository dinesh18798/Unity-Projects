using UnityEngine;

public class Health : MonoBehaviour
{
    public float initialHealth = 5f;
    internal bool isAlive = false;

    private float currentHealth;

    private Actions playerActions;
    private EnemyActions enemyActions;
    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    private void OnEnable()
    {
        currentHealth = initialHealth;
        isAlive = true;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 && isAlive)
        {
            Die();
            isAlive = false;
        }

        if (gameObject.CompareTag("Player"))
            gameController.UpdateHealthStatus(currentHealth);
    }

    private void Die()
    {
        if (gameObject.CompareTag("Player") && isAlive)
        {
            playerActions = gameObject.GetComponentInChildren<Actions>();
            gameController.GameOver();
            playerActions.Death();
            Destroy(gameObject, 15f);
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            enemyActions = gameObject.GetComponentInChildren<EnemyActions>();
            enemyActions.Death();
            gameController.numberofEnemies -= 1;

            if (gameController.numberofEnemies == 0 && ApplicationUtil.CurrentSceneIndex == 2)
                gameController.YouWin();
#if (DEBUG)
            Debug.Log("Number of enemies: " + gameController.numberofEnemies);
#endif
            Destroy(gameObject, 4f);
        }
        return;
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
