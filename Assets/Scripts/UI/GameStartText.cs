using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartText : MonoBehaviour
{
    public Text text;
    public float maxSize;

    private float currentSize;
    private float startTime;
    private float currentTime;

    private TimeManager timeManager;

    void Start()
    {
        startTime = Time.unscaledTime;
        timeManager = TimeManager.GetInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.isCountDown)
        {
            currentTime = Time.unscaledTime - startTime;

            if (currentTime > 1.0f)
            {
                text.fontSize = 0;
                startTime = Time.unscaledTime;
            }
            
            text.fontSize = (int)Mathf.Lerp(0.0f, maxSize, currentTime);

            if (timeManager.GetUnscaledTimer() <= 0.7f) text.text = "START";
            else text.text = "" + timeManager.GetUnscaledTimer().ToString("F0");

        }
        else
        {
            text.text = "";
        }
    }
}
