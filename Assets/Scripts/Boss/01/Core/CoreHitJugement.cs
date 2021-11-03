using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreHitJugement : MonoBehaviour
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
        if (hp <= 0.0f)
        {
            SoundManager.GetInstance.PlaySE("CoreBreak");
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 1.0f);
            //Destroy(gameObject);
        }
        //Debug.Log(hp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ショット
        if (collision.tag == "Bullet")
        {
            SoundManager.GetInstance.PlaySE("EnemyShot");
            //hp-=武器の攻撃力
            hp -= PlayerStatus.GetInstance.AttackDamage();
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
            //hp-=武器の攻撃力
            hp -= PlayerStatus.GetInstance.AttackDamage();
            if (hp >= 1.0f || hp > 0.1f)
            {
                GameObject ef = Instantiate(hummerHitEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(ef, 0.5f);
            }
        }
    }
}
