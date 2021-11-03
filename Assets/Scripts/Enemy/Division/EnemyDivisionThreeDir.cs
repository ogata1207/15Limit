using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDivisionThreeDir : MonoBehaviour
{
    [Header("3方向分裂の敵(ﾟДﾟ )")]

    [Range(0.0f, 30.0f)]
    public float speed;

    [Header("初動時間")]
    [Range(0, 3f)]
    public float startTime;
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
    private float establishment;
    private float angle;

    [Header("Playerの向きじゃないに方に移動する%")]
    public float dirTrhee;
    [Header("Playerの向きに移動する%")]
    public float dirPlayer;

    // 攻撃を受けたときのスタン時間
    private bool stanFlg;
    private float stanTimer;
    [SerializeField]
    [Header("スタン時間")]
    [Range(0, 3f)]
    private float stanTime;
    //--------------------------------------------------

    // Playerの弾に何発か当たったらスタン
    private int count;
    [SerializeField]
    [Header("弾に何発当たったらスタンさせる？^q^")]
    private int shotCount;
    //--------------------------------------------------

    private bool hit;   //壁に当たったフラグ
    private Rigidbody2D rb2d;

    private float search;
    public GameObject[] childe;
    private Vector3 pos1;
    private Vector3 pos2;

    private bool deadF;
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

        startFlg = false;
        waitTimer = 0.0f;
        player = GameObject.FindWithTag("Player");
        rand = 0;
        rotation = transform.eulerAngles;
        establishment = 0.0f;
        hit = false;
        stanFlg = false;
        stanTimer = 0;
        count = 0;
        search = 0;

        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Division();
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
    void Division()
    {
        pos1 = transform.position;
        pos2 = transform.position;

        var myHp = this.GetComponent<EnemyHitJudgment>().hp;

        if (myHp <= 0.0f)
        {
            GameObject ef = Instantiate(DeadEffet, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.3f);
            deadF = true;
        }
        if (deadF)
        {
            var enemyGroup = GameObject.FindWithTag("EnemyGroup");

            pos1.y = pos1.y + 1.5f;
            pos2.y = pos2.y - 1.5f;

            var newEnemy0 = Instantiate(childe[0], pos1, Quaternion.identity);
            var newEnemy1 = Instantiate(childe[1], pos2, Quaternion.identity);
            newEnemy0.transform.parent = enemyGroup.transform;
            newEnemy1.transform.parent = enemyGroup.transform;
            Destroy(gameObject);
            deadF = false;
        }
        Debug.Log(myHp);
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
    //Playerの方を向く(50%の確率で)
    //残り50%はPlayerを主軸にして2方向
    void Dir()
    {
        search += Time.deltaTime;
        angle = GetAngle(transform.position, player.transform.position);
        if (search >= moveAmountTime)
        {
            #region 右上
            if (angle >= 20.0f && angle <= 60.0f)
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = -45.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = 0.0f;      //上
                    }
                    else
                    {
                        rotation.z = -90.0f;    //右
                    }
                }
            }
            #endregion

            #region 右
            if (angle >= 60.0f && angle <= 100.0f)
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = -90.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = -45.0f;      //右上
                    }
                    else
                    {
                        rotation.z = -135.0f;    //右下
                    }
                }
            }
            #endregion

            #region 右下
            if (angle >= 100.0f && angle <= 140.0f)
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = -135.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = -90.0f;      //右
                    }
                    else
                    {
                        rotation.z = -180.0f;    //下
                    }
                }
            }
            #endregion

            #region 下
            if (angle >= 140.0f && angle <= 200.0f)
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = -180.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = -135.0f;      //右下
                    }
                    else
                    {
                        rotation.z = -225.0f;    //左下
                    }
                }
            }
            #endregion

            #region 左下
            if (angle >= 200.0f && angle <= 250.0f)
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = -225.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = -180.0f;      //下
                    }
                    else
                    {
                        rotation.z = -270.0f;    //左
                    }
                }
            }
            #endregion

            #region 左
            if (angle >= 250.0f && angle <= 285.0f)
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = -270.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = -225.0f;      //左下
                    }
                    else
                    {
                        rotation.z = -315.0f;    //左上
                    }
                }
            }
            #endregion

            #region 左上
            if (angle >= 285.0f && angle <= 340.0f)
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = -315.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = -270.0f;      //左
                    }
                    else
                    {
                        rotation.z = 0.0f;    //上
                    }
                }
            }
            #endregion

            #region 上
            if ((angle >= 340.0f && angle <= 360.0f) || (angle >= 0 && angle <= 20.0f))
            {
                if (Percent() <= dirPlayer)
                {
                    rotation.z = 0.0f;
                }
                else
                {
                    if (Percent() <= dirTrhee)
                    {
                        rotation.z = -315.0f;      //左上
                    }
                    else
                    {
                        rotation.z = -45.0f;    //右上
                    }
                }
            }
            #endregion
        }
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

    float Percent()
    {
        return establishment = Random.value * 100.0f;
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
        //ヌンチャク
        if (collision.tag == "" && !stanFlg)
        {

        }
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
