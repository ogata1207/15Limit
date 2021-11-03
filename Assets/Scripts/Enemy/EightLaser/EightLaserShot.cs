using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightLaserShot : MonoBehaviour
{
    public float speed;
    private Vector3 scale;
    void Start()
    {
        scale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<EnemyEightLaser>().shotStart)
        {
            scale.x += speed;
        }
        else
        {
            scale.x = 0.0f;
        }
        transform.localScale = scale;
    }
}
