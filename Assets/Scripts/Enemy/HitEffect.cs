using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private Vector3 scale;
    private float alpha;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
        alpha = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        scale.x += 1.5f;
        scale.y += 1.5f;
        alpha -= 0.01f;
        transform.localScale = scale;
        this.GetComponent<SpriteRenderer>().color = new Color(0.0f, 255.0f, 0.0f, alpha);
    }
}
