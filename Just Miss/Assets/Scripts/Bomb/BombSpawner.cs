using System.Collections;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject meteor;


    void Start()
    {
        StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnMeteor()
    {
        Vector3 spawnPositon = Random.onUnitSphere * 15;
        Instantiate(meteor, spawnPositon, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        if (PlayerCollision.playerInstance.isPlayerAlive)
        {
            StartCoroutine(SpawnMeteor());
        }
    }
}
