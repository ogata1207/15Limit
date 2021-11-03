using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveInvincibleExtension : PassiveItem
{
    public float invincibleRate;

    public override void UniqueEffect()
    {
        PlayerController.currentInvincibleTime = PlayerStatus.GetInstance.invincibleTime * invincibleRate;

    }

}
