using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core03Laser : MonoBehaviour
{
    public float speed;
    private Vector3 scale;
    private int soundTimer;
    void Start()
    {
        scale = transform.localScale;
        soundTimer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Core03>().shotStart)
        {
            scale.x += speed;
            soundTimer++;
        }
        else
        {
            scale.x = 0.0f;
            soundTimer = 0;
        }
        transform.localScale = scale;

        if (soundTimer == 1)
        {
            SoundManager.GetInstance.PlaySE("EnemyLaser");
        }
    }
}
