using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Shots : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 30.0f)]
    private float speed;
    private Vector3 rotation;
    private float angle;

    [Header("初動時間")]
    [Range(0f, 3f)]
    public float startTime;
    private float startTimer;

    //※どんだけ移動するのか
    private float moveAmountTimer;
    [SerializeField]
    [Header("移動量")]
    [Header("--------1秒=60.0f--------")]
    [Range(0f, 3f)]
    private float moveAmountTime;
    //--------------------------------------------------

    //移動後に止まる
    private float moveStopTimer;
    [SerializeField]
    [Header("移動後の待機")]
    [Range(0f, 3f)]
    private float moveStopTime;
    //--------------------------------------------------
    // 狙い定める時間
    private float lockonTimer;
    [SerializeField]
    [Header("狙い定める時間")]
    [Range(0f, 3f)]
    private float lockonTime;
    //--------------------------------------------------
    // 弾を撃つ予備動作時間
    private float chargeTimer;
    [SerializeField]
    [Header("弾を撃つ予備動作時間")]
    [Range(0f, 3f)]
    private float chargeTime;
    //--------------------------------------------------

    // 1発目から2発目の時間
    private float oneShotFromTwoShotTimer;
    [SerializeField]
    [Header("1発目から2発目の時間")]
    [Range(0f, 3f)]
    private float oneShotFromTwoShotTime;
    //--------------------------------------------------
    // 2発目から3発目の時間
    private float twoShotFromThreeShotTimer;
    [SerializeField]
    [Header("2発目から3発目の時間")]
    [Range(0f, 3f)]
    private float twoShotFromThreeShotTime;
    //--------------------------------------------------

    // 弾撃つ
    public bool oneShotStart;
    public bool twoShotStart;
    public bool threeShotStart;

    // 弾を撃った後の待機時間
    private float waitTimer;
    [SerializeField]
    [Header("弾を撃った後の待機時間")]
    [Range(0f, 3f)]
    private float waitTime;
    //--------------------------------------------------

    // 攻撃を受けたときのスタン時間
    private bool stanFlg;
    private float stanTimer;
    [SerializeField]
    [Header("スタン時間")]
    [Range(0f, 3f)]
    private float stanTime;
    //--------------------------------------------------

    // Playerの弾に何発か当たったらスタン
    private int count;
    [SerializeField]
    [Header("弾に何発当たったらスタンさせる？^q^")]
    private int shotCount;
    //--------------------------------------------------

    private float fProbabilityRate;
    private float search;

    private GameObject player;
    public GameObject chargeEffect;

    private bool effectFlg;
    public GameObject DeadEffet;
    private enum STATE
    {
        START,      //初動時間
        MOVE,       //移動
        MOVE_STOP,  //移動後の待機
        LOCKON,     //狙う
        CHARGE,     //弾を撃つ予備動作
        ONESHOT,    //1射精(*´Д`)
        TWOSHOT,    //2射精(*´Д`)
        THREESHOT,  //3射精(*´Д`)
        WAIT        //待機
    }
    STATE state;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        state = STATE.START;

        startTimer = 0.0f;
        moveAmountTimer = 0.0f;
        moveStopTimer = 0.0f;
        lockonTimer = 0.0f;
        chargeTimer = 0.0f;
        waitTimer = 0.0f;
        oneShotFromTwoShotTimer = 0.0f;
        twoShotFromThreeShotTimer = 0.0f;

        oneShotStart = false;
        twoShotStart = false;
        threeShotStart = false;
        stanFlg = false;
        effectFlg = true;

        angle = 0.0f;
        fProbabilityRate = 0.0f;
        search = 0.0f;
        stanTimer = 0.0f;
        count = 0;

        rotation = transform.eulerAngles;
    }

    // Update is called once per frame
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
                case STATE.MOVE:
                    Move();
                    break;
                case STATE.MOVE_STOP:
                    MoveStop();
                    break;
                case STATE.LOCKON:
                    LockOn();
                    break;
                case STATE.CHARGE:
                    Charge();
                    break;
                case STATE.ONESHOT:
                    OneShot();
                    break;
                case STATE.TWOSHOT:
                    TwoShot();
                    break;
                case STATE.THREESHOT:
                    ThreeShot();
                    break;
                case STATE.WAIT:
                    Wait();
                    break;
            }
        }
    }

    void Stan()
    {
        //if (Input.GetKeyDown(KeyCode.Q)) stanFlg = true;
        //if (Input.GetKeyDown(KeyCode.W)) count++;
        if (count >= shotCount)
        {
            stanFlg = true;
            count = 0;
        }
        if (stanFlg) stanTimer+= Time.deltaTime;
        if (stanTimer >= stanTime)
        {
            stanFlg = false;
            stanTimer = 0.0f;
        }
    }
    float GetAngle(Vector2 me, Vector2 target)
    {
        Vector2 dt = target - me;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;

        if (degree <= 0)
        {
            degree += 360;
        }

        return degree;
    }
    float Percent()
    {
        return fProbabilityRate = Random.value * 100.0f;
    }
    void StartMove()
    {
        startTimer += Time.deltaTime;
        if (startTimer >= startTime)
        {
            state = STATE.MOVE;
        }
    }

    void Dir()
    {
        search += Time.deltaTime;
        var player = GameObject.FindWithTag("Player");
        angle = GetAngle(transform.position, player.transform.position);

        #region Playerの位置を探してる
        if (search >= moveAmountTime)
        {
            if (angle >= 0.0f && angle <= 60.0f)
            {
                //50%
                if (Percent() >= 50.0f) rotation.z = 0.0f;
                else rotation.z = -60.0f;
            }

            if (angle >= 30.0f && angle <= 90.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -30.0f;
                else rotation.z = -90.0f;
            }

            if (angle >= 60.0f && angle <= 120.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -60.0f;
                else rotation.z = -120.0f;
            }

            if (angle >= 90.0f && angle <= 150.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -90.0f;
                else rotation.z = -150.0f;
            }

            if (angle >= 120.0f && angle <= 180.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -120.0f;
                else rotation.z = -180.0f;
            }

            if (angle >= 150.0f && angle <= 210.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -150.0f;
                else rotation.z = -210.0f;
            }

            if (angle >= 180.0f && angle <= 240.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -180.0f;
                else rotation.z = -240.0f;
            }

            if (angle >= 210.0f && angle <= 270.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -210.0f;
                else rotation.z = -270.0f;
            }

            if (angle >= 240.0f && angle <= 300.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -240.0f;
                else rotation.z = -300.0f;
            }

            if (angle >= 270.0f && angle <= 330.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -270.0f;
                else rotation.z = -330.0f;
            }

            if (angle >= 300.0f && angle <= 359.0f)
            {
                if (Percent() >= 50.0f) rotation.z = -300.0f;
                else rotation.z = -359.0f;
            }
        }
        #endregion
        transform.eulerAngles = rotation;
    }

    void Move()
    {
        Dir();
        waitTimer = 0.0f;
        moveAmountTimer += Time.deltaTime;

        Vector3 velocity = transform.rotation * new Vector3(0, speed, 0);
        transform.position += velocity * Time.deltaTime;

        if (moveAmountTimer >= moveAmountTime)
        {
            state = STATE.MOVE_STOP;
        }
    }

    void MoveStop()
    {
        search = 0.0f;
        moveAmountTimer = 0.0f;
        moveStopTimer += Time.deltaTime;
        if (moveStopTimer >= moveStopTime)
        {
            effectFlg = true;
            state = STATE.LOCKON;
        }
    }
    void LockOn()
    {
        moveStopTimer = 0;
        #region ロックオン
        var vec = (player.transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        #endregion
        lockonTimer += Time.deltaTime;
        if (lockonTimer >= lockonTime)
        {
            state = STATE.CHARGE;
        }
    }
    //チャージ
    void Charge()
    {
        lockonTimer = 0.0f;
        chargeTimer += Time.deltaTime;
        if (effectFlg)
        {
            GameObject ef = Instantiate(chargeEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, chargeTime);
            effectFlg = false;
        }
        if (chargeTimer >= chargeTime)
        {
            oneShotStart = true;//射精
            state = STATE.ONESHOT;
        }
    }

    //(*´Д`)ｳｯ!
    void OneShot()
    {
        chargeTimer = 0.0f;
        oneShotStart = false;//寸止め
        oneShotFromTwoShotTimer += Time.deltaTime;
        if (oneShotFromTwoShotTimer >= oneShotFromTwoShotTime)
        {
            twoShotStart = true;
            state = STATE.TWOSHOT;
        }
    }

    //(*´Д`)ｳｯ!
    void TwoShot()
    {
        oneShotFromTwoShotTimer = 0.0f;
        twoShotStart = false;
        twoShotFromThreeShotTimer += Time.deltaTime;
        if (twoShotFromThreeShotTimer >= twoShotFromThreeShotTime)
        {
            threeShotStart = true;
            state = STATE.THREESHOT;
        }
    }

    //(*´Д`)ｳｯ!
    void ThreeShot()
    {
        twoShotFromThreeShotTimer = 0.0f;
        threeShotStart = false;
        state = STATE.WAIT;
    }

    //待機時間
    void Wait()
    { 
        chargeTimer = 0.0f;
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            state = STATE.MOVE;
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
        if (collision.tag == "Hammer"&&!stanFlg)
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
