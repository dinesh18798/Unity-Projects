using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    public SimpleHealthBar barStatus;

    public float value, currentTime;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerobject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerobject != null)
            gameController = gameControllerobject.GetComponent<GameController>();

        barStatus = GetComponent<SimpleHealthBar>();
        value = 0.5f;
        

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        float time = currentTime * value;
        gameController.timeLimit = time;
        barStatus.UpdateBar(time, 100);
    }
}
