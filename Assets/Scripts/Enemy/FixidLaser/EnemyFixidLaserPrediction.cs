using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixidLaserPrediction : MonoBehaviour
{
    private SpriteRenderer sp;
    private GameObject player;
    private int flashTimer;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.enabled = false;
        flashTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<EnemyFixidLaser>().render)
        {
            sp.color = new Color(255.0f, 0.0f, 0.0f, 0.5f);
            sp.enabled = true;
        }
        if (FindObjectOfType<EnemyFixidLaser>().flash)
        {
            flashTimer++;
            if (flashTimer==1)
            {
                sp.color = new Color(255.0f,0.0f,0.0f,0.5f);
            }
            if (flashTimer == 5)
            {
                sp.color = new Color(255.0f, 255.0f, 255.0f,0.5f);
                flashTimer = 0;
            }
        }
        if (FindObjectOfType<EnemyFixidLaser>().shotStart)
        {
            sp.enabled = false;
        }
    }
}
