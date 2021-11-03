using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour
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
        if (FindObjectOfType<EnemyFixidLaser>().shotStart)
        {
            scale.x += speed;
            soundTimer++;
        }
        if (!FindObjectOfType<EnemyFixidLaser>().shotStart)
        {
            scale.x = 0.0f;
            soundTimer = 0;
        }
        transform.localScale = scale;

        if (soundTimer == 1)
        {
            SoundManager.GetInstance.PlaySE("enemy_beam");
        }
    }
}
