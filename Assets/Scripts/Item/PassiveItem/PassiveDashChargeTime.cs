using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveDashChargeTime : PassiveItem
{
    public float chargeTimeRate;

    public override void PassiveDashCharge()
    {
        base.PassiveDashCharge();

        PlayerController.GetInstance.currentChargeTime = PlayerStatus.GetInstance.dashChargeTime * chargeTimeRate;

    }

}
