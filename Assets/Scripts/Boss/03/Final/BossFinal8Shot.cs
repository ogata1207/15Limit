using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinal8Shot : MonoBehaviour
{
    public GameObject bullet;
    public float speed;
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<BossFinal>().shotFlg)
        {
            GameObject runcherBullet = GameObject.Instantiate(bullet) as GameObject;
            runcherBullet.GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
            runcherBullet.transform.position = transform.position;
        }
    }
}
