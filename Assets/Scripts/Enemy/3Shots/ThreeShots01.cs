using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeShots01 : MonoBehaviour
{
    public GameObject enemyBullet;
    public float shotSpeed;

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Enemy3Shots>().oneShotStart)
        {
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject a = GameObject.Instantiate(enemyBullet) as GameObject;
            a.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            a.transform.position = transform.position;
        }
    }
}
