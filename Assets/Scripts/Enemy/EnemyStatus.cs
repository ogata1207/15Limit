using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int hp;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (hp <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapos")
        {
            hp--;
            GameObject ef = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(ef, 0.15f);
        }
    }
}
