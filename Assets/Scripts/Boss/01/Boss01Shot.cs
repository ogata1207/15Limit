using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Shot : MonoBehaviour
{
    public GameObject enemyBullet;

    [Range(1.0f, 15.0f)]
    public float shotSpeed;
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Boss01>().shotStart)
        {
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = Instantiate(enemyBullet);
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }
    }
}
