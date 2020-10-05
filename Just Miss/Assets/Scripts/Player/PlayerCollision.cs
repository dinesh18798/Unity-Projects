using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    internal static PlayerCollision playerInstance;
    internal bool isPlayerAlive = true;

    private void Awake()
    {
        playerInstance = this;
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bomb")
        {
            isPlayerAlive = false;

            Bomb bomb = collision.gameObject.GetComponent<Bomb>();
            bomb.BombBlast();
            Destroy(gameObject);
        }
    }
}
