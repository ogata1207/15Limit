using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OGT_Utility;
public class SoundManager : Singleton<SoundManager>
{
    private SoundStatus soundStatus;
    private float masterVolume { get; set; }
    private float bgmVolume { get; set; }
    private float seVolume { get; set; }

    private AudioSource bgmSource;
    private AudioSource subBgmSource;
    private AudioSource seSource;

    // Use this for initialization
    void Start()
    {
        //シーン遷移後に維持させる
        TakeOverToTheNextScene();

        //再生用コンポーネントの追加
        bgmSource = gameObject.AddComponent<AudioSource>();
        subBgmSource = gameObject.AddComponent<AudioSource>();
        seSource = gameObject.AddComponent<AudioSource>();

        //初期の音量を調整
        soundStatus = SoundStatus.GetInstance;
        soundStatus.LoadFile();

        SetMasterVolume(soundStatus.masterVolume);
        SetBgmVolume(soundStatus.bgmVolume);
        SetSeVolume(soundStatus.seVolume);
    }

    void Update()
    {

    }

    /// <summary>
    /// 全体の音量を調整
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;

        SetBgmVolume(bgmVolume);
        SetSeVolume(seVolume);
    }

    /// <summary>
    /// BGMの音量を変更
    /// </summary>
    /// <param name="volume"></param>
    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume * masterVolume;
    }

    /// <summary>
    /// SEの音量を変更
    /// </summary>
    /// <param name="volume"></param>
    public void SetSeVolume(float volume)
    {
        seVolume = volume;
        seSource.volume = seVolume * masterVolume;
    }

    /// <summary>
    /// BGMを再生
    /// </summary>
    /// <param name="name">ファイル名</param>
    public void PlayBGM(string name)
    {
        //使用するファイルをセット
        bgmSource.clip = soundStatus.bgmList[name];
        
        //音量の調整
        bgmSource.volume = masterVolume * bgmVolume;

        //ループの有無
        bgmSource.loop = true;

        //実行(再生)
        bgmSource.Play();
    }

    public void PlaySubBGM(string name)
    {
        //使用するファイルをセット
        subBgmSource.clip = soundStatus.bgmList[name];

        //音量の調整
        subBgmSource.volume = masterVolume * bgmVolume;

        //ループの有無
        subBgmSource.loop = true;

        //実行(再生)
        subBgmSource.Play();
    }

    /// <summary>
    /// BGMを止める
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopSubBGM()
    {
        subBgmSource.Stop();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void PlaySE(string name)
    {
        seSource.PlayOneShot(soundStatus.seList[name]);
    }
}
