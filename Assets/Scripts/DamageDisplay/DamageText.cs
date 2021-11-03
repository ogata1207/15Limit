using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : OGT_Utility.Singleton<DamageText>
{
    public GameObject obj;
    public Vector3 drawPosition;

    public void DrawDamage(Vector3 position, string text)
    {
        var newObj = Instantiate(obj, transform);
        newObj.GetComponent<DamageDisplay>().DrawDamage(position + drawPosition, text);
    }
}
