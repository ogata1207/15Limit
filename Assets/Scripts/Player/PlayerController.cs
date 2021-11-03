using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//武器に共通の関数を持たせる(継承みたいな感じ)
public interface IWeapons
{
    void Attack(PlayerInfo player);
    void Idle(PlayerInfo player);
    void DestroyObject();
}

[System.Serializable]
public enum PlayerState
{
    Normal,
    Stun,
    InvincibleTime
}

//武器を使用する際に必要なプレイヤーの情報
public class PlayerInfo
{
    bool isAttack;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class PlayerController : OGT_Utility.Singleton<PlayerController>
{
    

    //public float stunTime;                          //硬直時間
    public float dashSpeed;                         //キーを押したときのダッシュ量
    public float currentStunTime;                   //

    public float shakePower;
    public float shakeTime;

    public Coroutine invincibleTime;

    [NonSerialized]
    static public float currentInvincibleTime;
    [NonSerialized]
    public float currentChargeTime;

    private bool isVibration;
   
    private bool haveWeapons;
    private float dashPower;                        //

    private Action<PlayerController> dashPassive;
 
    private Action<PlayerInfo> currentWeaponAttack; //現在所持している武器の攻撃
    private Action<PlayerInfo> currentWeaponIdle;   //現在所持している武器の待機状態


    private PlayerInfo info;                        //スティック入力等の情報
    private PlayerStatus status;
    private PlayerState currentState;

    private Vector3 movePower;
    private Vector3 direction;

    private ChargeGauge chargeGauge;
    private SoundManager soundManager;
    private ShakeObject shake;

    [SerializeField]
    private GameObject _KnockbackObject;
    static public GameObject knockbackObject;
    static public bool isKnockBack;

    //東がいじったエフェクトのprefab箱
    public GameObject DashC;
    public GameObject DashA;


    private bool isStun
    {
        get
        {
            if(currentStunTime > 0)
            {
                return true;
            }

            currentStunTime = 0.0f;
            return false;
        }
    }

    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        chargeGauge = ChargeGauge.GetInstance;
        status = PlayerStatus.GetInstance;
        soundManager = SoundManager.GetInstance;

        shake = Camera.main.gameObject.GetComponent<ShakeObject>();
       
        currentChargeTime = status.dashChargeTime;
        dashPower = 1.0f;
    }


