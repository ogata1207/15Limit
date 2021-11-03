using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinalHitJugement : MonoBehaviour
{
    public float hp;

    public GameObject deadEffect;
    public GameObject hummerHitEffect;

    public GameObject shotHiteffect;

    //public GameObject nunchakuHitEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Boss03Manager.SetHp(hp);

        if (hp <= 0.0f && !Boss03Manager.GetDeadPerformance())
        {
            SoundManager.GetInstance.PlaySE("EnemyDead");
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 1.0f);
            Boss03Manager.SetDeadPerformance(true);
        }
        if (Boss03Manager.GetDeadPerformance())
        {
            Boss03Manager.SetTackleLine(false);
        }

        //Debug.Log(Boss03Manager.GetHp());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Boss03Manager.GetDeadBefor())
        {
            //ショット
            if (collision.tag == "Bullet")
            {
                SoundManager.GetInstance.PlaySE("enemy_shotdamage");
                var damage = PlayerStatus.GetInstance.AttackDamage();

                //hp-=武器の攻撃力
                hp -= damage;
                if (!Boss03Manager.GetDeadPerformance()) DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));

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

                if (!Boss03Manager.GetDeadPerformance()) DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));
                if (hp >= 1.0f || hp > 0.1f)
                {
                    GameObject ef = Instantiate(hummerHitEffect, transform.position, Quaternion.identity) as GameObject;
                    Destroy(ef, 0.5f);
                }
            }
        }

        if (Boss03Manager.GetDeadBefor() && Boss03Manager.GetChildeDead()&&!Boss03Manager.GetRealDead())
        {
            //ショット
            if (collision.tag == "Bullet")
            {
                Boss03Manager.SetRealDead(true);
                var damage = PlayerStatus.GetInstance.AttackDamage();

                ////hp-=武器の攻撃力
                //hp -= damage;
                /*if (!Boss03Manager.GetDeadPerformance())*/ DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));
                //Destroy(gameObject);
                GameObject ef = Instantiate(shotHiteffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(ef, 0.5f);
            }
            //ハンマー
            if (collision.tag == "Hammer")
            {
                Boss03Manager.SetRealDead(true);

                SoundManager.GetInstance.PlaySE("Enemy_HammerDamage");

                var damage = PlayerStatus.GetInstance.AttackDamage();

                DamageText.GetInstance.DrawDamage(transform.position, damage.ToString("F0"));
                //Destroy(gameObject);
                GameObject ef = Instantiate(shotHiteffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(ef, 0.5f);
            }
        }

    }
}
