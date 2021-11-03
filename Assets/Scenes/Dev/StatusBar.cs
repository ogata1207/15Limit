using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatusGroup
{
    public Slider slider;
    public Text text;
}

public class StatusBar : MonoBehaviour
{
    [SerializeField]
    public StatusGroup[] status;
    private PlayerStatus player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerStatus.GetInstance;    
    }

    // Update is called once per frame  
    void Update()
    {
        //Debug.Log("search tinpo :" + (player.currentCriticalLevel / player.MaxCriticalLevel));
        status[0].slider.value = ((float)player.currentCriticalLevel / (float)player.MaxCriticalLevel);
        status[0].text.text = "Lv " + player.currentCriticalLevel + "/Max " + player.MaxCriticalLevel;

        status[1].slider.value = ((float)player.currentAttackPowerLevel / (float)player.MaxAttackPowerLevel);
        status[1].text.text = "Lv " + player.currentAttackPowerLevel + "/Max " + player.MaxAttackPowerLevel;

        status[2].slider.value = ((float)player.currentAttackSpeedLevel / (float)player.MaxAttackSpeedLevel);
        status[2].text.text = "Lv " + player.currentAttackSpeedLevel + "/Max " + player.MaxAttackSpeedLevel;
    }
}
