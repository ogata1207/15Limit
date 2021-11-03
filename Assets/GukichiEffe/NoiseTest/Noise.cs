using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    public Material VHS;
    [SerializeField, Range(0, 1)] float _bleeding = 0.8f;
    [SerializeField, Range(0, 1)] float _fringing = 1.0f;
    [SerializeField, Range(0, 1)] float _scanline = 0.125f;


    float NoiseInterval;
    float IntervalTime;

    private void Start()
    {
        //インターバル設定
        NoiseInterval = Random.Range(1.0f, 3.0f);
    }

    IEnumerator GeneratePulseNoise()
    {
        //良い感じのランダム値設定
        //Random.Range(a, b)はa以上b未満
        int size = Random.Range(0, 30);
        int j = Random.Range(1, 3) * 180;
        int k = Random.Range(6, 11) * 10;

        //ノイズ発生後、テクスチャを元に戻す為sinが0になる値を代入
        for (int i = Random.Range(-360, 1); i <= j; i += k)
        {
            if (i + k > j)
            {
                i = j;
            }
            VHS.SetFloat("_Amount2", 0.8f * Mathf.Sin(i * Mathf.Deg2Rad));
            VHS.SetFloat("_Size", size);
            yield return null;
        }
    }

    private void Update()
    {
        //ノイズをランダム秒後に発生させる
        IntervalTime += Time.deltaTime;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        VHS.SetFloat("_src", 0.5f);
        var bleedWidth = 0.04f * _bleeding;  // width of bleeding
        var bleedStep = 2.5f / src.width; // max interval of taps
        var bleedTaps = Mathf.CeilToInt(bleedWidth / bleedStep);
        var bleedDelta = bleedWidth / bleedTaps;
        var fringeWidth = 0.0025f * _fringing; // width of fringing

        VHS.SetInt("_Width", src.width);
        VHS.SetInt("_Height", src.height);
        VHS.SetInt("_BleedTaps", bleedTaps);
        VHS.SetFloat("_BleedDelta", bleedDelta);
        VHS.SetFloat("_FringeDelta", fringeWidth);
        VHS.SetFloat("_Scanline", _scanline);

        if (IntervalTime >= NoiseInterval)
        {
            StartCoroutine(GeneratePulseNoise());
            IntervalTime = 0;
            NoiseInterval = Random.Range(1.0f, 3.0f);
        }

        Graphics.Blit(src, dest, VHS);
    }
}
