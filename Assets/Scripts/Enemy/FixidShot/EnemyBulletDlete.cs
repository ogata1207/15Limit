using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDlete : MonoBehaviour
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
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
        // 障害物
        if (collision.tag == "StageObj")
        {
            Destroy(gameObject);
        }
        // 障害物
        if (collision.tag == "StageBlock")
        {
            Destroy(gameObject);
        }
    }
}
