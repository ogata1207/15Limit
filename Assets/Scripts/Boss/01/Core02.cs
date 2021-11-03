using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core02 : MonoBehaviour
{
    private BoxCollider2D bCd2d;
    private GameObject core01;
    public GameObject deadEffect;
    public GameObject coreSoul;
    public GameObject[] pos;
    public float speed;
    private bool one, two, three, four,move;
    private bool soulFlg;
    enum STATE
    {
        ONE,
        TWO,
        THREE,
        FOUR
    }
    STATE state;
    // Start is called before the first frame update
    void Start()
    {
        core01 = GameObject.Find("Core01");
        pos[0] = GameObject.Find("01");
        pos[1] = GameObject.Find("02");
        pos[2] = GameObject.Find("03");
        pos[3] = GameObject.Find("04");
        state = STATE.ONE;
        one = false;
        two = false;
        three = false;
        four = false;
        move = false;
        bCd2d = GetComponent<BoxCollider2D>();
        bCd2d.enabled = false;
        soulFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        var myHp = this.GetComponent<CoreHitJugement>().hp;

        if (myHp <= 0.0f)
        {
            soulFlg = true;
            if (soulFlg)
            {
                Instantiate(coreSoul, transform.position, Quaternion.identity);
                soulFlg = false;
            }
            GameObject ef = Instantiate(deadEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.3f);
            Destroy(gameObject);
        }
        if (core01 == null)
        {
            bCd2d.enabled = true;
            move = true;
        }
        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            if (move)
            {
                switch (state)
                {
                    case STATE.ONE:
                        OneMove();
                        break;
                    case STATE.TWO:
                        TwoMove();
                        break;
                    case STATE.THREE:
                        ThreeMove();
                        break;
                    case STATE.FOUR:
                        FourMove();
                        break;
                }
            }
        }
    }
    void Move()
    {
        Vector3 velocity = transform.rotation * new Vector3(0, speed, 0);
        transform.position += velocity * Time.deltaTime;
    }
    void OneMove()
    {
        two = false;

        var vec = (pos[0].transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        Move();

        if (one) state = STATE.TWO;
    }
    void TwoMove()
    {
        three = false;

        var vec = (pos[1].transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        Move();

        if (two) state = STATE.THREE;
    }

    void ThreeMove()
    {
        four = false;

        var vec = (pos[2].transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        Move();

        if (three) state = STATE.FOUR;
    }

    void FourMove()
    {
        one = false;

        var vec = (pos[3].transform.position - transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) - 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        Move();

        if (four) state = STATE.ONE;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Core02Pos01"&&!one)
        {
            one = true;
        }
        if (collision.tag == "Core02Pos02" && !two)
        {
            two = true;
        }
        if (collision.tag == "Core02Pos03" && !three)
        {
            three = true;
        }
        if (collision.tag == "Core02Pos04" && !four)
        {
            four = true;
        }
    }
}
