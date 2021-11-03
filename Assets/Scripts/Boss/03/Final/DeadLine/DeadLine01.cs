using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine01 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Boss03Manager.GetChildeDead01())
        {
            Destroy(gameObject);
        }
    }
}
