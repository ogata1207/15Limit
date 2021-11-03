using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : OGT_Utility.Singleton<TimeManager>
{
    private float unscaledCountDownTimer;           //タイムスケールに影響しないタイマー
    private float countDownTimer;                   //カウントダウン用のタイマー
    private IEnumerator countDownCoroutine;         //減算用コルーチン

    public bool isCountDown;                        //unscaledTimerのオンオフ用
    public bool stopFlg;
 
    public float timeScale
    {
        set { Time.timeScale = value; }
        get { return Time.timeScale; }

    }
    // Start is called before the first frame update
    void Start()
    {
        //シーン遷移後も保持する
        TakeOverToTheNextScene();
        stopFlg = false;

    }

    // Update is called once per frame
    void Update()
    {
        //減算
        if(isCountDown)
            unscaledCountDownTimer = Mathf.Clamp(unscaledCountDownTimer - Time.unscaledDeltaTime, 0, float.MaxValue);
    }

    #region Getter / Setter
    //*************************************************************************************************************************

    public void SetUnscaledTimerEnabled(bool flg)
    {
        isCountDown = flg;
    }
    //タイムスケールに影響しないタイマーの時間をセット
    public void SetUnscaledTimer(float time)
    {
        unscaledCountDownTimer = time;
    }    

    //タイムスケールに影響しないタイマーの時間を取得
    public float GetUnscaledTimer()
    {
        return unscaledCountDownTimer;
    }

    //*** カウントダウン系 ***//
    #region CountDown
    
    //時間をセット
    public void SetCountDownTimer(float time)
    {
        countDownTimer = time;
    }
    //カウントダウン用の時間を取得
    public float GetCountDownTimer()
    {
        return countDownTimer;
    }

    //カウント開始
    public void StartCountDown()
    {
        ResetCountDown();
        countDownCoroutine = countDown();
        StartCoroutine(countDownCoroutine);
    }
    //カウント中断
    public void StopCountDown()
    {
        if(countDownCoroutine != null)
        StopCoroutine(countDownCoroutine);
    }
    public void ResetCountDown()
    {
        StopCountDown();
        countDownCoroutine = null;
    }
    #endregion
    //*************************************************************************************************************************
    #endregion
    private IEnumerator countDown()
    {
        while(true)
        {


            //減算
            if(!stopFlg)
            countDownTimer = Mathf.Clamp(countDownTimer - 0.1f, 0, float.MaxValue);
            yield return new WaitForSeconds(0.1f);
        }

    }

}
