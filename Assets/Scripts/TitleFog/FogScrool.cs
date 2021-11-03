using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScrool : MonoBehaviour
{
    float x = 0.01f;
    public float DeletePosX;
    public float NewX, NewY;

    void Update()
    {
        transform.Translate(-x, 0.0f, 0);
        if (transform.position.x < DeletePosX)
        {
            transform.position = new Vector3(NewX, NewY, 0.0f);
        }
    }
}
