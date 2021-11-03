using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDelete : MonoBehaviour
{
    public GameObject target;
    private SpriteRenderer sp;
    private bool dead;
    private float alpha;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        dead = false;
        alpha = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) dead = true;
        if (dead) alpha -= 0.1f;
        if (dead) Destroy(gameObject);
    }
}
