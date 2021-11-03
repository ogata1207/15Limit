using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    private GameObject core01, core02;
    public bool shotStart;
    [Header("弾を撃った後の待機時間")]
    [Range(0.0f, 3.0f)]
    public float waitTime;
    private float waitTimer;
    private GameObject player;
    public GameObject deadEffect;

    //
    public bool laserStart;
    // 狙い定める時間
    private float lockonTimer;
    [SerializeField]
    [Header("狙い定める時間")]
    [Range(0f, 3f)]
    private float lockonTime;
    //--------------------------------------------------
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
    private float laserWaitTimer;
    [SerializeField]
    [Header("レーザーを撃った後の待機時間")]
    [Range(0f, 3f)]
    private float laserWaitTime;
    //--------------------------------------------------
    private int change;
    private enum STATE
    {
        SHOT,       //射精(*´Д`)
        WAIT,        //待機

        LOCKON,      //狙う
        SHOTBEFORE,  //射精前
        LASER,        //射精(*´Д`)
        DISPLAY,     //オーガズムの時間
        LASER_WAIT,         //待機
        DEAD
    }
    STATE state;

    // Start is called before the first frame update
    void Start()
    {
        state = STATE.SHOT;
        player = GameObject.FindWithTag("Player");
        core01 = GameObject.Find("Core01");
        core02 = GameObject.Find("Core02");
        change = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var myHp = this.GetComponent<BossHitJugment>().hp;

        if (myHp <= 0.0f)
        {
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.3f);
            Destroy(gameObject);
        }
        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            if (core01)
            {
                #region ロックオン
                var vec = (player.transform.position - transform.position).normalized;
                var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                #endregion

                switch (state)
                {
                    case STATE.SHOT:
                        Shot();
                        break;
                    case STATE.WAIT:
                        Wait();
                        break;
                }
            }
            if (core01 == null)
            {
                change++;
            }
            if (change == 1) state = STATE.LOCKON;

            if (core01 == null && core02)
            {
                switch (state)
                {
                    case STATE.LOCKON:
                        LockOn();
                        break;
                    case STATE.SHOTBEFORE:
                        ShotBefore();
                        break;
                    case STATE.SHOT:
                        Laser();
                        break;
                    case STATE.LASER_WAIT:
                        LaserWait();
                        break;
                }
            }
            if (core02 == null)
            {
                laserStart = false;
                render = false;
                flash = false;
                state = STATE.DEAD;
            }
        }
    }

    void Shot()
    {
        waitTimer = 0.0f;
        shotStart = true;
        state = STATE.WAIT;
        SoundManager.GetInstance.PlaySE("enemy_shot");
    }
    void Wait()
    {
        shotStart = false;
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            state = STATE.SHOT;
        }
    }

    void LockOn()
    {
        shotStart = false;
        render = true;
        laserWaitTimer = 0;
        #region ロックオン
        var vec = (player.transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        #endregion
        lockonTimer += Time.deltaTime;
        if (lockonTimer >= lockonTime)
        {
            state = STATE.SHOTBEFORE;
        }
    }
    void ShotBefore()
    {
        flash = true;
        lockonTimer = 0;
        laserBeforTimer += Time.deltaTime;
        if (laserBeforTimer >= laserBeforTime)
        {
            state = STATE.SHOT;
        }
    }
    void Laser()
    {
        laserBeforTimer = 0;
        flash = false;
        laserStart = true;//射精
        laserDisplayTimer += Time.deltaTime;
        if (laserDisplayTimer >= laserDisplayTime)
        {
            state = STATE.LASER_WAIT;
        }
        SoundManager.GetInstance.PlaySE("enemy_beam");
    }
    void LaserWait()
    {
        render = false;
        laserDisplayTimer = 0.0f;
        laserStart = false;
        laserWaitTimer += Time.deltaTime;
        if (laserWaitTimer >= laserWaitTime)
        {
            state = STATE.LOCKON;
        }
    }
}
