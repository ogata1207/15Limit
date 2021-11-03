using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huwahuwa : MonoBehaviour
{

    public float hurihaba;
    public float speed;
    float forcus;
    void Start()
    {
        forcus = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time*speed, hurihaba)+forcus, transform.position.z);
    }
}

