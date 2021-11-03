using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine02 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Boss03Manager.GetChildeDead02())
        {
            Destroy(gameObject);
        }
    }
}
