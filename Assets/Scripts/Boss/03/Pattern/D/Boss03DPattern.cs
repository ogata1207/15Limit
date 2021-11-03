using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03DPattern : MonoBehaviour
{
    enum STATE
    {
        FADE_OUT,
        DENGER01,
        DOWN_TACKLE,
        DENGER02,
        UP_TACKLE,
        DENGER03,
        MIDLE_TACKLE,
        END,
    }
    STATE state;

    public GameObject target;
    public GameObject[] pos;
    public GameObject[] tacklePos;
    private bool cameraOut;
    private float downDengerTimer;
    private bool downEndFlg;

    private float upDengerTimer;
    private bool upEndFlg;

    private float midleDengerTimer;
    private bool midleEndFlg;


    [Header("画面外に行く速度")]
    [Header("==========パターンD==========")]
    public float fadeOutSpeed;
    [Header("タックル速度")]
    public float tackeSpeed;
    [Header("画面外に出てから下段の警告表示時間")]
    public float downDengerTime;
    [Header("画面外に出てから上段の警告表示時間")]
    public float upDengerTime;
    [Header("画面外に出てから中段の警告表示時間")]
    public float midleDengerTime;

    private bool warningFlg;
    // Start is called before the first frame update
    void Start()
    {
        state = STATE.FADE_OUT;
        downDengerTimer = 0.0f;
        cameraOut = false;
        downEndFlg = false;

        upDengerTimer = 0.0f;
        upEndFlg = false;

        midleDengerTimer = 0.0f;
        midleEndFlg = false;

        warningFlg = false;
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

    void FadeOut()
    {
        Boss03Manager.SetSpeed(fadeOutSpeed);
        if (!cameraOut)
        {
            TackleDir(target.transform);
            TackleMove(target.transform, fadeOutSpeed);
            SoundManager.GetInstance.PlaySE("boss_tacckle");
        }
        if (cameraOut)
        {
            transform.position = pos[0].transform.position;
            state = STATE.DENGER01;
        }
    }

    void Denger01()
    {
        if (!warningFlg)
        {
            SoundManager.GetInstance.PlaySE("warning");
            warningFlg = true;
        }
        cameraOut = false;
        Boss03Manager.SetDenPos(1);
        downDengerTimer += Time.deltaTime;
        if (downDengerTimer >= downDengerTime)
        {
            state = STATE.DOWN_TACKLE;
        }
    }

    void DownTackle()
    {
        warningFlg = false;
        Boss03Manager.SetOverlap(false);
        Boss03Manager.SetSpeed(tackeSpeed);

        Boss03Manager.SetDenPos(0);
        downDengerTimer = 0.0f;
        cameraOut = false;
        if (!downEndFlg)
        {
            TackleMove(tacklePos[0].transform, tackeSpeed);
            SoundManager.GetInstance.PlaySE("boss_tacckle");
        }
        if (downEndFlg) state = STATE.DENGER02;
    }

    void Denger02()
    {
        if (!warningFlg)
        {
            SoundManager.GetInstance.PlaySE("warning");
            warningFlg = true;
        }
        upDengerTimer += Time.deltaTime;
        Boss03Manager.SetDenPos(2);
        upDengerTimer += Time.deltaTime;
        if (upDengerTimer >= upDengerTime)
        {
            transform.position = pos[1].transform.position;
            state = STATE.UP_TACKLE;
        }
    }
    void UpTackle()
    {
        warningFlg = false;

        Boss03Manager.SetOverlap(false);
        Boss03Manager.SetSpeed(tackeSpeed);

        Boss03Manager.SetDenPos(0);
        upDengerTimer = 0.0f;

        if (!upEndFlg)
        {
            TackleMove(tacklePos[1].transform, tackeSpeed);
            SoundManager.GetInstance.PlaySE("boss_tacckle");
        }
        if (upEndFlg) state = STATE.DENGER03;
    }

    void Denger03()
    {
        if (!warningFlg)
        {
            SoundManager.GetInstance.PlaySE("warning");
            warningFlg = true;
        }
        //tacklePos[2].active = false;
        midleEndFlg = false;

        midleDengerTimer += Time.deltaTime;
        Boss03Manager.SetDenPos(3);
        midleDengerTimer += Time.deltaTime;
        if (midleDengerTimer >= midleDengerTime)
        {
            transform.position = pos[2].transform.position;
            state = STATE.MIDLE_TACKLE;
        }
    }
    void MidleTackle()
    {
        warningFlg = false;
        //tacklePos[2].active = true;
        Boss03Manager.SetSpeed(tackeSpeed);

        Boss03Manager.SetDenPos(0);
        Boss03Manager.SetOverlap(false);
        midleDengerTimer = 0.0f;

        TackleDir(tacklePos[2].transform);
        if (!midleEndFlg)
        {
            TackleMove(tacklePos[2].transform, tackeSpeed);
            SoundManager.GetInstance.PlaySE("boss_tacckle");
        }
        if (midleEndFlg)
        {
            var shakeObj = Camera.main.GetComponent<ShakeObject>();
            shakeObj.PlayShake(1.5f, 0.1f);
            Boss03Manager.SetOverlap(true);
            TackleMove(tacklePos[2].transform, -2.0f);
            transform.position = transform.position - (transform.up * 3.0f);
            state = STATE.END;
        }
    }

    void End()
    {

    }

    //void Areturn()
    //{
    //    state = STATE.FADE_OUT;
    //    PatternAchangeTimer = 0.0f;
    //    Boss03Manager.SetPattern(0);
    //    Boss03Manager.SetOverlap(false);
    //    downEndFlg = false;
    //    upEndFlg = false;
    //    midleEndFlg = false;
    //}

    // Update is called once per frame
    void Update()
    {
        //Boss03Manager.SetPattern(3);
        if (TimeManager.GetInstance.timeScale >= 1.0f && Boss03Manager.GetPattern() == 3 && !Boss03Manager.GetDeadPerformance())
        {
            switch (state)
            {
                case STATE.FADE_OUT:
                    FadeOut();
                    break;
                case STATE.DENGER01:
                    Denger01();
                    break;
                case STATE.DOWN_TACKLE:
                    DownTackle();
                    break;
                case STATE.DENGER02:
                    Denger02();
                    break;
                case STATE.UP_TACKLE:
                    UpTackle();
                    break;
                case STATE.DENGER03:
                    Denger03();
                    break;
                case STATE.MIDLE_TACKLE:
                    MidleTackle();
                    break;
                case STATE.END:
                    End();
                    break;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CameraOut")
        {
            cameraOut = true;
        }
        if (collision.tag == "Down")
        {
            downEndFlg = true;
        }
        if (collision.tag == "Up")
        {
            upEndFlg = true;
        }
        if (collision.tag == "Midle")
        {
            SoundManager.GetInstance.PlaySE("boss_tacckleWall");
            midleEndFlg = true;
        }
    }
}
