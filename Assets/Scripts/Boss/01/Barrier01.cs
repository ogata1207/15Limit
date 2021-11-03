using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier01 : MonoBehaviour
{
    private GameObject core01;
    private SpriteRenderer sp;
    private bool dead;
    private float alpha;
    // Start is called before the first frame update
    void Start()
    {
        core01 = GameObject.Find("Core01");
        sp = GetComponent<SpriteRenderer>();
        dead = false;
        alpha = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (core01 == null) dead = true;
        if (dead) alpha = 0;
        if (alpha <= 0.0f) Destroy(gameObject);
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
    }
}
