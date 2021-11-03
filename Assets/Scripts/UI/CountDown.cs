using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text text;
    private TimeManager timeManager;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        timeManager = TimeManager.GetInstance;

        var wait = new OGT_Utility.RealtimeWaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            var time = Mathf.CeilToInt( timeManager.GetUnscaledTimer() );
            text.text = "" + time;

            if (time == 0) break;
            
        }

        text.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
