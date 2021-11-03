using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02 : MonoBehaviour
{
    public GameObject muzleObj;
    private GameObject muzlePos;

    private bool randFlg;
    private float randTimer;
    [Header("ランダムに移動する時間")]
    public float randTime;

    private float randMoveTimer;
    [Header("ランダムに移動する時間のスピード")]
    public float randMoveTime;

    [Header("1回目のレーザー発射位置")]
    [Header("0:上 1:右 2:下 3:左")]
    public int laserPos01;

    [Header("2回目のレーザー発射位置")]
    [Header("0:上 1:右 2:下 3:左")]
    public int laserPos02;

    [Header("3回目のレーザー発射位置")]
    [Header("0:上 1:右 2:下 3:左")]
    public int laserPos03;

    [Header("4回目のレーザー発射位置")]
    [Header("0:上 1:右 2:下 3:左")]
    public int laserPos04;

    public GameObject[] bossFrame;
    private Vector3 rotation;

    ///
    //ボスがレーザー打った回数
    [HideInInspector]
    public int laserCount;
    private bool laser;
    //// レーザーを撃つ予備動作時間
    //private float laserPreviousTimer;
    //[SerializeField]
    //[Header("レーザーを撃つ予備動作時間")]
    //[Range(0f, 3f)]
    //private float laserPreviousTime;
    ////--------------------------------------------------

    // レーザーを撃つ予備動作時間
    private float laserBeforTimer;
    [SerializeField]
    [Header("レーザーを撃つ予備動作時間")]
    [Range(0f, 3f)]
    private float laserBeforTime;
    public bool render;
    public bool flash;
    //--------------------------------------------------

    // レーザーを表示時間
    private float laserDisplayTimer;
    [SerializeField]
    [Header("レーザーの表示時間")]
    [Range(0f, 3f)]
    private float laserDisplayTime;
    //--------------------------------------------------

    // レーザーを撃った後の待機時間
    private float waitTimer;
    [SerializeField]
    [Header("レーザーを撃った後の待機時間")]
    [Range(0f, 3f)]
    private float waitTime;
    //--------------------------------------------------
    public bool laserStart;

    public GameObject[] core;
    public GameObject frameEffect;
    public GameObject centerPos;
    public GameObject deadEffect;
    private BoxCollider2D bc2d;
    private int deadTimer;
    enum STATE
    {
        RAND,
        UP,
        RIGHT,
        DOWN,
        LEFT,
        LASER_BEFOR,
        LASER,
        WAIT
    }
    STATE state;
    private int rand;
    private int up, right, down, left;
    // Start is called before the first frame update
    void Start()
    {
        muzlePos = GameObject.Find("BossMazlePos");

        randFlg = false;
        randTimer = 0.0f;
        randMoveTimer = 0.0f;
        state = STATE.RAND;
        laserCount = 0;
        rotation = transform.eulerAngles;
        laserBeforTimer = 0.0f;
        laserDisplayTimer = 0.0f;
        waitTimer = 0.0f;
        laser = false;
        render = false;
        flash = false;
        //laserPreviousTimer = 0.0f;
        up = 0;
        right = 0;
        down = 0;
        left = 0;
        deadTimer = 0;
        bc2d = GetComponent<BoxCollider2D>();
        this.bc2d.enabled = false;
    }
    void Core()
    {
        for (int i = 0; i < 4; i++)
        {
            if (core[i] == null)
            {
                Destroy(bossFrame[i]);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        var myHp = this.GetComponent<Boss02HitJugement>().hp;

        if (myHp <= 0.0f)
        {
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.3f);
            Destroy(gameObject);
        }
        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            Dead();
            Core();
            switch (state)
            {
                case STATE.RAND:
                    RandMove();
                    break;
                case STATE.UP:
                    Up();
                    break;
                case STATE.RIGHT:
                    Right();
                    break;
                case STATE.DOWN:
                    Down();
                    break;
                case STATE.LEFT:
                    Left();
                    break;
                case STATE.LASER_BEFOR:
                    LaserBefor();
                    break;
                case STATE.LASER:
                    Laser();
                    break;
                case STATE.WAIT:
                    Wait();
                    break;
            }
        }
        Debug.Log(laserCount);

        transform.eulerAngles = rotation;
    }
    void RandMove()
    {
        waitTimer = 0.0f;

        randMoveTimer += Time.deltaTime;
        randTimer += Time.deltaTime;

        if (randMoveTimer >= randMoveTime)
        {
            randFlg = true;
        }
        if (randFlg)
        {
            randFlg = false;
            randMoveTimer = 0.0f;

            if (core[0] || core[1] || core[2] || core[3])
            {
                for (rand = Random.Range(0, 4); !core[rand]; rand = Random.Range(0, 4))
                {

                }
            }
            if (/*up == 0 &&*/ core[0])
            {
                if (rand == 0)
                {
                    transform.position = bossFrame[0].transform.position;
                    rotation.z = 180.0f;

                    //up = 1;
                    //right = 0;
                    //down = 0;
                    //left = 0;

                    if(deadTimer<=0) SoundManager.GetInstance.PlaySE("Boss02Warp");
                    FrameMoveEffect();
                }
            }

            if (/*right == 0 && */core[1])
            {
                if (rand == 1)
                {
                    transform.position = bossFrame[1].transform.position;
                    rotation.z = 90.0f;

                    //right = 1;
                    //up = 0;
                    //down = 0;
                    //left = 0;

                    if (deadTimer <= 0) SoundManager.GetInstance.PlaySE("Boss02Warp");
                    FrameMoveEffect();
                }
            }
            if (/*down == 0 &&*/ core[2])
            {
                if (rand == 2)
                {
                    transform.position = bossFrame[2].transform.position;
                    rotation.z = 0.0f;

                    //down = 1;
                    //up = 0;
                    //right = 0;
                    //left = 0;

                    if (deadTimer <= 0) SoundManager.GetInstance.PlaySE("Boss02Warp");
                    FrameMoveEffect();
                }
            }

            if (/*left == 0 &&*/ core[3])
            {
                if (rand == 3)
                {
                    transform.position = bossFrame[3].transform.position;
                    rotation.z = 270.0f;

                    //left = 1;
                    //up = 0;
                    //right = 0;
                    //down = 0;

                    if (deadTimer <= 0) SoundManager.GetInstance.PlaySE("Boss02Warp");
                    FrameMoveEffect();
                }
            }
        }

        if (randTimer >= randTime)
        {
            //レーザー一回目
            if (laserCount == 0) LaserPos01();
            else if (laserCount == 1) LaserPos02();
            else if (laserCount == 2) LaserPos03();
            else if (laserCount == 3) LaserPos04();
            //else UnkoChan();
        }
        // BreakRandMove();
    }

    //保険
    void UnkoChan()
    {
        rand = Random.Range(0, 4);
        if (rand == 0)
        {
            state = STATE.UP;
        }
        if (rand == 1)
        {
            state = STATE.RIGHT;
        }
        if (rand == 2)
        {
            state = STATE.DOWN;
        }
        if (rand == 3)
        {
            state = STATE.LEFT;
        }
    }

    void FrameMoveEffect()
    {
        GameObject ef = Instantiate(frameEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(ef, 0.1f);
    }

    #region レーザー発射位置
    void LaserPos01()
    {
        if (laserPos01 == 0)
        {
            state = STATE.UP;
        }
        if (laserPos01 == 1)
        {
            state = STATE.RIGHT;
        }
        if (laserPos01 == 2)
        {
            state = STATE.DOWN;
        }
        if (laserPos01 == 3)
        {
            state = STATE.LEFT;
        }
    }

    void LaserPos02()
    {
        if (laserPos02 == 0)
        {
            state = STATE.UP;
        }
        if (laserPos02 == 1)
        {
            state = STATE.RIGHT;
        }
        if (laserPos02 == 2)
        {
            state = STATE.DOWN;
        }
        if (laserPos02 == 3)
        {
            state = STATE.LEFT;
        }
    }

    void LaserPos03()
    {
        if (laserPos03 == 0)
        {
            state = STATE.UP;
        }
        if (laserPos03 == 1)
        {
            state = STATE.RIGHT;
        }
        if (laserPos03 == 2)
        {
            state = STATE.DOWN;
        }
        if (laserPos03 == 3)
        {
            state = STATE.LEFT;
        }
    }

    void LaserPos04()
    {
        if (laserPos04 == 0)
        {
            state = STATE.UP;
        }
        if (laserPos04 == 1)
        {
            state = STATE.RIGHT;
        }
        if (laserPos04 == 2)
        {
            state = STATE.DOWN;
        }
        if (laserPos04 == 3)
        {
            state = STATE.LEFT;
        }
        laserCount = -1;
    }
    #endregion
    #region 位置
    void Up()
    {
        if (deadTimer <= 0) SoundManager.GetInstance.PlaySE("Boss02Warp");
        EmergencyMove();

        right = 0;
        down = 0;
        left = 0;

        if (core[0])
        {
            up = 1;

            FrameMoveEffect();
            transform.position = bossFrame[0].transform.position;
            rotation.z = 180.0f;
            state = STATE.LASER_BEFOR;
        }

        if (core[0] == null)
        {
            up = 1;

            randTimer += Time.deltaTime;
            if (randTimer <= randTime)
            {
                rand = Random.Range(1, 4);
            }
            if (bossFrame[0] == null)
            {
                if (rand == 1)
                {
                    state = STATE.RIGHT;
                }
                if (rand == 2)
                {
                    state = STATE.DOWN;
                }
                if (rand == 3)
                {
                    state = STATE.LEFT;
                }
            }

        }
        randTimer = 0.0f;

    }
    void Right()
    {
        if (deadTimer <= 0) SoundManager.GetInstance.PlaySE("Boss02Warp");

        EmergencyMove();

        up = 0;
        down = 0;
        left = 0;

        if (core[1])
        {
            right = 1;

            FrameMoveEffect();
            transform.position = bossFrame[1].transform.position;
            rotation.z = 90.0f;
            state = STATE.LASER_BEFOR;
        }

        if (core[1] == null)
        {
            right = 1;
            randTimer += Time.deltaTime;
            if (randTimer <= randTime)
            {
                rand = Random.Range(1, 4);
            }
            if (bossFrame[1] == null)
            {
                if (rand == 1)
                {
                    state = STATE.UP;
                }
                if (rand == 2)
                {
                    state = STATE.DOWN;
                }
                if (rand == 3)
                {
                    state = STATE.LEFT;
                }
            }

        }
        randTimer = 0.0f;

    }
    void Down()
    {
        if (deadTimer <= 0) SoundManager.GetInstance.PlaySE("Boss02Warp");

        EmergencyMove();

        up = 0;
        right = 0;
        left = 0;

        if (core[2])
        {
            down = 1;

            FrameMoveEffect();
            transform.position = bossFrame[2].transform.position;
            rotation.z = 0.0f;
            state = STATE.LASER_BEFOR;
        }

        if (core[2] == null)
        {
            down = 1;

            randTimer += Time.deltaTime;
            if (randTimer <= randTime)
            {
                rand = Random.Range(1, 4);
            }
            if (bossFrame[2] == null)
            {
                if (rand == 1)
                {
                    state = STATE.UP;
                }
                if (rand == 2)
                {
                    state = STATE.RIGHT;
                }
                if (rand == 3)
                {
                    state = STATE.LEFT;
                }
            }

        }
        randTimer = 0.0f;

    }

    void Left()
    {
        if (deadTimer <= 0) SoundManager.GetInstance.PlaySE("Boss02Warp");

        EmergencyMove();

        up = 0;
        right = 0;
        down = 0;

        if (core[3])
        {
            left = 1;

            FrameMoveEffect();
            transform.position = bossFrame[3].transform.position;
            rotation.z = 270.0f;
            state = STATE.LASER_BEFOR;
        }

        if (core[3] == null)
        {
            left = 1;

            randTimer += Time.deltaTime;
            if (randTimer <= randTime)
            {
                rand = Random.Range(1, 4);
            }
            if (bossFrame[3] == null/* && left == 0*/)
            {
                if (rand == 1)
                {
                    state = STATE.UP;
                }
                if (rand == 2)
                {
                    state = STATE.DOWN;
                }
                if (rand == 3)
                {
                    state = STATE.RIGHT;
                }
            }
        }
        randTimer = 0.0f;

    }
    #endregion

    void LaserBefor()
    {
        flash = true;
        render = true;
        laserBeforTimer += Time.deltaTime;
        if (laserBeforTimer >= laserBeforTime)
        {
            var obj = Instantiate(muzleObj, muzlePos.transform.position, Quaternion.identity);
            Destroy(obj, laserBeforTime);
            state = STATE.LASER;
        }
        EmergencyMove();
    }
    void Laser()
    {
        laserBeforTimer = 0;
        flash = false;
        laserStart = true;//射精
        EmergencyMove();

        laserDisplayTimer += Time.deltaTime;
        if (laserDisplayTimer >= laserDisplayTime)
        {
            laserCount++;
            state = STATE.WAIT;
        }
        SoundManager.GetInstance.PlaySE("enemy_beam");
    }
    void Wait()
    {
        render = false;
        laserDisplayTimer = 0.0f;
        laserStart = false;
        EmergencyMove();
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            state = STATE.RAND;
        }
    }
    void ResetSan()
    {
        render = false;
        laserStart = false;
        flash = false;
        randTimer = 0.0f;
        randMoveTimer = 0.0f;
        laserBeforTimer = 0.0f;
        laserDisplayTimer = 0.0f;
        waitTimer = 0.0f;
    }
    void EmergencyMove()
    {
        if (up == 1 && core[0] == null)
        {
            ResetSan();
            state = STATE.RAND;
        }
        if (right == 1 && core[1] == null)
        {
            ResetSan();
            state = STATE.RAND;
        }
        if (down == 1 && core[2] == null)
        {
            ResetSan();
            state = STATE.RAND;
        }
        if (left == 1 && core[3] == null)
        {
            ResetSan();
            state = STATE.RAND;
        }
    }

    void Dead()
    {
        if (core[0] == null && core[1] == null && core[2] == null && core[3] == null)
        {
            deadTimer++;
            rotation.z = 0.0f;
            this.bc2d.enabled = true;
            transform.position = centerPos.transform.position;
            GameObject ef = Instantiate(frameEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.1f);
        }
        if (deadTimer == 1)
        {
            SoundManager.GetInstance.PlaySE("BossVoice");
        }
    }

}
