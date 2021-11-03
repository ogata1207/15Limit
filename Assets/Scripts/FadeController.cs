using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : OGT_Utility.Singleton<FadeController>
{

    public SpriteMask mask;
    public SpriteRenderer renderer;
    public Camera fadeCamera;
    public bool isFinish;

    private TimeManagerStatus timeManagerStatus;

    // Use this for initialization
    void Start()
    {
        timeManagerStatus = TimeManagerStatus.GetInstance;
        TakeOverToTheNextScene();
    }

    public IEnumerator FadeIn()
    {


        //カメラとスプライトの描画を有効にする
        fadeCamera.enabled = true;
        renderer.enabled = true;

        //トランジション
        float value = 0;
        float startTime = Time.unscaledTime;
        float elapsedTime;

        var wait = new OGT_Utility.RealtimeWaitForSeconds(0.0f);

        while (value < 1.0f)
        {
            elapsedTime = Time.unscaledTime - startTime;
            value = elapsedTime / timeManagerStatus.fadeInTime;
            mask.alphaCutoff = value;
            yield return wait;
        }

        isFinish = true;

        //重くなるので次使う時まで無効
        fadeCamera.enabled = false;
        renderer.enabled = false;



    }

    public IEnumerator FadeOut()
    {
        isFinish = false;

        //カメラとスプライトの描画を有効にする
        renderer.enabled = true;
        fadeCamera.enabled = true;

        //トランジション
        float value = 0;
        float startTime = Time.unscaledTime;
        float elapsedTime;
        var wait = new OGT_Utility.RealtimeWaitForSeconds(0.0f);
        while (value < 1.0f)
        {
            elapsedTime = Time.unscaledTime - startTime;
            value = elapsedTime / timeManagerStatus.fadeOuntTime;
            mask.alphaCutoff = 1 - value;
            yield return wait;
        }

    }

}
