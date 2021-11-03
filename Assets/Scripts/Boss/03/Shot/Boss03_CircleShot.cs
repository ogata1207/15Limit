using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03_CircleShot : MonoBehaviour
{
    public GameObject bullet;

    [Range(1.0f, 15.0f)]
    public float shotSpeed;
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Boss03BPattern>().circleShotFlg && Boss03Manager.GetPattern() == 1)
        {
            FindObjectOfType<Boss03BPattern>().circleShotFlg = false;
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }

        if (FindObjectOfType<Boss03CPattern>().circleShotFlg && Boss03Manager.GetPattern() == 2)
        {
            FindObjectOfType<Boss03CPattern>().circleShotFlg = false;
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }

        if (FindObjectOfType<BossFinal>().circleShotFlg && Boss03Manager.GetForm() == 2)
        {
            FindObjectOfType<BossFinal>().circleShotFlg = false;
            SoundManager.GetInstance.PlaySE("EnemyShot");
            GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * shotSpeed;
            runcherBullet.transform.position = transform.position;
        }
    }
}
