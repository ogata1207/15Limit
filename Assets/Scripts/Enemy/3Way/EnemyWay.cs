using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWay : MonoBehaviour
{
    public GameObject enemyBullet;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Enemy3Way>().shotStart)
        {
            GameObject runcherBullet = GameObject.Instantiate(enemyBullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
            runcherBullet.transform.position = transform.position;
        }
    }
}
