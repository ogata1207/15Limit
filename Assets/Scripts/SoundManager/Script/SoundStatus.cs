using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundStatus", menuName = "OGT/SoundStatus")]
public class SoundStatus : ScriptableObject {

    #region Instance

    /// <summary>
    /// Tableのパス
    /// </summary>
    [SerializeField, Header("Resourcesフォルダに置く")]

    public static string FILE_PATH = "Sound/SoundStatus";
    private static SoundStatus instance;
    public static SoundStatus GetInstance
    {
        get
        {
            if (instance == null)
            {
                var table = (SoundStatus)Resources.Load(FILE_PATH);
                if (table == null) Debug.LogError("指定のパスにTileStatusTableが存在ません");
                else instance = table;
            }
            return instance;
        }
    }

    #endregion

    const string filePath = "Sound/";

    [SerializeField]
    public Dictionary<string, AudioClip> bgmList;
    [SerializeField]
    public Dictionary<string, AudioClip> seList;

    public float masterVolume;
    public float bgmVolume;
    public float seVolume;

    public void LoadFile()
    {
        //Resourcesからファイルをロード
        AudioClip[] bgm = Resources.LoadAll<AudioClip>(filePath + "BGM");
        AudioClip[] se = Resources.LoadAll<AudioClip>(filePath + "SE");

        //BGMをリストに追加
        bgmList = new Dictionary<string, AudioClip>();
        foreach (var index in bgm)
        {
            bgmList.Add(index.name, index);
        }

        //SEをリストに追加
        seList = new Dictionary<string, AudioClip>();
        foreach (var index in se)
        {
            seList.Add(index.name, index);
        }
    }

}