    // Update is called once per frame
    void Update()
    {
        //移動
        movePower = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.Q)) gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");

        //スタン中動けなくなる
        if (!State()) return;

        movePower.x = Input.GetAxis("Horizontal");
        movePower.y = Input.GetAxis("Vertical");

        Dash();


        //方向指定(プレイヤーの向き)
        direction.x = Input.GetAxis("Horizontal2");
        direction.y = Input.GetAxis("Vertical2");

        VibrationController.GetInstance.PlayVibration(isVibration, 1.0f);
    }
    void FixedUpdate()
    {
        Move(movePower);

        if(direction.x > 0.05f || direction.x < -0.05f)
        {
            if(direction.y > 0.05f || direction.y < -0.05f)
            {
                Rotation(direction);
            }
        }
        
    }

    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************

    /// <summary>
    /// 
    /// </summary>
    /// <returns>スタン中 false</returns>
    bool State()
    {
        switch(currentState)
        {
            case PlayerState.Normal:
                //敵との衝突判定があるレイヤーを適応
                gameObject.layer = LayerMask.NameToLayer("Player");


                //攻撃するお
                if (currentWeaponAttack != null)
                    currentWeaponAttack(info);

                //敵の攻撃にあたればStateを移動させる
                if (isStun)
                {
                    //SE
                    soundManager.PlaySE("player_damage2");

                    //
                    shake.PlayShake(shakePower, shakeTime);

                    //敵との判定を消してるレイヤーに変更
                    gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");

                    //
                    currentState = PlayerState.Stun;
                    isVibration = true;
                }
                break;

            case PlayerState.Stun:

                //スタン時間減算
                currentStunTime -= Time.deltaTime;

                //敵との判定を消してるレイヤーに変更
                gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");

                //パッシブ
                
                if (isKnockBack && knockbackObject == null)
                {
                    knockbackObject = Instantiate(_KnockbackObject, transform);
                }
                else if (isKnockBack)
                {
                    knockbackObject.SetActive(true);
                }
               



                //待機状態
                if (currentWeaponIdle != null)
                    currentWeaponIdle(info);

                //すたんしゅうりょう
                if (!isStun)
                {
                    //State遷移
                    currentState = PlayerState.InvincibleTime;

                    //無敵モード
                    Invincible();
                }
                return false;

            case PlayerState.InvincibleTime:
                /*
                    コルーチンの中で通常に遷移
                    InvincibleTime() 参照
                */

                if (isKnockBack)
                    knockbackObject.SetActive(false);

                isVibration = false;

                if (currentWeaponAttack != null)
                    currentWeaponAttack(info);

                break;
        }
        return true;
    }

    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************
    public void DestroyWeapon()
    {
        currentWeaponAttack = null;
        currentWeaponIdle = null;
        if (haveWeapons)
        transform.GetComponentInChildren<IWeapons>().DestroyObject();
    }

    /// <summary>
    /// 武器変更[IWeapon]
    /// </summary>
    /// <param name="idle">待機</param>
    /// <param name="attack">攻撃</param>
    public void WeaponChange(Action<PlayerInfo> idle, Action<PlayerInfo> attack)
    {
        currentWeaponAttack = null;
        currentWeaponIdle = null;

        currentWeaponAttack = attack;
        currentWeaponIdle = idle;

        //SE
        //soundManager.PlaySE("WeaponChange");

        haveWeapons = true;
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Move(Vector2 power)
    {
        //元の位置
        var currentPosition = transform.position;

        //移動
        currentPosition += ((Vector3)power/10)*(status.moveSpeed)*dashPower;

        transform.position = currentPosition;
    }

    /// <summary>
    /// 右スティックにあわせてプレイヤーを回転させる
    /// </summary>
    public void Rotation(Vector3 dir)
    {
        ////元の位置
        var currentPosition = transform.position;
        currentPosition += dir;

        var diff = (currentPosition - transform.position).normalized;

        transform.rotation = Quaternion.FromToRotation(Vector2.up, diff);

    }

    /// <summary>
    /// ダッシュ
    /// </summary>
    void Dash()
    {
        #region Input系
        //ボタンを押したときのやつ
        if (Input.GetButtonDown("Dash"))
        {
            if (status.passiveDashCharge != null)
                status.passiveDashCharge();
            //ゲージをセットする
            chargeGauge.SetGauge(currentChargeTime);
        }

        //ボタンを押している途中のやつ
        if (Input.GetButton("Dash"))
        {
            //東がいじったエフェクト部分
            DashC.SetActive(true);
            DashA.SetActive(false);
            //ゲージを進める
            chargeGauge.GaugeUpdate();
        }

        //ボタンを離したときのやつ
        if (Input.GetButtonUp("Dash"))
        {
            //東がいじったエフェクト部分
            DashA.SetActive(true);
            DashC.SetActive(false);
            if (chargeGauge.CurrentProgressPercentage() >= 1.0f)
            {
                if (status.passiveDash != null)
                    status.passiveDash();
            }

            //ダーーーーーｭｼｭ
            soundManager.PlaySE("PlayerDash");
            dashPower = dashSpeed * chargeGauge.CurrentProgressPercentage();
            Debug.Log("DashSpeed : " + dashPower + "　[" + (chargeGauge.CurrentProgressPercentage() * 100) + "%]");

            //ゲージリセット
            chargeGauge.Reset();
        }
        #endregion

        //減速
        dashPower = Mathf.Clamp(dashPower - 0.1f, 1.0f, float.MaxValue);
    }
    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************

    /// <summary>
    /// 無敵時間
    /// </summary>
    /// <returns></returns>

    public IEnumerator InvincibleTime(float time)
    {
        var startTime = Time.time;
        var elapsedTime = 0.0f;
        var wait = new WaitForSeconds(0.1f);
        var spriteRenderer = GetComponent<SpriteRenderer>();

        //敵との衝突判定があるレイヤーを適応
        gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");

        //State遷移
        currentState = PlayerState.InvincibleTime;

        //無敵時間中ループさせる
        while (elapsedTime < time)
        {
            elapsedTime = Time.time - startTime;

            //プレイヤー点滅
            var enabled = !spriteRenderer.enabled;
            spriteRenderer.enabled = enabled;

            yield return wait;
        }
        Debug.Log("MUTEKI OWARI");


        //レイヤーを戻す
        gameObject.layer = LayerMask.NameToLayer("Player");
        spriteRenderer.enabled = true;

        //State遷移
        currentState = PlayerState.Normal;

        invincibleTime = null;
    }

    //********************************************************************************************************************
    //
    //
    //********************************************************************************************************************
    public void Invincible()
    {
        if(invincibleTime ==null)
        invincibleTime = StartCoroutine(InvincibleTime(currentInvincibleTime));
    }


}



