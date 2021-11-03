using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBreakFontTracking : MonoBehaviour
{
    public Transform target;
    private Vector3 pos;
    public GameObject core;
    public GameObject[] obj;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        for (int i = 0; i < 2; i++)
        {
            obj[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (core == null)
        {
            for (int i = 0; i < 2; i++)
            {
                obj[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        if (target == null) Destroy(gameObject);
        pos.y = target.position.y + 2.0f;
        pos.x = target.position.x;
        transform.position = pos;
    }
}
