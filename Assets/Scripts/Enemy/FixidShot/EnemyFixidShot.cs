using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixidShot : MonoBehaviour
{
    public GameObject enemyBullet;

    [Range(1.0f,15.0f)]
    public float shotSpeed;
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<EnemyFixid>().shotStart)
        {
            SoundManager.GetInstance.PlaySE("enemy_shot");
            GameObject runcherBullet = Instantiate(enemyBullet);
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }
    }
}
