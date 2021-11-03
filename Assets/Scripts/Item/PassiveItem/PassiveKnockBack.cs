using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveKnockBack : PassiveItem
{
    public GameObject knockbackObject;
    public override void UniqueEffect()
    {
        PlayerController.isKnockBack = true;
    }
    
}
