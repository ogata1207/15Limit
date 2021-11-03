using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAddRestartCountItem : PassiveItem
{
    public int addCount;
    public override void UniqueEffect()
    {
        PlayerStatus.GetInstance.continueCount += addCount;
        Debug.Log("Continue Count : " + PlayerStatus.GetInstance.continueCount);
    }

}
