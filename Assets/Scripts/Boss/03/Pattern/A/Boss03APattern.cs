using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03APattern : MonoBehaviour
{
    //private SpriteRenderer sp;
    enum STATE
    {
        START,
        TWO_TACKLE_BEFOR,
        TWO_TACKLE,
        FOUR_DIR_SHOT,
        FOUR_DIR_END,
        ONE_TACKLE_BEFOR,
        ONE_TACKLE,
        PATTERN_CHANGE
    }
    STATE state;
    //初動
    private float startTimer;

    //2回タックル
    private float twoTackleBeforTimer;
    private float tackleNextFourShotTimer;
    private int tackleCount;
    private bool tackleNow;
    private bool twoTackleF;

    //4方向ショット
    public GameObject centerTarget;
    private float shotTimer;
    private bool effectFlg;
    //public GameObject shotChegeEffect;
    private float rotationSave;

    //1回タックル
    private float fourShotTimeNextOneTackleTimer;
    private float oneTackleBeforTimer;
    private float patternChangeTimer;
    private int rand;

    [Header("============Aパターン============")]
    public float tackleSpeed;
    [Header("初動時間")]
    public float startTime;
    public Transform[] tacklePosObj;
    [Header("0:左上 1:右上 2:右下 3:左下")]
    [Header("------タックル2回------")]
    public int[] tacklePos02;
    [Header("タックル(2回)する前の警告線表示時間")]
    public float twoTackleBeforTime;
    [Header("タックル2回から4方向ショット1回遷移時間")]
    public float tackleNextFourShotTime;

    [Header("真ん中に移動するスピード")]
    [Header("------4方向ショット1回------")]
    public float centerMoveSpeed;
    [Header("回転する速度")]
    public float rotationSpeed;
    [System.NonSerialized]
    public bool shotFlg;
    private bool shotEndFlg;
    [Header("真ん中に到着してから弾撃つまでの時間")]
    public float shotTime;
    [Header("4方向ショット1回からタックル1回遷移時間")]
    public float fourShotTimeNextOneTackleTime;

    [Header("0:左上 1:右上 2:右下 3:左下")]
    [Header("------タックル1回------")]
    public int[] tacklePos01;
    [Header("タックル(1回)する前の警告線表示時間")]
    public float oneTackleBeforTime;
    [Header("タックル(1回)後BorCorDパターン遷移時間")]
    public float patternChangeTime;

    [Header("HPが一定以下の値(Dの遷移条件)")]
    public float constanHp;

    public bool constanFlg;

    // Start is called before the first frame update
    void Start()
    {
        Boss03Manager.Start();

        //sp = GetComponent<SpriteRenderer>();
        Boss03Manager.SetPattern(0);//Aパターン

        state = STATE.START;
        startTimer = 0.0f;

        tackleNow = false;
        twoTackleF = false;
        twoTackleBeforTimer = 0.0f;
        tackleNextFourShotTimer = 0.0f;
        tackleCount = 0;

        centerTarget.active = false;
        shotTimer = 0.0f;
        shotEndFlg = false;
        effectFlg = false;

        fourShotTimeNextOneTackleTimer = 0.0f;
        oneTackleBeforTimer = 0.0f;
        patternChangeTimer = 0.0f;
        rand = 0;

        rotationSave = rotationSpeed;
        constanFlg = false;
    }

    void StartMove()
    {
        startTimer += Time.deltaTime;
        if (startTimer >= startTime)
        {
            state = STATE.TWO_TACKLE_BEFOR;
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
    void TwoTackleBefor()
    {
        Boss03Manager.SetSpeed(tackleSpeed);

        Boss03Manager.SetTacklePos(0);

        Boss03Manager.SetMode(2);

        ///レーザーみたいな感じで指定した位置に警告線
        Boss03Manager.SetTackleLine(true);
        Boss03Manager.SetOverlap(true);
        if (Boss03Manager.GetMode() == 2)
        {
            twoTackleBeforTimer += Time.deltaTime;
        }

        if (Boss03Manager.GetTackleCount() == 0)
        {
            if (tacklePos02[0] == 0)
            {
                TackleDir(tacklePosObj[0]);
            }
            if (tacklePos02[0] == 1)
            {
                TackleDir(tacklePosObj[1]);
            }
            if (tacklePos02[0] == 2)
            {
                TackleDir(tacklePosObj[2]);
            }
            if (tacklePos02[0] == 3)
            {
                TackleDir(tacklePosObj[3]);
            }
        }

        if (Boss03Manager.GetTackleCount() == 1)
        {
            if (tacklePos02[1] == 0)
            {
                TackleDir(tacklePosObj[0]);
            }
            if (tacklePos02[1] == 1)
            {
                TackleDir(tacklePosObj[1]);
            }
            if (tacklePos02[1] == 2)
            {
                TackleDir(tacklePosObj[2]);
            }
            if (tacklePos02[1] == 3)
            {
                TackleDir(tacklePosObj[3]);
            }
        }

        if (twoTackleBeforTimer >= twoTackleBeforTime)
        {
            state = STATE.TWO_TACKLE;
            if (Boss03Manager.GetTackleCount() == 1)
            {
                tackleCount++;
                state = STATE.TWO_TACKLE;
            }

        }
    }
    void TwoTackle()
    {
        Boss03Manager.SetTackleLine(false);
        Boss03Manager.SetOverlap(false);
        twoTackleBeforTimer = 0.0f;

        #region タックル1回目
        if (Boss03Manager.GetTackleCount() == 0)
        {
            if (tacklePos02[0] == 0)
            {
                if (!tackleNow) TackleMove(tacklePosObj[0], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 1 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos02[0] == 1)
            {
                if (!tackleNow) TackleMove(tacklePosObj[1], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 2 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos02[0] == 2)
            {
                if (!tackleNow) TackleMove(tacklePosObj[2], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 3 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
            if (tacklePos02[0] == 3)
            {
                if (!tackleNow) TackleMove(tacklePosObj[3], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 4 && !tackleNow)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    tackleCount++;
                    tackleNow = true;
                }
            }
        }
        #endregion
        if (Boss03Manager.GetTackleCount() == 1)
        {
            state = STATE.TWO_TACKLE_BEFOR;
        }
        #region タックル2回目
        if (Boss03Manager.GetTackleCount() == 2)
        {
            if (tacklePos02[1] == 0)
            {
                if (!twoTackleF) TackleMove(tacklePosObj[0], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 1 && !twoTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    twoTackleF = true;
                    transform.position = tacklePosObj[0].position;
                }
            }
            if (tacklePos02[1] == 1)
            {
                if (!twoTackleF) TackleMove(tacklePosObj[1], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 2 && !twoTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    twoTackleF = true;
                    transform.position = tacklePosObj[1].position;
                }
            }
            if (tacklePos02[1] == 2)
            {
                if (!twoTackleF) TackleMove(tacklePosObj[2], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 3 && !twoTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    twoTackleF = true;
                    transform.position = tacklePosObj[2].position;
                }
            }
            if (tacklePos02[1] == 3)
            {
                if (!twoTackleF) TackleMove(tacklePosObj[3], Boss03Manager.GetSpeed());
                if (Boss03Manager.GetTacklePos() == 4 && !twoTackleF)
                {
                    SoundManager.GetInstance.PlaySE("boss_tacckle");
                    twoTackleF = true;
                    transform.position = tacklePosObj[3].position;
                }
            }
        }
        #endregion

        if (twoTackleF)
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
        twoTackleF = false;
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
            state = STATE.FOUR_DIR_END;
        }

    }

    void FourDirShotEnd()
    {
        Boss03Manager.SetCenterCol(false);
        shotTimer = 0.0f;

        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        shotFlg = false;
        shotEndFlg = false;

        rotationSpeed -= rotationSpeed * Time.deltaTime;
        if (rotationSpeed <= 0.0f) rotationSpeed = 0.0f;

        fourShotTimeNextOneTackleTimer += Time.deltaTime;
        if (fourShotTimeNextOneTackleTimer >= fourShotTimeNextOneTackleTime)
        {
            rotationSpeed = rotationSave;
            state = STATE.ONE_TACKLE_BEFOR;
        }
    }
    void OneTackleBefor()
    {
        Boss03Manager.SetSpeed(tackleSpeed);

        Boss03Manager.SetMode(1);
        fourShotTimeNextOneTackleTimer = 0.0f;

        Boss03Manager.SetTackleLine(true);
        Boss03Manager.SetOverlap(true);

        if (Boss03Manager.GetMode() == 1)
        {
            oneTackleBeforTimer += Time.deltaTime;
        }
        if (tacklePos01[0] == 0)
        {
            TackleDir(tacklePosObj[0]);
        }
        if (tacklePos01[0] == 1)
        {
            TackleDir(tacklePosObj[1]);
        }
        if (tacklePos01[0] == 2)
        {
            TackleDir(tacklePosObj[2]);
        }
        if (tacklePos01[0] == 3)
        {
            TackleDir(tacklePosObj[3]);
        }

        if (oneTackleBeforTimer >= oneTackleBeforTime)
        {
            state = STATE.ONE_TACKLE;
        }
    }
    void OneTackle()
    {
        Boss03Manager.SetTackleLine(false);
        Boss03Manager.SetOverlap(false);
        oneTackleBeforTimer = 0.0f;

        if (tacklePos01[0] == 0)
        {
            if (!tackleNow) TackleMove(tacklePosObj[0], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 1 && !tackleNow)
            {
                SoundManager.GetInstance.PlaySE("boss_tacckle");
                tackleNow = true;
            }
        }
        if (tacklePos01[0] == 1)
        {
            if (!tackleNow) TackleMove(tacklePosObj[1], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 2 && !tackleNow)
            {
                SoundManager.GetInstance.PlaySE("boss_tacckle");
                tackleNow = true;
            }
        }
        if (tacklePos01[0] == 2)
        {
            if (!tackleNow) TackleMove(tacklePosObj[2], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 3 && !tackleNow)
            {
                SoundManager.GetInstance.PlaySE("boss_tacckle");
                tackleNow = true;
            }
        }
        if (tacklePos01[0] == 3)
        {
            if (!tackleNow) TackleMove(tacklePosObj[3], Boss03Manager.GetSpeed());
            if (Boss03Manager.GetTacklePos() == 4 && !tackleNow)
            {
                SoundManager.GetInstance.PlaySE("boss_tacckle");
                tackleNow = true;
            }
        }

        if (tackleNow)
        {
            patternChangeTimer += Time.deltaTime;
            Boss03Manager.SetOverlap(true);
            Boss03Manager.SetCenterCol(false);
        }
        if (FindObjectOfType<Boss03HitJugement>().hp >= constanHp)
        {
            rand = Random.Range(1, 3);
        }

        //Aパターンの時HPが少なかったら強制的にDパターンにしよう。
        if (patternChangeTimer >= patternChangeTime)
        {
            if (FindObjectOfType<Boss03HitJugement>().hp <= constanHp)
            {
                //sp.enabled = false;
                Boss03Manager.SetPattern(3);
            }
            if (rand == 1 && FindObjectOfType<Boss03HitJugement>().hp >= constanHp)
            {
                Boss03Manager.SetPattern(1);
            }
            if (rand == 2 && FindObjectOfType<Boss03HitJugement>().hp >= constanHp)
            {
                Boss03Manager.SetPattern(2);
            }

            patternChangeTimer = 0.0f;
            tackleNow = false;
            if (!tackleNow)
            {
                state = STATE.TWO_TACKLE_BEFOR;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Boss03Manager.SetTackleCount(tackleCount);
        Debug.Log(constanFlg);
        if (Boss03Manager.GetPattern() == 0 && FindObjectOfType<Boss03HitJugement>().hp <= constanHp)
        {
            constanFlg = true;
        }
        else
        {
            constanFlg = false;
        }

        if (TimeManager.GetInstance.timeScale >= 1.0f && Boss03Manager.GetPattern() == 0 && !Boss03Manager.GetDeadPerformance())
        {
            switch (state)
            {
                case STATE.START:
                    StartMove();
                    break;
                case STATE.TWO_TACKLE_BEFOR:
                    TwoTackleBefor();
                    break;
                case STATE.TWO_TACKLE:
                    TwoTackle();
                    break;
                case STATE.FOUR_DIR_SHOT:
                    FourDirShot();
                    break;
                case STATE.FOUR_DIR_END:
                    FourDirShotEnd();
                    break;
                case STATE.ONE_TACKLE_BEFOR:
                    OneTackleBefor();
                    break;
                case STATE.ONE_TACKLE:
                    OneTackle();
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

