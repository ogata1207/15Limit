using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03WarningMidle : MonoBehaviour
{
    SpriteRenderer sp;
    private Vector3 scale;
    private Vector3 save;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.enabled = false;
        scale = transform.localScale;
        save = scale;

        scale.y = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss03Manager.GetDenPos() == 3)
        {
            scale.y += 0.15f;
            if (scale.y >= save.y)
            {
                scale.y = save.y;
            }
            sp.enabled = true;
        }
        if (Boss03Manager.GetDenPos() == 0)
        {
            scale.y = 0.0f;

            sp.enabled = false;
        }
        transform.localScale = scale;

    }
}
