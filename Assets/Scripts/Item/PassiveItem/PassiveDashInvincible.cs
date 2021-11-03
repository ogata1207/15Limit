using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveDashInvincible : PassiveItem
{
    public float invincibleTime;

    public override void PassiveDash()
    {
        base.PassiveDash();
        PlayerController.GetInstance.Invincible();
    }
}
