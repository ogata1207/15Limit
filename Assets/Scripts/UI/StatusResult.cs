using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusResult : MonoBehaviour
{
    public Text moveSpeedLevel;
    public Text attackSpeedLevel;
    public Text stunResistanceLevel;

    private PlayerStatus playerStatus;
    private TimeManager timeManager;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = PlayerStatus.GetInstance;
        timeManager = TimeManager.GetInstance;
        Open();    
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.B)) playerStatus.currentAttackPowerLevel += 1;
        if (Input.GetKeyDown(KeyCode.N)) playerStatus.currentAttackSpeedLevel += 1;
        if (Input.GetKeyDown(KeyCode.M)) playerStatus.currentCriticalLevel += 1;

#endif
        moveSpeedLevel.text         = "Lv." + playerStatus.currentAttackPowerLevel;
        attackSpeedLevel.text       = "Lv." + playerStatus.currentAttackSpeedLevel;
        stunResistanceLevel.text    = "Lv." + playerStatus.currentCriticalLevel;
    }

    /// <summary>
    /// ステータス強化画面を開く
    /// </summary>
    public void Open()
    {
        timeManager.timeScale = 0.0f;
    }

    /// <summary>
    /// ステータス強化画面を閉じる
    /// </summary>
    public void Close()
    {
        timeManager.timeScale = 1.0f;
    }
}
