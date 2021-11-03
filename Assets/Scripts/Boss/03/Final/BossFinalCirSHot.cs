using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinalCirSHot : MonoBehaviour
{
    public GameObject bullet;

    [Range(1.0f, 15.0f)]
    public float shotSpeed;
    // Update is called once per frame
    void Update()
    {
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
