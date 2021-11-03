using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03TackleLine : MonoBehaviour
{
    private SpriteRenderer sp;
    private int timer;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.enabled = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Boss03Manager.GetTackleLine())
        {
            timer++;
        }
        if (timer >= 1)
        {
            sp.enabled = true;
            if (timer >= 2)
            {
                sp.enabled = false;
                timer = 0;
            }
        }
        if(!Boss03Manager.GetTackleLine())
        {
            sp.enabled = false;
        }
    }
}
