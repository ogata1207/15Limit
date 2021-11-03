using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02HitJugement : MonoBehaviour
{
    public float hp;

    public GameObject deadEffect;
    public GameObject shotHiteffect;

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0.0f)
        {
            //SoundManager.GetInstance.PlaySE("EnemyDead");
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 1.0f);
        }
        //Debug.Log(hp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ショット
        if (collision.tag == "Bullet")
        {
            SoundManager.GetInstance.PlaySE("enemy_shotdamage");
            //hp-=武器の攻撃力
            hp -= PlayerStatus.GetInstance.AttackDamage();
            if ( hp >= 1.0f || hp > 0.1f)
            {
                GameObject ef = Instantiate(shotHiteffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(ef, 0.5f);
            }
        }
    }
}
