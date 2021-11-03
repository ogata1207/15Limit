using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildeDeadLine : MonoBehaviour
{
    public SpriteRenderer[] sp;
    int max;
    // Start is called before the first frame update
    void Start()
    {
        max = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss03Manager.GetDeadBefor())
        {
            for(int i=0;i< max; i++)
            {
                sp[i].enabled = true;
            }
        }
    }
}
