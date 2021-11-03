using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChildeHitJugement : MonoBehaviour
{
    public float hp;

    public GameObject deadEffect;
    public GameObject hummerHitEffect;
    public GameObject shotHiteffect;
    private int deadCount;
    private bool soudF;
    // Start is called before the first frame update
    void Start()
    {
        soudF = false;
        //deadCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            //cunt
            //deadCount++;
            //Boss03Manager.SetChildeDeadCount(deadCount);
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 1.0f);
            if (!soudF)
            {
                SoundManager.GetInstance.PlaySE("CoreBreak");
                soudF = false;
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Boss03Manager.GetDeadBefor())
        {
            //ショット
            if (collision.tag == "Bullet")
            {
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
    }
}
