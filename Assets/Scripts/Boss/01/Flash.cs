using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private SpriteRenderer sp;
    public GameObject[] core;
    private float alpha;
    public float num;
    private bool minusF01, minusF02, minusF03;
    enum STATE
    {
        CORE_01,
        CORE_02,
        CORE_03,
        NONE
    }
    STATE state;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        state = STATE.CORE_01;
        alpha = 0.0f;
        minusF01 = false;
        minusF02 = false;
        minusF03 = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.CORE_01:
                Core01Delete();
                break;
            case STATE.CORE_02:
                Core02Delete();
                break;
            case STATE.CORE_03:
                Core03Delete();
                break;
        }
        Debug.Log(alpha);
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
    }
    void Core01Delete()
    {
        if (core[0] == null)
        {
            if (!minusF01) alpha += num;
            if (alpha >= 1.0f)
            {
                minusF01 = true;
            }
        }
        if (minusF01)
        {
            alpha -= num;
            if (alpha <= 0.0f)
            {
                alpha = 0.0f;
                state = STATE.CORE_02;
            }
        }
    }
    void Core02Delete()
    {
        if (core[1] == null)
        {
            if (!minusF02) alpha += num;
            if (alpha >= 1.0f)
            {
                minusF02 = true;
            }
        }
        if (minusF02)
        {
            alpha -= num;
            if (alpha <= 0.0f)
            {
                alpha = 0.0f;
                state = STATE.CORE_03;
            }
        }

    }
    void Core03Delete()
    {
        if (core[2] == null)
        {
            if (!minusF03) alpha += num;
            if (alpha >= 1.0f)
            {
                minusF03 = true;
            }
        }
        if (minusF03)
        {
            alpha -= num;
            if (alpha <= 0.0f)
            {
                alpha = 0.0f;
                state = STATE.NONE;
            }
        }
    }

}
