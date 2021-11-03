using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEmitter : OGT_Utility.Singleton<EnemyEmitter>
{
    public List<GameObject> stageEnemyGroup;
    public GameObject currentEnemyGroup;

    private bool spawnEnabled;
    public bool SpawnEnabled
    {
        set { spawnEnabled = value; }
        get { return spawnEnabled; }
    }

    /// <summary>
    /// 敵が全滅すればtrue
    /// </summary>
    public bool isFinish
    {
        get
        {

            foreach (Transform child in currentEnemyGroup.transform)
            {
                if (child.tag == "Enemy")
                    return false;
            }
            Debug.Log("敵全滅");
            return true;
            //OLD
            //return (spawnEnabled == false && currentEnemyGroup.transform.childCount > 0 )? false : true;
        }
    }

    void Start()
    {
        spawnEnabled = true;
        currentEnemyGroup = new GameObject();
        stageEnemyGroup = StageStatus.GetInstance.stages;
        TakeOverToTheNextScene();   
    }


    
    public void SpawnEnemy(int number)
    {
        //if (spawnEnabled)
        {
            //生成
            currentEnemyGroup = Instantiate(stageEnemyGroup[number]);
            spawnEnabled = false;
        }
    }

    public bool CheckNextStage(int number)
    {
        return ( stageEnemyGroup.Count - 1  >= number ) ? true : false;
    }

}
