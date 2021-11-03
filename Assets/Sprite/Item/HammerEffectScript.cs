using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEffectScript : MonoBehaviour
{
    private Vector3 scale;
    private SpriteRenderer sp;
    private float alpha;
    public float num;
    private GameObject player;
    private bool a, b;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
        sp = GetComponent<SpriteRenderer>();
        alpha = 1.0f;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        scale.x += num;
        scale.y += num;

        if (scale.x == (transform.localScale.x + 2.0f))
        {
           scale.x = transform.localScale.x;
        }
        if (scale.y == (transform.localScale.y + 2.0f))
        {
            scale.y = transform.localScale.y;
        }
        transform.localScale = scale;
        transform.position = player.transform.position;

        if (alpha >= 1.0f)
        {
            b = true;
        }
        if(b) alpha -= 0.1f;
        if (alpha <= 0.0f)
        {
            b = false;
            a = true;
        }
        if (a) alpha += 0.1f;
        if (alpha >= 1.0f)
        {
            a = false;
            b = true;
        }

        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
    }
}
