using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core03 : MonoBehaviour
{
    private BoxCollider2D bCd2d;

    public float speed;
    private float time;
    public GameObject[] pos;
    private bool one, two, three, four, five;
    private GameObject core02;
    public GameObject deadEffect;
    public GameObject coreSoul;
    private bool soulFlg;

    //
    public bool shotStart;

    // 回転する時間
    private float rotationTimer;
    [SerializeField]
    [Header("回転する時間")]
    [Range(0f, 3f)]
    private float rotationTime;
    [SerializeField]
    [Header("回転する速度")]
    [Range(0f, 1000f)]
    private float rotationSpeed;
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
    private float waitTimer;
    [SerializeField]
    [Header("弾を撃った後の待機時間")]
    [Range(0f, 3f)]
    private float waitTime;
    //--------------------------------------------------
    private enum STATE
    {
        ROTATION,      //回転
        SHOTBEFORE,  //射精前
        SHOT,        //射精(*´Д`)
        DISPLAY,     //オーガズムの時間
        WAIT         //待機
    }
    STATE state;
    // Start is called before the first frame update
    void Start()
    {
        state = STATE.ROTATION;
        core02 = GameObject.Find("Core02");
        one = true;
        two = false;
        three = false;
        four = false;
        five = false;

        laserBeforTimer = 0;
        render = false;
        flash = false;
        laserDisplayTimer = 0;
        waitTimer = 0;
        shotStart = false;
        bCd2d = GetComponent<BoxCollider2D>();
        bCd2d.enabled = false;
    }

    void Update()
    {
        time = speed * Time.deltaTime;
        var myHp = this.GetComponent<CoreHitJugement>().hp;

        if (myHp <= 0.0f)
        {
            soulFlg = true;
            if (soulFlg)
            {
                Instantiate(coreSoul, transform.position, Quaternion.identity);
                soulFlg = false;
            }
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.3f);
            Destroy(gameObject);
        }

        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            if (core02 == null)
            {
                bCd2d.enabled = true;
                MoveSet();
                switch (state)
                {
                    case STATE.ROTATION:
                        Rotation();
                        break;
                    case STATE.SHOTBEFORE:
                        ShotBefore();
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

    }

    void Rotation()
    {
        render = true;
        waitTimer = 0;
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        rotationTimer += Time.deltaTime;
        if (rotationTimer >= rotationTime)
        {
            state = STATE.SHOTBEFORE;
        }
    }

    void ShotBefore()
    {
        flash = true;
        rotationTimer = 0;
        laserBeforTimer += Time.deltaTime;
        if (laserBeforTimer >= laserBeforTime)
        {
            state = STATE.SHOT;
        }
    }
    void Shot()
    {
        laserBeforTimer = 0;
        flash = false;
        shotStart = true;//射精
        laserDisplayTimer += Time.deltaTime;
        if (laserDisplayTimer >= laserDisplayTime)
        {
            state = STATE.WAIT;
        }
    }
    void Wait()
    {
        render = false;
        laserDisplayTimer = 0.0f;
        shotStart = false;
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            state = STATE.ROTATION;
        }
    }

    void MoveSet()
    {
        if (one)
        {
            five = false;
            OneMove();
        }
        if (two)
        {
            one = false;
            TwoMove();
        }
        if (three)
        {
            two = false;
            ThreeMove();
        }
        if (four)
        {
            three = false;
            FourMove();
        }
        if (five)
        {
            four = false;
            FiveMove();
        }
    }
    void OneMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos[0].transform.position, time);
        if (transform.position.x == pos[0].transform.position.x && transform.position.y == pos[0].transform.position.y)
        {
            two = true;
        }
    }
    void TwoMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos[1].transform.position, time);
        if (transform.position.x == pos[1].transform.position.x && transform.position.y == pos[1].transform.position.y)
        {
            three = true;
        }
    }
    void ThreeMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos[2].transform.position, time);
        if (transform.position.x == pos[2].transform.position.x && transform.position.y == pos[2].transform.position.y)
        {
            four = true;
        }
    }
    void FourMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos[3].transform.position, time);
        if (transform.position.x == pos[3].transform.position.x && transform.position.y == pos[3].transform.position.y)
        {
            five = true;
        }
    }
    void FiveMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos[4].transform.position, time);
        if (transform.position.x == pos[4].transform.position.x && transform.position.y == pos[4].transform.position.y)
        {
            one = true;
        }
    }

}
