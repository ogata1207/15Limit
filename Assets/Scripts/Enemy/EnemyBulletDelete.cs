using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDelete : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //カメラ外に出た
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //                              ↓障害物
        if (collision.tag == "Player"|| collision.tag=="")
        {
            Destroy(gameObject);
        }
    }
}
