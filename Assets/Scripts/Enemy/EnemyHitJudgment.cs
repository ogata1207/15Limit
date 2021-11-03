using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitJudgment : MonoBehaviour
{
    public float hp;

    public GameObject deadEffect;
    public GameObject hummerHitEffect;

    public GameObject shotHiteffect;

    private bool knockBack;
    private float knockbackPower;
    private Vector3 direction;
    

    //public GameObject nunchakuHitEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0.0f)
        {
            SoundManager.GetInstance.PlaySE("EnemyDead");
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 1.0f);
        }

        //---KBTIT---
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    knockBack = true;
        //}
        //KnockBack(new Vector3(0.0f,1.0f), 0.5f);
        //---KBTIT---
        KnockBack(direction, knockbackPower);
    }
    void KnockBack(Vector3 dir,float power)
    {
        if (knockBack)
        {
            transform.position = transform.position - (dir * power);
            knockBack = false;
        }
    }
    public void SetKnockBack(Vector3 dir, float power)
    {
        knockBack = true;

        direction = dir;
        knockbackPower = power;        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ショット
        if (collision.tag == "Bullet")
        {
            SoundManager.GetInstance.PlaySE("enemy_shotdamage");
            var damage = PlayerStatus.GetInstance.AttackDamage();

            //hp-=武器の攻撃力
            hp -= damage;
            DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));

            if (hp >= 1.0f || hp > 0.1f)
            {
                GameObject ef = Instantiate(shotHiteffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(ef, 0.5f);
            }
        }
        //ハンマー
        if (collision.tag == "Hammer")
        {
            SoundManager.GetInstance.PlaySE("Enemy_HammerDamage");

            var damage = PlayerStatus.GetInstance.AttackDamage();

            //hp-=武器の攻撃力
            hp -= damage;

            DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));
            if (hp >= 1.0f || hp > 0.1f)
            {
                GameObject ef = Instantiate(hummerHitEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(ef, 0.5f);
            }
        }

        ////ノックバック
        //if (collision.tag == "")
        //{
        //    knockBack = true;
        //}
    }

}
