using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBullet : MonoBehaviour
{

    static private float speed;
    private Vector3 movePower;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        //移動速度の取得
        if(speed <= 0)
        {
            speed = PlayerStatus.GetInstance.shotSpeed;
        }

        movePower = transform.up;
        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position + (movePower * speed);
        transform.position = pos;
        var length = Vector3.Distance(transform.position, startPosition);

        if(length > 50.0f)
        {
            Destroy(gameObject);
        }

    }

    //衝突判定
    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }


}
