using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dekachibi : MonoBehaviour
{
    public float hurihaba;
    public float speed;
    float forcusX;
    float forcusY;
    void Start()
    {
        forcusX = transform.localScale.x;
        forcusY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(Mathf.PingPong(Time.time * speed, hurihaba) + forcusX, Mathf.PingPong(Time.time * speed, hurihaba) + forcusY, transform.position.z);
    }
}
