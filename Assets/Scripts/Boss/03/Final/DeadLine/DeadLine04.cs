using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine04 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Boss03Manager.GetChildeDead04())
        {
            Destroy(gameObject);
        }
    }
}
