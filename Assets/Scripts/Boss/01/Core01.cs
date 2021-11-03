using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core01 : MonoBehaviour
{
    private Vector3 pos;
    public float speed;

    private bool up;
    private bool down;
    public GameObject deadEffect;
    public GameObject coreSoul;
    private bool soulFlg;
    enum STATE
    {
        UP,
        DOWN
    }
    STATE state;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        up = false;
        down = false;
        soulFlg = false;
        state = STATE.UP;
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
        if (TimeManager.GetInstance.timeScale >= 1.0f)
        {
            switch (state)
            {
                case STATE.UP:
                    Up();
                    break;
                case STATE.DOWN:
                    Down();
                    break;
            }
        }
        transform.position = pos;
    }
    void Up()
    {
        down = false;
        pos.y += speed*Time.deltaTime;
        if (up) state = STATE.DOWN;
    }

    void Down()
    {
        up = false;
        pos.y -= speed*Time.deltaTime;
        if (down) state = STATE.UP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CoreUp"&&!up)
        {
            up = true;
        }
        if (collision.tag == "CoreDown"&&!down)
        {
            down = true;
        }
    }
}
