using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TimeManagerStatus", menuName = "OGT/Status Table/TimeManagerStatus")]
public class TimeManagerStatus : ScriptableObject
{
    public static string FILE_PATH = "Status/TimeManagerStatus";

    #region Instance
    private static TimeManagerStatus instance;
    public static TimeManagerStatus GetInstance
    {
        get
        {
            if (instance == null)
            {
                var table = (TimeManagerStatus)Resources.Load(FILE_PATH);
                if (table == null) Debug.LogError("指定のパスにTimeManagerStatusが存在ません");
                else instance = table;
            }
            return instance;
        }
    }

    #endregion

    //メイン画面の時間
    public float mainWaitTime;      //ゲーム開始までの待機時間
    public float mainTimeLimit;     //ゲームの制限時間

    //フェード関連
    public float fadeInTime;
    public float fadeOuntTime;

}
