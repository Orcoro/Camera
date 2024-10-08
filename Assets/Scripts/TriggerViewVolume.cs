using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerViewVolume : AViewVolume
{
    private GameObject _target;
    private string _tag;
    private int _layer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target || other.gameObject.CompareTag(_tag) || (1 << other.gameObject.layer & _layer) != 0) {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _target || other.gameObject.CompareTag(_tag) || (1 << other.gameObject.layer & _layer) != 0) {
            SetActive(false);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label($"TriggerViewVolume {_uID}");
    }
}