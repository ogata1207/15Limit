using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeShots03 : MonoBehaviour
{
    public GameObject enemyBullet;
    public float shotSpeed;

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Enemy3Shots>().threeShotStart)
        {
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = Instantiate(enemyBullet);
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }
    }
}
