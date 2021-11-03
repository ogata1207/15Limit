using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03TacklePosActive : MonoBehaviour
{
    private BoxCollider2D bc2d;
    // Start is called before the first frame update
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        bc2d.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Boss03Manager.GetMode()==1|| Boss03Manager.GetMode() == 2 || Boss03Manager.GetMode() == 3)
        {
            bc2d.enabled = true;
        }
        else
        {
            bc2d.enabled = false;
        }
    }
}
