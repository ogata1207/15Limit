using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageBase
{
    public GameObject stagePrefab;
}

[CreateAssetMenu(fileName = "StageStatus", menuName = "OGT/Status Table/StageStatus")]
public class StageStatus : ScriptableObject
{
    public static string FILE_PATH = "Status/StageStatus";

    #region Instance
    private static StageStatus instance;
    public static StageStatus GetInstance
    {
        get
        {
            if (instance == null)
            {
                var table = (StageStatus)Resources.Load(FILE_PATH);
                if (table == null) Debug.LogError("指定のパスにTimeManagerStatusが存在ません");
                else instance = table;
            }
            return instance;
        }
    }

    #endregion

    [SerializeField]
    public List<GameObject> stages;
}
