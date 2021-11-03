using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour {

    private Vector3 localPosition;
    private Coroutine shakeCoroutine;

    void Start()
    {
        TimeManager.GetInstance.stopFlg = false;
    }
    /// <summary>
    /// カメラをシェイクさせる
    /// </summary>
    /// <param name="power">強さ</param>
    /// <param name="time">再生する時間</param>
    public void PlayShake(float power, float time)
    {
        if (shakeCoroutine == null)
            shakeCoroutine = StartCoroutine(Shake(power, time));
    }

    public void PlayShake(Vector3 power, float time)
    {
        if (shakeCoroutine == null)
            shakeCoroutine = StartCoroutine(Shake(power, time));
    }

    IEnumerator Shake(float power, float time)
    {
        //開始前の座標を保存
        localPosition = transform.localPosition;

        //開始時の時間を保存
        var startTime = Time.unscaledTime;

        while(Time.unscaledTime - startTime < time)
        {
            float x = Random.Range(-power, power);
            float y = Random.Range(-power, power);

            transform.localPosition = localPosition + new Vector3(x, y);

            yield return null;
        }
        
        transform.localPosition = localPosition;
        shakeCoroutine = null;
    }

    IEnumerator Shake(Vector3 power, float time)
    {
        //開始前の座標を保存
        localPosition = transform.localPosition;

        //開始時の時間を保存
        var startTime = Time.unscaledTime;

        while (Time.unscaledTime - startTime < time)
        {
            int x = Random.Range(-1, 0);
            var dir = (x < 0) ? -1 : 1;
            transform.localPosition = localPosition + new Vector3(power.x * dir, power.y * dir);

            yield return null;
        }

        transform.localPosition = localPosition;
        shakeCoroutine = null;
    }
}
