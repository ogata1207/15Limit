using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02FrameMoveEffect : MonoBehaviour
{
    private float alpha;
    private Vector3 rotation;
    private Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        alpha = 1.0f;
        rotation = transform.eulerAngles;
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        alpha -= 0.05f;
        scale.x += 0.3f;
        scale.y += 0.3f;
        if (alpha <= 0.0f) Destroy(gameObject);
    }
}
