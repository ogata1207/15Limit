using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    public Text text;

    private TimeManager timeManager;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = TimeManager.GetInstance;
    }

    // Update is called once per frame
    void Update()
    {
        var time = timeManager.GetCountDownTimer().ToString("F0");
        text.text = ""+ time;    
    }
}
