using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02Core : MonoBehaviour
{
    public GameObject deadEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var myHp = this.GetComponent<EnemyHitJudgment>().hp;

        if (myHp <= 0.0f)
        {
            SoundManager.GetInstance.PlaySE("CoreBreak");

            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 1.5f);
            Destroy(gameObject);
        }
    }
}
