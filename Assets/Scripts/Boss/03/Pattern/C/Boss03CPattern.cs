using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03CPattern : MonoBehaviour
{
    enum STATE
    {
        CIRCLE_SHOT,
        ONE_TACKLE_STRONG_BEFOR,
        ONE_TACKLE_STRONG_FIXID,
        ONE_TACKLE_STRONG,
        ONE_TACKLE_STRONG_NEXT,
        THREE_TACKLE_BEFOR,
        THREE_TACKLE,
        FOUR_DIR_SHOT,
        FOUR_DIR_END,
        PATTERN_CHANGE
    }
    STATE state;

    //円形ショット
    private float shotSpanTimer;
    private float circleShotNextStrongTimer;
    public GameObject centerTarget;
    private float shotTimer;
    private float rotationSpeedSave;

    //強タックル1回
    private float targetTimer;
    private float targetNextTimer;
    private bool wallHitFlg;
    private GameObject player;
    public GameObject tackleWall;
    private float backSpeed;
    private float nextTackleTimer;

    //タックル3回
    private float threeTackleBeforTimer;
    private float tackleNextFourShotTimer;
    private int tackleCount;
    private bool tackleNow;
    private bool threeTackleF;

    //4方向ショット
    private float fourshotTimer;
    private bool effectFlg;
    private float rotationSave;

    private float patternAChangeTimer;


    [Header("回転速度")]
    [Header("--------円形ショット-------")]
    [Header("============Cパターン============")]
    public float rotationSpeed;
    [Header("撃つ間隔")]
    public float shotSpan;
    [System.NonSerialized]
    public bool circleShotFlg;
    [Header("真ん中に行く速度")]
    public float centerMoveSpeed;
    [Header("真ん中に到着してから弾撃つまでの時間")]
    public float shotTime;
    [Header("円形ショットから強タックル1回遷移時間")]
    public float circleShotNextStrongTime;

    [Header("Playerに狙いを定める時間")]
    [Header("--------強タックル1回-------")]
    public float targetTime;
    [Header("狙い固定後タックルするまでの時間")]
    public float targetNextTime;
    [Header("タックルの速度")]
    public float tackleSpeed;
    [Header("強タックル1回からタックル3回遷移時間")]
    public float nextTackleTime;

    public Transform[] tacklePosObj;
    [Header("0:左上 1:右上 2:右下 3:左下")]
    [Header("------タックル3回------")]
    public int[] tacklePos03;
    [Header("タックル3回速度")]
    public float threeTackleSpeed;
    [Header("タックル(3回)する前の警告線表示時間")]
    public float threeTackleBeforTime;
    [Header("タックル3回から4方向ショット1回遷移時間")]
    public float tackleNextFourShotTime;

    [Header("真ん中に移動する速度")]
    [Header("------4方向ショット1回------")]
    public float fourCenterMoveSpeed;
    [Header("回転する速度")]
    public float fourRotationSpeed;
    [System.NonSerialized]
    public bool shotFlg;
    private bool shotEndFlg;
    [Header("真ん中に到着してから弾撃つまでの時間")]
    public float fourShotTime;

    [Header("Aパターンに戻るまでの時間")]
    public float patternAChangeTime;
    // Start is called before the first frame update
    void Start()
    {
        state = STATE.CIRCLE_SHOT;

        //centerTarget = GameObject.Find("centerTarget");
        player = GameObject.FindWithTag("Player");
        tackleWall.active = false;


        circleShotFlg = false;
        centerTarget.active = false;
        shotSpanTimer = 0.0f;
        shotTimer = 0.0f;
        circleShotNextStrongTimer = 0.0f;

        targetTimer = 0.0f;
        targetNextTimer = 0.0f;
        nextTackleTimer = 0.0f;
        wallHitFlg = false;
        backSpeed = 1.0f;

        rotationSpeedSave = rotationSpeed;

        threeTackleBeforTimer = 0.0f;
        tackleNextFourShotTimer = 0.0f;
        tackleCount = 0;
        tackleNow = false;
        threeTackleF = false;

        fourshotTimer = 0.0f;
        rotationSave = fourRotationSpeed;

        patternAChangeTimer = 0.0f;
    }
    void TackleDir(Transform target)
    {
        var vec = (target.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    void TargetMove(Transform target, float speed)
    {
        var vec = target.position - transform.position;
        vec = vec.normalized;
        transform.position += new Vector3(vec.x * speed, vec.y * speed) * Time.deltaTime;
    }

    void Tackle(float speed)
    {
        Vector3 velocity = transform.rotation * new Vector3(0, speed, 0);
        transform.position += velocity * Time.deltaTime;
    }

    void CircleShot()
    {
        wallHitFlg = false;
        //FindObjectOfType<Boss03BPattern>().circleShotFlg = false;
        centerTarget.active = true;
        Boss03Manager.SetMode(8);
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (!Boss03Manager.GetCenterCol()) TargetMove(centerTarget.transform, centerMoveSpeed);

        if (Boss03Manager.GetCenterCol()) shotTimer += Time.deltaTime;
        if (shotTimer >= shotTime)
        {
            shotSpanTimer += Time.deltaTime;
        }
        if (shotSpanTimer >= shotSpan && !circleShotFlg)
        {
            circleShotFlg = true;
            shotSpanTimer = 0.0f;
        }

        if (Boss03Manager.GetCenterCol()) circleShotNextStrongTimer += Time.deltaTime;
        if (circleShotNextStrongTimer >= circleShotNextStrongTime)
        {
            Boss03Manager.SetCenterCol(false);
            circleShotFlg = false;
            state = STATE.ONE_TACKLE_STRONG_BEFOR;
        }
    }

    void OneTackleStrongBefor()
    {
        Boss03Manager.SetSpeed(tackleSpeed);
        Boss03Manager.SetMode(4);

        circleShotFlg = false;
        shotTimer = 0.0f;
        shotSpanTimer = 0.0f;
        circleShotNextStrongTimer = 0.0f;

        Boss03Manager.SetSpeed(tackleSpeed);

        TackleDir(player.transform);
        Boss03Manager.SetTackleLine(true);
        Boss03Manager.SetOverlap(true);

        targetTimer += Time.deltaTime;
        if (targetTimer >= targetTime)
        {
            state = STATE.ONE_TACKLE_STRONG_FIXID;
        }
    }

    void OneTackleStrongFixid()
    {
        wallHitFlg = false;
        targetTimer = 0.0f;
        targetNextTimer += Time.deltaTime;
        if (targetNextTimer >= targetNextTime)
        {
            state = STATE.ONE_TACKLE_STRONG;
        }
    }

    void OneTackleStrong()
    {
        tackleWall.active = true;
        Boss03Manager.SetTackleLine(false);
        Boss03Manager.SetOverlap(false);

        targetNextTimer = 0.0f;
        if (!wallHitFlg)
        {
            Tackle(tackleSpeed);
            SoundManager.GetInstance.PlaySE("boss_tacckle");
        }
        if (wallHitFlg)
        {
            var shakeObj = Camera.main.GetComponent<ShakeObject>();
            shakeObj.PlayShake(0.5f, 0.1f);
            tackleWall.active = false;
            transform.position = transform.position - (transform.up * 1.0f);
            wallHitFlg = false;
            state = STATE.ONE_TACKLE_STRONG_NEXT;
        }
    }

    void OneTackleStrongNext()
    {
        Boss03Manager.SetOverlap(true);
        nextTackleTimer += Time.deltaTime;
        if (nextTackleTimer >= nextTackleTime)
        {
            state = STATE.THREE_TACKLE_BEFOR;
        }
    }

    void ThreeTackleBefor()
    {
        Boss03Manager.SetTackleLine(true);
        Boss03Manager.SetOverlap(true);

        Boss03Manager.SetTacklePos(0);
        Boss03Manager.SetMode(3);

        if (Boss03Manager.GetMode() == 3)
        {
            threeTackleBeforTimer += Time.deltaTime;
        }

        if (tackleCount == 0)
        {
            if (tacklePos03[0] == 0)
            {
                TackleDir(tacklePosObj[0]);
            }
            if (tacklePos03[0] == 1)
            {
                TackleDir(tacklePosObj[1]);
            }
            if (tacklePos03[0] == 2)
            {
                TackleDir(tacklePosObj[2]);
            }
            if (tacklePos03[0] == 3)
            {
                TackleDir(tacklePosObj[3]);
            }
        }
        if (tackleCount == 1)
        {
            if (tacklePos03[1] == 0)
            {
                TackleDir(tacklePosObj[0]);
            }
            if (tacklePos03[1] == 1)
            {
                TackleDir(tacklePosObj[1]);
            }
            if (tacklePos03[1] == 2)
            {
                TackleDir(tacklePosObj[2]);
            }
            if (tacklePos03[1] == 3)
            {
                TackleDir(tacklePosObj[3]);
            }
        }
        if (tackleCount == 3)
        {
            if (tacklePos03[2] == 0)
            {
                TackleDir(tacklePosObj[0]);
            }
            if (tacklePos03[2] == 1)
            {
                TackleDir(tacklePosObj[1]);
            }
            if (tacklePos03[2] == 2)
            {
                TackleDir(tacklePosObj[2]);
            }
            if (tacklePos03[2] == 3)
            {
                TackleDir(tacklePosObj[3]);
            }
        }


        if (threeTackleBeforTimer >= threeTackleBeforTime)
        {
            state = STATE.THREE_TACKLE;
            if (tackleCount == 1)
            {
                tackleCount++;
                state = STATE.THREE_TACKLE;
            }
            if (tackleCount == 3)
            {
                tackleCount++;
                state = STATE.THREE_TACKLE;
            }
        }
    }

    void ThreeTackle()
    {
        Boss03Manager.SetSpeed(threeTackleSpeed);
        Boss03Manager.SetTackleLine(false);
        Boss03Manager.SetOverlap(false);
        threeTackleBeforTimer = 0.0f;
        #region タックル1回目
        if (tackleCount == 0)
        {
            if (tacklePos03[0] == 0)
            {
                if (!tackleNow) TargetMove(tacklePosObj[0], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 1 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos03[0] == 1)
            {
                if (!tackleNow) TargetMove(tacklePosObj[1], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 2 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos03[0] == 2)
            {
                if (!tackleNow) TargetMove(tacklePosObj[2], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 3 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos03[0] == 3)
            {
                if (!tackleNow) TargetMove(tacklePosObj[3], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 4 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
        }
        #endregion
        if (tackleCount == 1)
        {
            tackleNow = false;
            state = STATE.THREE_TACKLE_BEFOR;
        }
        #region タックル2回目
        if (tackleCount == 2)
        {
            if (tacklePos03[1] == 0)
            {
                if (!tackleNow) TargetMove(tacklePosObj[0], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 1 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos03[1] == 1)
            {
                if (!tackleNow) TargetMove(tacklePosObj[1], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 2 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos03[1] == 2)
            {
                if (!tackleNow) TargetMove(tacklePosObj[2], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 3 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos03[1] == 3)
            {
                if (!tackleNow) TargetMove(tacklePosObj[3], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 4 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
        }
        #endregion
        if (tackleCount == 3)
        {
            tackleNow = false;
            state = STATE.THREE_TACKLE_BEFOR;
        }
        #region タックル3回目
        if (tackleCount == 4)
        {
            if (tacklePos03[2] == 0)
            {
                if (!threeTackleF) TargetMove(tacklePosObj[0], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 1 && !threeTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    threeTackleF = true;
                    transform.position = tacklePosObj[0].position;
                }
            }
            if (tacklePos03[2] == 1)
            {
                if (!threeTackleF) TargetMove(tacklePosObj[1], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 2 && !threeTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    threeTackleF = true;
                    transform.position = tacklePosObj[1].position;
                }
            }
            if (tacklePos03[2] == 2)
            {
                if (!threeTackleF) TargetMove(tacklePosObj[2], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 3 && !threeTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    threeTackleF = true;
                    transform.position = tacklePosObj[2].position;
                }
            }
            if (tacklePos03[2] == 3)
            {
                if (!threeTackleF) TargetMove(tacklePosObj[3], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 4 && !threeTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    threeTackleF = true;
                    transform.position = tacklePosObj[3].position;
                }
            }
        }
        #endregion
        if (threeTackleF)
        {
            Boss03Manager.SetOverlap(true);
            tackleNextFourShotTimer += Time.deltaTime;
        }
        if (tackleNextFourShotTimer >= tackleNextFourShotTime)
        {
            //effectFlg = true;
            Boss03Manager.SetCenterCol(false);
            state = STATE.FOUR_DIR_SHOT;
        }
    }

    void FourDirShot()
    {

        Boss03Manager.SetTacklePos(0);
        tackleCount = 0;
        tackleNow = false;
        threeTackleF = false;
        tackleNextFourShotTimer = 0.0f;
        //if (effectFlg)
        //{
        //    GameObject ef = Instantiate(shotChegeEffect, transform.position, Quaternion.identity) as GameObject;
        //    Destroy(ef, 1.0f);
        //    effectFlg = false;
        //}
        //Boss03Manager.SetOverlap(true);
        centerTarget.active = true;
        Boss03Manager.SetMode(6);
        transform.Rotate(0, 0, fourRotationSpeed * Time.deltaTime);
        if (!Boss03Manager.GetCenterCol()) TargetMove(centerTarget.transform, centerMoveSpeed);
        if (Boss03Manager.GetCenterCol()) fourshotTimer += Time.deltaTime;
        if (fourshotTimer >= fourShotTime)
        {
            shotFlg = true;
            shotEndFlg = true;
        }

        if (shotEndFlg)
        {
            state = STATE.FOUR_DIR_END;
        }
    }

    void FourDirShotEnd()
    {
        Boss03Manager.SetCenterCol(false);
        fourshotTimer = 0.0f;

        shotFlg = false;
        shotEndFlg = false;

        fourRotationSpeed -= fourRotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, fourRotationSpeed * Time.deltaTime);
        if (fourRotationSpeed <= 0.0f)
        {
            fourRotationSpeed = 0.0f;
        }

        patternAChangeTimer += Time.deltaTime;
        if (patternAChangeTimer >= patternAChangeTime)
        {
            fourRotationSpeed = rotationSave;
            state = STATE.PATTERN_CHANGE;
        }
    }
    void PatternReturn()
    {
        Boss03Manager.SetPattern(0);
        tackleWall.active = false;
        patternAChangeTimer = 0.0f;
        state = STATE.CIRCLE_SHOT;
        Boss03Manager.SetCenterCol(false);
    }
    // Update is called once per frame
    void Update()
    {
        //Boss03Manager.SetPattern(2);
        //Debug.Log(Boss03Manager.GetTackleLine());
        if (TimeManager.GetInstance.timeScale >= 1.0f && Boss03Manager.GetPattern() == 2 &&! Boss03Manager.GetDeadPerformance())
        {
            switch (state)
            {
                case STATE.CIRCLE_SHOT:
                    CircleShot();
                    break;

                case STATE.ONE_TACKLE_STRONG_BEFOR:
                    OneTackleStrongBefor();
                    break;

                case STATE.ONE_TACKLE_STRONG_FIXID:
                    OneTackleStrongFixid();
                    break;

                case STATE.ONE_TACKLE_STRONG:
                    OneTackleStrong();
                    break;

                case STATE.ONE_TACKLE_STRONG_NEXT:
                    OneTackleStrongNext();
                    break;

                case STATE.THREE_TACKLE_BEFOR:
                    ThreeTackleBefor();
                    break;

                case STATE.THREE_TACKLE:
                    ThreeTackle();
                    break;

                case STATE.FOUR_DIR_SHOT:
                    FourDirShot();
                    break;

                case STATE.FOUR_DIR_END:
                    FourDirShotEnd();
                    break;

                case STATE.PATTERN_CHANGE:
                    PatternReturn();
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LeftUP")
        {
            Boss03Manager.SetTacklePos(1);
        }
        if (collision.tag == "RightUp")
        {
            Boss03Manager.SetTacklePos(2);
        }
        if (collision.tag == "RightDown")
        {
            Boss03Manager.SetTacklePos(3);
        }
        if (collision.tag == "LeftDown")
        {
            Boss03Manager.SetTacklePos(4);
        }

        if (collision.tag == "TackleWall")
        {
            SoundManager.GetInstance.PlaySE("boss_tacckleWall");
            wallHitFlg = true;
        }
    }
}
