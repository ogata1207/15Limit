using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeGauge : OGT_Utility.Singleton<ChargeGauge>
{
    public Slider slider;

    private bool isUsingItElsewhere;    //他の場所で使っているかどうか

    // Start is called before the first frame update
    void Start()
    {
        slider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void GaugeUpdate()
    {
        //ゲージを進める
        slider.value += Time.deltaTime;
    }

    public bool isFinish()
    {
        if (!isUsingItElsewhere) return true;
        
        if(CurrentProgressPercentage() >= 1.0f)
        {
            Reset();
            return true;
        }

        return false;
    }

    public bool SetGauge(float maxValue)
    {
        if (isUsingItElsewhere) return false;       //使用中

        isUsingItElsewhere = true;

        slider.gameObject.SetActive(isUsingItElsewhere);

        slider.value = 0.0f;
        slider.maxValue = maxValue;

        return true;
    }

    public float CurrentProgressPercentage()
    {
        if (!isUsingItElsewhere) return 0.0f;
        return (slider.value / slider.maxValue);
    }

    public void Reset()
    {
        slider.value = 0.0f;
        slider.gameObject.SetActive(false);

        isUsingItElsewhere = false;
    }
}
