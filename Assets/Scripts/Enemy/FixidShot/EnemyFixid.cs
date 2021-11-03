using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixid : MonoBehaviour
{
    [Header("固定砲台の敵ですわ(ﾟДﾟ )")]
    public bool shotStart;

    private bool chargeFlg;
    private float chargeTimer;

    [Header("初動時間")]
    [Range(0.0f, 3.0f)]
    public float startTime;
    private float startTimer;

    [Header("弾を撃つ予備動作")]
    [Header("--------1秒=60.0f--------")]
    [Range(0.0f, 3.0f)]
    public float chargeTime;

    private bool waitFlg;
    private float waitTimer;

    [Header("弾を撃った後の待機時間")]
    [Range(0.0f, 3.0f)]
    public float waitTime;

    // 攻撃を受けたときのスタン時間
    private bool stanFlg;
    private float stanTimer;
    [SerializeField]
    [Header("スタン時間")]
    [Range(0.0f, 3.0f)]
    private float stanTime;
    //--------------------------------------------------

    // Playerの弾に何発か当たったらスタン
    private int count;
    [SerializeField]
    [Header("弾に何発当たったらスタンさせる？^q^")]
    private int shotCount;
    //--------------------------------------------------
    private bool effectFlg;

    public GameObject chargeEffect;
    private GameObject player;
    public GameObject DeadEffet;
    private enum STATE
    {
        START,      //初動時間
        CHARGE,     //弾を撃つ予備動作
        SHOT,       //射精(*´Д`)
        WAIT        //待機
    }
    STATE state;
    //Start is called before the first frame update
    void Start()
    {
        startTimer = 0;
        state = STATE.START;
        chargeFlg = true;
        chargeTimer = 0;
        shotStart = false;
        waitFlg = false;
        waitTimer = 0;
        player = GameObject.FindWithTag("Player");
        effectFlg = true;
    }

    //Update is called once per frame
    void Update()
    {
        var myHp = this.GetComponent<EnemyHitJudgment>().hp;

        if (myHp <= 0.0f)
        {
            GameObject ef = Instantiate(DeadEffet, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.3f);
            Destroy(gameObject);
        }
        Stan();
        if (!stanFlg && TimeManager.GetInstance.timeScale >= 1.0f)
        {
            switch (state)
            {
                case STATE.START:
                    StartMove();
                    break;
                case STATE.CHARGE:
                    Charge();
                    break;
                case STATE.SHOT:
                    Shot();
                    break;
                case STATE.WAIT:
                    Wait();
                    break;
            }
        }
    }
    void Stan()
    {

        if (count >= shotCount)
        {
            stanFlg = true;
            count = 0;
        }
        if (stanFlg) stanTimer += Time.deltaTime;
        if (stanTimer >= stanTime)
        {
            stanFlg = false;
            stanTimer = 0;
        }
    }
    void StartMove()
    {
        startTimer += Time.deltaTime;
        if (startTimer >= startTime)
        {
            effectFlg = true;
            state = STATE.CHARGE;
        }
    }
    void Charge()
    {
        waitTimer = 0;

        //弾撃つ予備動作

        #region ロックオン
        var vec = (player.transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        #endregion
        chargeTimer += Time.deltaTime;
        if (effectFlg)
        {
            GameObject ef = Instantiate(chargeEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, chargeTime);
            effectFlg = false;
        }
        if (chargeTimer >= chargeTime)
        {
            shotStart = true;//射精
            state = STATE.SHOT;
        }

    }

    void Shot()
    {
        shotStart = false;//寸止め
        state = STATE.WAIT;
    }

    void Wait()
    {
        chargeTimer = 0;
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            effectFlg = true;
            state = STATE.CHARGE;
        }
    }

    //スタンフラグ用の当たり判定ですぅ・・・。
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ショット
        if (collision.tag == "Bullet" && !stanFlg)
        {
            count++;
        }
        //ハンマー
        if (collision.tag == "Hammer" && !stanFlg)
        {
            stanFlg = true;
        }
        ////ノックバック
        //if (collision.tag == "Player" && !stanFlg)
        //{
        //    transform.position = transform.position - transform.up * 2.0f;
        //    stanFlg = true;
        //}
    }
}
