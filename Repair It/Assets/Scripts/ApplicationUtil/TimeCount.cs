using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCount : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = this.GetComponent<TextMeshProUGUI>();
        //textMesh.text = "Hello";
    }

    // Update is called once per frame
    void Update()
    {
        TimeCalculate();
    }

    private void TimeCalculate()
    {
        if (!ApplicationUtil.GamePaused)
        {
            currentTime += Time.deltaTime;
            string minutes = Mathf.Floor((currentTime % 3600) / 60).ToString("00");
            string second = (currentTime % 60).ToString("00");
            textMesh.text = minutes + ":" + second;

        }
    }
}
