using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03CenterCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Center")
        {
            Boss03Manager.SetCenterCol(true);
        }
    }
}
