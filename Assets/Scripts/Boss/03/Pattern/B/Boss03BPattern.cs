using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03BPattern : MonoBehaviour
{
    enum STATE
    {
        CIRCLE_SHOT,
        THREE_TACKLE_STRONG_BEFOR,
        THREE_TACKLE_STRONG_FIXID,
        THREE_TACKLE_STRONG,
        THREE_TACKLE_STRONG_NEXT,
        FOUR_DIR_SHOT,
        FOUR_DIR_SHOTX,
        FOUR_DIR_END,
        PATTERN_CHANGE
    }
    STATE state;
    private float shotSpanTimer;
    private float circleShotNextStrongTimer;
    public GameObject centerTarget;
    private float shotTimer;
    private float rotationSpeedSave;

    private float targetTimer;
    private float targetNextTimer;

    private bool wallHitFlg;
    private GameObject player;
    public GameObject tackleWall;
    private int tackleCount;
    private float backSpeed;
    private float nextTackleTimer;

    private float fourRotationSpeedSave;
    private bool shotEnd;
    private int shotCount;
    private float fourShotSpanTimer;
    private float fourShotTimer;

    private float patternAChangeTimer;

    [Header("回転速度")]
    [Header("--------円形ショット-------")]
    [Header("============Bパターン============")]
    public float rotationSpeed;
    [Header("撃つ間隔")]
    public float shotSpan;
    [System.NonSerialized]
    public bool circleShotFlg;
    [Header("真ん中に行く速度")]
    public float centerMoveSpeed;
    [Header("真ん中に到着してから弾撃つまでの時間")]
    public float shotTime;
    [Header("円形ショットから強タックル3回遷移時間")]
    public float circleShotNextStrongTime;

    [Header("--------強タックル3回-------")]
    [Header("Playerに狙いを定める時間")]
    public float targetTime;
    [Header("狙い固定後タックルするまでの時間")]
    public float targetNextTime;
    [Header("タックルの速度")]
    public float tackleSpeed;
    [Header("次タックルするまでの時間且つT~4Shot")]
    public float nextTackleTime;

    [System.NonSerialized]
    public bool shotFlg;
    [Header("--------4方向ショット2回-------")]
    [Header("撃つ間隔")]
    public float fourShotSpan;
    [Header("回転速度")]
    public float fourRotationSpeed;
    [Header("真ん中に行く速度")]
    public float foutCenterMoveSpeed;
    [Header("真ん中に到着してから弾撃つまでの時間")]
    public float fourShotTime;

    [Header("Aパターンに戻るまでの時間")]
    public float patternAChangeTime;

    // Start is called before the first frame update
    void Start()
    {
        //centerTarget = GameObject.Find("centerTarget");
        player = GameObject.FindWithTag("Player");

        tackleWall.active = false;

        state = STATE.CIRCLE_SHOT;
        circleShotFlg = false;
        centerTarget.active = false;
        shotSpanTimer = 0.0f;
        shotTimer = 0.0f;
        circleShotNextStrongTimer = 0.0f;

        targetTimer = 0.0f;
        targetNextTimer = 0.0f;
        wallHitFlg = false;
        tackleCount = 0;
        backSpeed = 1.0f;
        nextTackleTimer = 0.0f;

        shotEnd = false;
        shotFlg = false;
        shotCount = 0;
        fourShotTimer = 0.0f;
        patternAChangeTimer = 0.0f;

        rotationSpeedSave = rotationSpeed;
        fourRotationSpeedSave = fourRotationSpeed;
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

    //private float tt;
    //void test()
    //{
    //    if (Boss03Manager.GetPattern() == 1)
    //    {
    //        tt += Time.deltaTime;
    //    }

    //    if (tt >= 0.1f)
    //    {
    //        state = STATE.CIRCLE_SHOT;
    //    }
    //}

    void CircleShot()
    {
        
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
            state = STATE.THREE_TACKLE_STRONG_BEFOR;
        }
    }

    void ThreeStrongTackleBefor()
    {
        wallHitFlg = false;
        nextTackleTimer = 0.0f;

        Boss03Manager.SetMode(5);

        shotTimer = 0.0f;
        circleShotNextStrongTimer = 0.0f;
        circleShotFlg = false;
        shotSpanTimer = 0.0f;
        Boss03Manager.SetSpeed(tackleSpeed);

        TackleDir(player.transform);

        Boss03Manager.SetTackleLine(true);
        Boss03Manager.SetOverlap(true);
        targetTimer += Time.deltaTime;
        if (targetTimer >= targetTime)
        {
            state = STATE.THREE_TACKLE_STRONG_FIXID;
        }
    }
    void StrongTackleFixid()
    {
        targetTimer = 0.0f;
        targetNextTimer += Time.deltaTime;
        if (targetNextTimer >= targetNextTime)
        {
            state = STATE.THREE_TACKLE_STRONG;
        }
    }
    void StrongTackle()
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

            Boss03Manager.SetOverlap(true);
            tackleWall.active = false;
            //
            transform.position = transform.position - (transform.up * 1.0f);
            //
            //Tackle(-tackleSpeed);
            tackleCount++;
            wallHitFlg = false;
            state = STATE.THREE_TACKLE_STRONG_NEXT;
        }
    }
    void StrongTackleNext()
    {
        nextTackleTimer += Time.deltaTime;
        if (nextTackleTimer >= nextTackleTime)
        {
            state = STATE.THREE_TACKLE_STRONG_BEFOR;
        }
        if (tackleCount == 3)
        {
            Boss03Manager.SetCenterCol(false);
            centerTarget.active = false;
            state = STATE.FOUR_DIR_SHOT;
        }
    }

    void FourDirShot()
    {
        SoundManager.GetInstance.PlaySE("enemy_shot");
        //if (effectFlg)
        //{
        //    GameObject ef = Instantiate(shotChegeEffect, transform.position, Quaternion.identity) as GameObject;
        //    Destroy(ef, 1.0f);
        //    effectFlg = false;
        //}
        wallHitFlg = false;
        tackleCount = 0;
        Boss03Manager.SetOverlap(true);
        centerTarget.active = true;
        Boss03Manager.SetMode(7);
        transform.Rotate(0, 0, fourRotationSpeed * Time.deltaTime);

        if (!Boss03Manager.GetCenterCol()) TargetMove(centerTarget.transform, centerMoveSpeed);
        if (Boss03Manager.GetCenterCol()) fourShotTimer += Time.deltaTime;

        if (fourShotTimer >= fourShotTime)
        {
            fourShotSpanTimer += Time.deltaTime;
        }

        if (fourShotSpanTimer >= fourShotSpan && !shotFlg)
        {
            shotFlg = true;
            shotEnd = true;
            shotCount++;
        }

        if (shotEnd)
        {
            state = STATE.FOUR_DIR_SHOTX;
        }

        if (shotCount == 2)
        {
            state = STATE.FOUR_DIR_END;
        }
    }
    void ShotX()
    {
        SoundManager.GetInstance.PlaySE("enemy_shot");
        shotFlg = false;
        shotEnd = false;
        fourShotSpanTimer = 0.0f;
        state = STATE.FOUR_DIR_SHOT;
    }

    void FourDirShotEnd()
    {
        Boss03Manager.SetCenterCol(false);
        fourShotTimer = 0.0f;
        fourShotSpanTimer = 0.0f;
        shotCount = 0;
        shotFlg = false;

        fourRotationSpeed -= fourRotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, fourRotationSpeed * Time.deltaTime);
        if (fourRotationSpeed <= 0.0f)
        {
            fourRotationSpeed = 0.0f;
        }
        patternAChangeTimer += Time.deltaTime;

        if (patternAChangeTimer >= patternAChangeTime)
        {
            fourRotationSpeed = fourRotationSpeedSave;
            state = STATE.PATTERN_CHANGE;
        }
    }

    //Aパターンに戻る
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
        Debug.Log(tackleCount);

        //でばぐよう
        //Boss03Manager.SetPattern(1);
        if (TimeManager.GetInstance.timeScale >= 1.0f && Boss03Manager.GetPattern() == 1 && !Boss03Manager.GetDeadPerformance())
        {
            switch (state)
            {
                //case STATE.T:
                //    test();
                //    break;
                case STATE.CIRCLE_SHOT:
                    CircleShot();
                    break;
                case STATE.THREE_TACKLE_STRONG_BEFOR:
                    ThreeStrongTackleBefor();
                    break;
                case STATE.THREE_TACKLE_STRONG_FIXID:
                    StrongTackleFixid();
                    break;
                case STATE.THREE_TACKLE_STRONG:
                    StrongTackle();
                    break;
                case STATE.THREE_TACKLE_STRONG_NEXT:
                    StrongTackleNext();
                    break;
                case STATE.FOUR_DIR_SHOT:
                    FourDirShot();
                    break;
                case STATE.FOUR_DIR_SHOTX:
                    ShotX();
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
        if (collision.tag == "TackleWall")
        {
            SoundManager.GetInstance.PlaySE("boss_tacckleWall");
            wallHitFlg = true;
        }
    }

}
