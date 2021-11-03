using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationController : OGT_Utility.Singleton<VibrationController>
{
    public void PlayVibration(bool isEnabled, float power)
    {
        var setPower = (isEnabled) ? power : 0.0f;
        XInputDotNetPure.GamePad.SetVibration( 0, setPower, setPower );
    }
}
