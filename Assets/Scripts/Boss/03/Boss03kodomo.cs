using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03kodomo : MonoBehaviour
{
    private CircleCollider2D cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss03Manager.GetDeadPerformance())
        {
            cc.enabled = false;
        }

    }
}
