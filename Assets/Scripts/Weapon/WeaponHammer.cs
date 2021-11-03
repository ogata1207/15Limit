using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHammer : MonoBehaviour, IWeapons
{
    public GameObject item;

    public float attackWait;
    public Vector3 pos;

    private float currentWait;
    private float attackSpeed;
    private PlayerStatus status;
    private Animator anim;

   
    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーに装備する
        var player = FindObjectOfType<PlayerController>();
        player.WeaponChange(Idle, Attack);
        status = PlayerStatus.GetInstance;
        status.currentWeapons = Weapons.Hammer;

        anim = GetComponent<Animator>();
        attackSpeed = 1.0f;
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
        //次の攻撃までの時間
        Mathf.Clamp(currentWait -= Time.deltaTime, 0, attackWait);

        //Idleに遷移
        anim.SetFloat("AttackSpeed", attackSpeed);

        transform.localPosition = pos;

        if (GetComponent<CircleCollider2D>().enabled)
        {
            var shakeObj = Camera.main.GetComponent<ShakeObject>();
            shakeObj.PlayShake(0.05f, 0.05f);
        }
    }

    public void Idle(PlayerInfo player)
    {
        attackSpeed = 0.0f;        
    }
   
    public void Attack(PlayerInfo player)
    {
        //基本の攻撃速度(レベル関係なし)
        var baseSpeed = status.attackSpeed * status.weaponStatus[(int)Weapons.Hammer].attackSpeed;

        //レベルに比例した攻撃速度の倍率
        var attackSpeedRate = status.currentAttackSpeedLevel * status.attackSpeedLevelRate;

        //最終結果 ( 基本攻撃速度*倍率 )   
        attackSpeed = baseSpeed + attackSpeedRate;

        //if (currentWait <= 0.0f)
        //{
        //    //攻撃アニメーション
        //    anim.SetBool("isAttack", true);

        //    //次の攻撃までの間隔をいれる
        //    currentWait = attackWait;
        //}
        //else
        //{
        //    anim.SetBool("isAttack", false);
        //}
    }

    public void DestroyObject()
    {
        var obj = Instantiate(item, transform.position, Quaternion.identity);
        Debug.Log("Destryo Hammer");
        Destroy(gameObject);
    }

    //衝突判定
    void OnTriggerEnter2D(Collider2D col)
    {
        //仮
        var shakeObj = Camera.main.GetComponent<ShakeObject>();
        shakeObj.PlayShake(0.1f, 0.1f);
    }

}
