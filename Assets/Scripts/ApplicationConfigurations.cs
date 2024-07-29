using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationConfigurations : MonoBehaviour
{
    private void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld) // Checks if this Android or IOS device
        {
            Application.targetFrameRate = -1; // Turns off fps Limit
            QualitySettings.vSyncCount = 0; // Turns off Vsync
        }
    }
}
