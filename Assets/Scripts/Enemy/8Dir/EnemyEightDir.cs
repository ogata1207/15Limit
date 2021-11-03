using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEightDir : MonoBehaviour
{
    [Header("8方向ランダムの敵ですわ(ﾟДﾟ )")]

    [Range(0.0f, 30.0f)]
    public float speed;

    [Header("初動時間")]
    [Range(0.0f, 3f)]
    public float startTime;

    [Header("スタン時間")]
    [Range(0f, 3f)]
    private float startTimer;
    private bool startFlg;

    //※どんだけ移動するのか
    private float moveAmountTimer;
    [SerializeField]
    [Header("移動量")]
    [Range(0f, 3f)]
    private float moveAmountTime;

    [Header("停止してから動きだすまでの時間")]
    [Range(0f, 3f)]
    public float waitTime;
    private float waitTimer;

    private GameObject player;

    private int rand;
    private Vector3 rotation;
    private float fProbabilityRate;
    private float serchTimer;
    private float angle;

    [Header("8方向ランダムに移動する%")]
    public float dirEight;
    [Header("Playerの向きに移動する%")]
    public float dirPlayer;

    private bool hit;   //壁に当たったフラグ
    private Rigidbody2D rb2d;
    // 攻撃を受けたときのスタン時間
    private bool stanFlg;
    private float stanTimer;
    [SerializeField]
    [Header("スタン時間")]
    [Range(1f, 60f * 2)]
    private float stanTime;
    //--------------------------------------------------

    // Playerの弾に何発か当たったらスタン
    private int count;
    [SerializeField]
    [Header("弾に何発当たったらスタンさせる？^q^")]
    private int shotCount;
    //--------------------------------------------------

    private float search;

    public GameObject DeadEffet;
    enum STATE
    {
        START,
        MOVE,
        WAIT
    }
    STATE state;
    // Start is called before the first frame update
    void Start()
    {
        state = STATE.START;
        moveAmountTimer = 0.0f;
        waitTimer = 0.0f;
        startTimer = 0;
        startFlg = false;
        player = GameObject.FindWithTag("Player");
        rand = 0;
        rotation = transform.eulerAngles;
        serchTimer = 0;
        fProbabilityRate = 0.0f;
        hit = false;
        stanFlg = false;
        stanTimer = 0;
        count = 0;
        rb2d = GetComponent<Rigidbody2D>();

        search = 0;
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
            state = STATE.MOVE;
            startTimer = 0.0f;
        }
    }

     void Dir()
    {
        search += Time.deltaTime;
        if (search >= moveAmountTime)
        {
            rand = Random.Range(0, 8);
        }
        #region 8方向
        if (Percent() <= dirEight)
        {
            if (search >= moveAmountTime)
            {
                switch (rand)
                {
                    case 0:
                        rotation.z = 0.0f;
                        break;
                    case 1:
                        rotation.z = -45.0f;
                        break;
                    case 2:
                        rotation.z = -90.0f;
                        break;
                    case 3:
                        rotation.z = -135.0f;
                        break;
                    case 4:
                        rotation.z = -180.0f;
                        break;
                    case 5:
                        rotation.z = -225.0f;
                        break;
                    case 6:
                        rotation.z = -270.0f;
                        break;
                    case 7:
                        rotation.z = -315.0f;
                        break;
                    default:
                        rotation.z = 0.0f;
                        break;
                }
            }
        }
        #endregion

        #region Playerの向き
        if (Percent() <= dirPlayer)
        {
            angle = GetAngle(transform.position, player.transform.position);

            if (search >= moveAmountTime)
            {
                //右上
                if (angle >= 20.0f && angle <= 60.0f)
                {
                    rotation.z = -45.0f;
                }
                //右
                if (angle >= 60.0f && angle <= 100.0f)
                {
                    rotation.z = -90;
                }
                //右下
                if (angle >= 100.0f && angle <= 140.0f)
                {
                    rotation.z = -135.0f;
                }
                //下
                if (angle >= 140.0f && angle <= 200.0f)
                {
                    rotation.z = -180.0f;
                }
                //左下
                if (angle >= 200.0f && angle <= 250.0f)
                {
                    rotation.z = -225.0f;
                }
                //左
                if (angle >= 250.0f && angle <= 285.0f)
                {
                    rotation.z = -270.0f;
                }
                //左上
                if (angle >= 285.0f && angle <= 340.0f)
                {
                    rotation.z = -315.0f;
                }
                //上
                if ((angle >= 340.0f && angle <= 360.0f) || (angle >= 0 && angle <= 20.0f))
                {
                    rotation.z = 0.0f;
                }
            }
        }
        #endregion
        transform.eulerAngles = rotation;

    }

    void Move()
    {
        waitTimer = 0.0f;
        Dir();
        Vector3 velocity = transform.rotation * new Vector3(0, speed, 0);
        transform.position += velocity * Time.deltaTime;
        moveAmountTimer += Time.deltaTime;
        if (moveAmountTimer >= moveAmountTime)
        {
            state = STATE.WAIT;
        }
    }

    void Wait()
    {
        search = 0.0f;
        moveAmountTimer = 0.0f;
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            state = STATE.MOVE;
        }
    }


    float GetAngle(Vector2 me, Vector2 target)
    {
        Vector2 dt = target - me;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        return degree;
    }
    float Percent()
    {
        return fProbabilityRate = Random.value * 100.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            hit = true;
        }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            rb2d.velocity = Vector3.zero;
            rb2d.angularVelocity = 0.0f;
        }
    }
}
