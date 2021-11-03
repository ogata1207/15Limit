using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChilde : MonoBehaviour
{

    //オブジェクト
    public GameObject Prefab;
    private GameObject Boss;

    //リスト
    private List<GameObject> FollowerList = new List<GameObject>();

    //Followerの設定
    private float Vel = 5.0f;       //速度
    public float Dist = 1.0f;       //距離
    private float DistSave;
    private int MaxFollowers = 4;   //子機の最大数
    private int NumFollowers = 4;    //現在の子機の数

    public GameObject[] deadBeforPos;

    public GameObject hummerHitEffect;
    public GameObject shotHiteffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Boss03Manager.GetDeadBefor())
        {
            //ショット
            if (collision.tag == "Bullet")
            {
                var damage = PlayerStatus.GetInstance.AttackDamage();
                FindObjectOfType<BossFinalHitJugement>().hp -= damage;
                //hp-=武器の攻撃力
                if (!Boss03Manager.GetDeadPerformance()) DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));

                if (FindObjectOfType<BossFinalHitJugement>().hp >= 1.0f || FindObjectOfType<BossFinalHitJugement>().hp > 0.1f)
                {
                    GameObject ef = Instantiate(shotHiteffect, transform.position, Quaternion.identity) as GameObject;
                    Destroy(ef, 0.5f);
                }
            }
            //ハンマー
            if (collision.tag == "Hammer")
            {
                SoundManager.GetInstance.PlaySE("Enemy_HammerDamage");

                var damage = PlayerStatus.GetInstance.AttackDamage();

                //hp-=武器の攻撃力
                FindObjectOfType<BossFinalHitJugement>().hp -= damage;

                if (!Boss03Manager.GetDeadPerformance()) DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));
                if (FindObjectOfType<BossFinalHitJugement>().hp >= 1.0f || FindObjectOfType<BossFinalHitJugement>().hp > 0.1f)
                {
                    GameObject ef = Instantiate(hummerHitEffect, transform.position, Quaternion.identity) as GameObject;
                    Destroy(ef, 0.5f);
                }
            }
        }
    }

    public void SetFollower(Vector3 pos)
    {
        FollowerList.Add(Instantiate(Prefab, pos, Prefab.transform.rotation) as GameObject);
        NumFollowers++;
    }

    public void SetFollower()
    {
        FollowerList.Add(Instantiate(Prefab, Prefab.transform.position, Prefab.transform.rotation) as GameObject);
        NumFollowers++;
    }


    public Vector3 GetFollowerPos(int a)
    {
        //引数の値が現在のフォロワーの数より小さかったらその番号の子機のposをかえす
        //それ以外は一番後ろの子機のposをかえしてさしあげる

        if (NumFollowers > a) return FollowerList[a].transform.position;
        else return FollowerList[NumFollowers].transform.position;
    }

    void Start()
    {
        DistSave = Dist;

        //初期のフォロワーの数が0じゃないときに最初に生成させる
        if (NumFollowers != 0)
        {
            for (int i = 0; i < MaxFollowers; i++)
            {
                SetFollower();
            }
        }

        Boss = GameObject.Find("BossFinal");

        //todo
        //子機の速度をプレイヤーの速度に合わせる
        Vel = Boss03Manager.GetSpeed() * 80.0f * Time.deltaTime;

    }

    // Update is called once per frame

    void Update()
    {
        if (!FollowerList[0])
        {
            Boss03Manager.SetChildeDead01(true);
        }
        if (!FollowerList[1])
        {
            Boss03Manager.SetChildeDead02(true);
        }
        if (!FollowerList[2])
        {
            Boss03Manager.SetChildeDead03(true);
        }
        if (!FollowerList[3])
        {
            Boss03Manager.SetChildeDead04(true);
        }

        if (FollowerList[0] == null&& FollowerList[1] == null&& FollowerList[2] == null&& FollowerList[3] == null)
        {
            Boss03Manager.SetChildeDead(true);
        }
        if (Boss03Manager.GetDeadBefor())
        {
            FollowerList[0].transform.position = deadBeforPos[0].transform.position;
            FollowerList[1].transform.position = deadBeforPos[1].transform.position;
            FollowerList[2].transform.position = deadBeforPos[2].transform.position;
            FollowerList[3].transform.position = deadBeforPos[3].transform.position;
        }
        //todo
        //今回は自機のスピードが変わる仕様なので
        //毎回スピードを再取得する
        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            if (!Boss03Manager.GetDeadBefor())
            {

                Vel = Boss03Manager.GetSpeed() * 80.0f * Time.deltaTime;
                for (int i = 0; i < NumFollowers; i++)
                {
                    if (!Boss03Manager.GetOverlap())
                    {
                        Dist = DistSave;
                    }

                    if (Boss03Manager.GetOverlap())
                    {
                        Dist -= 0.1f;
                    }

                    if (Dist <= 0.5f)
                    {
                        FollowerList[i].transform.position = Boss.transform.position;
                        Dist = 0.5f;
                    }

                    //円形ショットの時//
                    if (Boss03Manager.GetMode() == 8)
                    {
                        FollowerList[i].transform.position = Boss.transform.position;
                    }
                    //8ショットの時//
                    if (Boss03Manager.GetMode() == 9)
                    {
                        FollowerList[i].transform.position = Boss.transform.position;
                    }

                    Vector2 dir;
                    if (i == 0)
                        dir = -1 * FollowerList[i].transform.position + Boss.transform.position;
                    else
                        dir = -1 * FollowerList[i].transform.position + FollowerList[i - 1].transform.position;

                    if (dir.magnitude > Dist)

                        FollowerList[i].GetComponent<Rigidbody2D>().velocity = dir.normalized * Vel;
                    else
                        FollowerList[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

                }
            }

        }

    }
}
