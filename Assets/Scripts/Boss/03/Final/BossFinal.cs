using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinal : MonoBehaviour
{
    enum STATE
    {
        START,
        TACKLE_TARGET,
        TACKLE_BEFOR,
        TACKLE,
        EIGHT_DIR_SHOT,
        EIGHT_DIR_SHOT_END,
        CIECLE_SHOT,
        TACKLE_SHOT_TARGET,
        TACKLE_SHOT_BEFOR,
        TACKLE_SHOT,
        TACKLE_SHOT_END
    }
    STATE state;
    //初動
    private float startTimer;


    //タックル
    private int oneTacklePos;
    private int tackleRand;
    private float oneTackleBeforTimer;
    public Transform[] tacklePos;
    private bool tackleNow;
    private float tackleNextTimer;


    //8方向ショット
    public GameObject centerTarget;
    private float shotTimer;
    private bool effectFlg;
    //public GameObject shotChegeEffect;
    private float rotationSave;
    private float eightShotTimeNextCirclrTimer;

    //円形ショット
    private float shotSpanTimer;
    private float circleShotNextTimer;
    private float cirShotTimer;
    private float rotationSpeedSave;

    //タックルショット
    private float shotTackleBeforTimer;
    private bool shotTackleNow;
    private float shotTackleNextTimer;

    private float breakTimer;
    private bool shakeFlg;

    [Header("============2形態============")]
    public float tackleSpeed;
    [Header("初動時間")]
    public float startTime;
    [Header("タックル(1回)する前の警告線表示時間")]
    public float oneTackleBeforTime;
    [Header("タックル(1回)後8方向ショット遷移時間")]
    public float tackleNextTime;

    [Header("真ん中に移動する速度")]
    [Header("------8方向ショット1回------")]
    public float centerMoveSpeed;
    [Header("回転する速度")]
    public float rotationSpeed;
    [System.NonSerialized]
    public bool shotFlg;
    private bool shotEndFlg;
    [Header("真ん中に到着してから弾撃つまでの時間")]
    public float shotTime;
    [Header("8方向ショット1回から円形ショット撃つまでの時間")]
    public float eightShotTimeNextCirclrTime;

    [Header("回転速度")]
    [Header("--------円形ショット-------")]
    public float cirRotationSpeed;
    [Header("撃つ間隔")]
    public float shotSpan;
    [System.NonSerialized]
    public bool circleShotFlg;
    [Header("弾撃つまでの時間")]
    public float cirShotTime;
    [Header("円形ショットからタックルショット遷移時間")]
    public float circleShotNextTime;

    [Header("ショットタックルする前の警告線表示時間")]
    [Header("------ショットタックル------")]
    public float shotTackleBeforTime;
    [Header("ショットタックル後タックル(1回)遷移時間")]
    public float shotTackleNextTime;
    [System.NonSerialized]
    public bool oneShotFlg;
    private int wait;

    [Header("本体爆破するまでの時間")]
    public float breakTime;
    public GameObject breakEffect;
    // Start is called before the first frame update
    void Start()
    {
        Boss03Manager.Start();
        Boss03Manager.SetForm(2);
        state = STATE.START;
        startTimer = 0.0f;

        oneTackleBeforTimer = 0.0f;
        tackleNow = false;
        tackleNextTimer = 0.0f;

        centerTarget.active = false;
        rotationSave = rotationSpeed;
        shotTimer = 0.0f;
        eightShotTimeNextCirclrTimer = 0.0f;

        shotSpanTimer = 0.0f;
        circleShotNextTimer = 0.0f;
        cirShotTimer = 0.0f;
        rotationSpeedSave = cirRotationSpeed;

        shakeFlg = false;
        wait = 0;
        breakTimer = 0.0f;
    }

    void StartMove()
    {
        startTimer += Time.deltaTime;
        if (startTimer >= startTime)
        {
            state = STATE.TACKLE_TARGET;
        }
    }
    void TackleDir(Transform target)
    {
        var vec = (target.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
    void TackleMove(Transform target, float speed)
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

    void TackleTarget()
    {
        shotTackleNextTimer = 0.0f;
        tackleRand = Random.Range(0, 4);
        state = STATE.TACKLE_BEFOR;
    }

    void TackleBefor()
    {
        Boss03Manager.SetTackleLine(true);
        Boss03Manager.SetOverlap(true);
        oneTackleBeforTimer += Time.deltaTime;

        if (tackleRand == 0)
        {
            TackleDir(tacklePos[0]);
        }
        if (tackleRand == 1)
        {
            TackleDir(tacklePos[1]);
        }
        if (tackleRand == 2)
        {
            TackleDir(tacklePos[2]);
        }
        if (tackleRand == 3)
        {
            TackleDir(tacklePos[3]);
        }

        if (oneTackleBeforTimer >= oneTackleBeforTime)
        {
            state = STATE.TACKLE;
        }
    }

    void Tackle()
    {
        Boss03Manager.SetSpeed(tackleSpeed);

        Boss03Manager.SetTackleLine(false);
        Boss03Manager.SetOverlap(false);
        //tackleRand = 0;
        oneTackleBeforTimer = 0.0f;
        if (tackleRand == 0)
        {
            if (!tackleNow) TackleMove(tacklePos[0], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 1 && !tackleNow)
            {
                tackleNow = true;
            }
        }
        if (tackleRand == 1)
        {
            if (!tackleNow) TackleMove(tacklePos[1], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 2 && !tackleNow)
            {
                tackleNow = true;
            }
        }
        if (tackleRand == 2)
        {
            if (!tackleNow) TackleMove(tacklePos[2], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 3 && !tackleNow)
            {
                tackleNow = true;
            }
        }
        if (tackleRand == 3)
        {
            if (!tackleNow) TackleMove(tacklePos[3], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 4 && !tackleNow)
            {
                tackleNow = true;
            }
        }

        if (tackleNow)
        {
            Boss03Manager.SetOverlap(true);
            tackleNextTimer += Time.deltaTime;
        }
        if (tackleNextTimer >= tackleNextTime)
        {
            //effectFlg = true;
            Boss03Manager.SetCenterCol(false);
            state = STATE.EIGHT_DIR_SHOT;
        }
    }
    void EightDirShot()
    {
        tackleNow = false;
        Boss03Manager.SetTacklePos(0);
        tackleNextTimer = 0.0f;

        centerTarget.active = true;
        Boss03Manager.SetMode(9);
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (!Boss03Manager.GetCenterCol()) TackleMove(centerTarget.transform, centerMoveSpeed);
        if (Boss03Manager.GetCenterCol()) shotTimer += Time.deltaTime;

        if (shotTimer >= shotTime)
        {
            shotFlg = true;
            shotEndFlg = true;
        }

        if (shotEndFlg)
        {
            state = STATE.EIGHT_DIR_SHOT_END;
        }

    }

    void EightDirShotEnd()
    {
        //Boss03Manager.SetCenterCol(false);
        shotTimer = 0.0f;
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        shotFlg = false;
        shotEndFlg = false;
        rotationSpeed -= rotationSpeed * Time.deltaTime;
        if (rotationSpeed <= 0.0f) rotationSpeed = 0.0f;

        eightShotTimeNextCirclrTimer += Time.deltaTime;
        if (eightShotTimeNextCirclrTimer >= eightShotTimeNextCirclrTime)
        {
            rotationSpeed = rotationSave;
            state = STATE.CIECLE_SHOT;
        }
    }

    void CicleShot()
    {
        eightShotTimeNextCirclrTimer = 0.0f;
        centerTarget.active = true;
        Boss03Manager.SetMode(8);
        transform.Rotate(0, 0, cirRotationSpeed * Time.deltaTime);

        //if (!Boss03Manager.GetCenterCol()) TackleMove(centerTarget.transform, centerMoveSpeed);
        /* if (Boss03Manager.GetCenterCol())*/
        cirShotTimer += Time.deltaTime;
        if (cirShotTimer >= cirShotTime)
        {
            shotSpanTimer += Time.deltaTime;
        }
        if (shotSpanTimer >= shotSpan && !circleShotFlg)
        {
            circleShotFlg = true;
            shotSpanTimer = 0.0f;
        }

        if (Boss03Manager.GetCenterCol()) circleShotNextTimer += Time.deltaTime;

        if (circleShotNextTimer >= circleShotNextTime)
        {
            transform.Rotate(0, 0, -cirRotationSpeed * Time.deltaTime);
            //Boss03Manager.SetCenterCol(false);
            circleShotFlg = false;
            state = STATE.TACKLE_SHOT_TARGET;
        }
    }
    void TackleShotTarget()
    {
        cirShotTimer = 0.0f;
        shotSpanTimer = 0.0f;
        circleShotNextTimer = 0.0f;
        cirRotationSpeed = rotationSpeedSave;
        circleShotFlg = false;

        Boss03Manager.SetTacklePos(0);
        centerTarget.active = false;
        Boss03Manager.SetCenterCol(false);

        tackleRand = Random.Range(0, 4);
        state = STATE.TACKLE_SHOT_BEFOR;
    }
    void TackleShotBefor()
    {
        Boss03Manager.SetTackleLine(true);
        Boss03Manager.SetOverlap(true);
        shotTackleBeforTimer += Time.deltaTime;

        if (tackleRand == 0)
        {
            TackleDir(tacklePos[0]);
        }
        if (tackleRand == 1)
        {
            TackleDir(tacklePos[1]);
        }
        if (tackleRand == 2)
        {
            TackleDir(tacklePos[2]);
        }
        if (tackleRand == 3)
        {
            TackleDir(tacklePos[3]);
        }

        if (shotTackleBeforTimer >= shotTackleBeforTime)
        {
            state = STATE.TACKLE_SHOT;
        }

    }

    void TackleShot()
    {
        Boss03Manager.SetMode(10);
        Boss03Manager.SetSpeed(tackleSpeed);

        Boss03Manager.SetTackleLine(false);
        Boss03Manager.SetOverlap(false);
        //tackleRand = 0;
        shotTackleBeforTimer = 0.0f;

        if (tackleRand == 0)
        {
            if (!shotTackleNow) TackleMove(tacklePos[0], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 1 && !shotTackleNow)
            {
                shotTackleNow = true;
                oneShotFlg = true;
            }
        }
        if (tackleRand == 1)
        {
            if (!shotTackleNow) TackleMove(tacklePos[1], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 2 && !shotTackleNow)
            {
                shotTackleNow = true;
                oneShotFlg = true;
            }
        }
        if (tackleRand == 2)
        {
            if (!shotTackleNow) TackleMove(tacklePos[2], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 3 && !shotTackleNow)
            {
                shotTackleNow = true;
                oneShotFlg = true;
            }
        }
        if (tackleRand == 3)
        {
            if (!shotTackleNow) TackleMove(tacklePos[3], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 4 && !shotTackleNow)
            {
                shotTackleNow = true;
                oneShotFlg = true;
            }
        }
        //if (timer == 1&&!oneShotFlg)
        //{
        //    oneShotFlg = true;
        //}
        if (shotTackleNow)
        {
            state = STATE.TACKLE_SHOT_END;
            Boss03Manager.SetOverlap(true);
        }

    }

    void TackleShotEnd()
    {
        oneShotFlg = false;
        shotTackleNow = false;
        Boss03Manager.SetCenterCol(false);
        shotTackleNextTimer += Time.deltaTime;
        if (shotTackleNextTimer >= shotTackleNextTime)
        {
            state = STATE.TACKLE_TARGET;
            //effectFlg = true;
            //tackleRand = 0;
        }
    }

    void DeadBeforCenter()
    {
        transform.position = centerTarget.transform.position;
        if (!shakeFlg)
        {
            var shakeObj = Camera.main.GetComponent<ShakeObject>();
            shakeObj.PlayShake(1.5f, 0.1f);
            shakeFlg = true;
        }
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = pos;
    }

    void DeadNow()
    {
        if (Boss03Manager.GetRealDead() && breakTimer <= breakTime)
        {
            TimeManager.GetInstance.stopFlg = true;
            Shake(0.5f, 0.5f);
            breakTimer += Time.deltaTime;
        }
        if (breakTimer >= breakTime)
        {
            Boss03Manager.SetRealDeadEnd(true);
            Destroy(gameObject);
            GameObject ef = Instantiate(breakEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 1.0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Boss03Manager.GetHp());
        DeadNow();
        //hp減ったら。
        if (FindObjectOfType<BossFinalHitJugement>().hp <= 0.0f && wait >= 10)
        {
            Boss03Manager.SetDeadBefor(true);
            if (Boss03Manager.GetDeadBefor())
            {
                DeadBeforCenter();
                Boss03Manager.SetTackleLine(false);
            }
        }

        if (TimeManager.GetInstance.timeScale >= 1.0f && Boss03Manager.GetForm() == 2 && !Boss03Manager.GetDeadBefor())
        {
            wait++;

            switch (state)
            {
                case STATE.START:
                    StartMove();
                    break;
                case STATE.TACKLE_TARGET:
                    TackleTarget();
                    break;
                case STATE.TACKLE_BEFOR:
                    TackleBefor();
                    break;
                case STATE.TACKLE:
                    Tackle();
                    break;
                case STATE.EIGHT_DIR_SHOT:
                    EightDirShot();
                    break;
                case STATE.EIGHT_DIR_SHOT_END:
                    EightDirShotEnd();
                    break;
                case STATE.CIECLE_SHOT:
                    CicleShot();
                    break;
                case STATE.TACKLE_SHOT_TARGET:
                    TackleShotTarget();
                    break;
                case STATE.TACKLE_SHOT_BEFOR:
                    TackleShotBefor();
                    break;
                case STATE.TACKLE_SHOT:
                    TackleShot();
                    break;
                case STATE.TACKLE_SHOT_END:
                    TackleShotEnd();
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

    }
}
