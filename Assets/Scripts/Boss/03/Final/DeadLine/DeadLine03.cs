using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine03 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Boss03Manager.GetChildeDead03())
        {
            Destroy(gameObject);
        }
    }
}
