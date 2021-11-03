using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixidLaser : MonoBehaviour
{
    [Header("固定レーザーの敵ですわ(ﾟДﾟ )")]
    public bool shotStart;
    private Vector3 rotation;
    private float angle;

    [Header("初動時間")]
    [Range(0.0f, 3.0f)]
    public float startTime;
    private float startTimer;

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
    private float waitTimer;
    [SerializeField]
    [Header("弾を撃った後の待機時間")]
    [Range(0f, 3f)]
    private float waitTime;
    //--------------------------------------------------

    // 攻撃を受けたときのスタン時間
    private bool stanFlg;
    private float stanTimer;
    [SerializeField]
    [Header("スタン時間")]
    [Range(0f, 3f)]
    private float stanTime;
    //--------------------------------------------------
    // Playerの弾に何発か当たったらスタン
    private int count;
    [SerializeField]
    [Header("弾に何発当たったらスタンさせる？^q^")]
    private int shotCount;
    //--------------------------------------------------
    private GameObject player;
    public GameObject DeadEffet;
    private enum STATE
    {
        START,       //初動時間
        LOCKON,      //狙う
        SHOTBEFORE,  //射精前
        SHOT,        //射精(*´Д`)
        DISPLAY,     //オーガズムの時間
        WAIT         //待機
    }
    STATE state;

    // Start is called before the first frame update
    void Start()
    {
        startTimer = 0;
        rotation = transform.eulerAngles;
        lockonTimer = 0;
        laserBeforTimer = 0;
        render = false;
        flash = false;
        laserDisplayTimer = 0;
        waitTimer = 0;
        shotStart = false;
        angle = 0.0f;
        state = STATE.START;
        stanFlg = false;
        stanTimer = 0;
        count = 0;
        player = GameObject.FindWithTag("Player");
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
                case STATE.LOCKON:
                    LockOn();
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
    float GetAngle(Vector2 me, Vector2 target)
    {
        Vector2 dt = target - me;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;

        if (degree <= 0)
        {
            degree += 360;
        }

        return degree;
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
            state = STATE.LOCKON;
        }
    }
    void LockOn()
    {
        render = true;
        waitTimer = 0;
        startTimer = 0;
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
    void Shot()
    {
        //SoundManager.GetInstance.PlaySE("enemy_beam");
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
            state = STATE.LOCKON;
        }
    }

    //スタンフラグ用の当たり判定ですぅ・・・。
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
}
