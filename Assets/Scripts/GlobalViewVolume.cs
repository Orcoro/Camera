using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalViewVolumeVolume : AViewVolume
{
    private void Start()
    {
        SetActive(true);
    }

    private void OnGUI()
    {
        GUILayout.Label($"GlobalViewVolume {_uID}");
    }
}