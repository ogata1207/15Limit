using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03_4Shot : MonoBehaviour
{
    public GameObject bullet;

    [Range(1.0f, 15.0f)]
    public float shotSpeed;
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Boss03APattern>().shotFlg)
        {
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }

        if (FindObjectOfType<Boss03BPattern>().shotFlg)
        {
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }

        if (FindObjectOfType<Boss03CPattern>().shotFlg)
        {
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }
    }
}
