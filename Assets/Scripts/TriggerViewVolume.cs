using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerViewVolume : AViewVolume
{
    [SerializeField] private GameObject _target;
    [SerializeField] private string _tag;
    [SerializeField] private LayerMask _layer;
    private Collider _collider;

    protected override void Init()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null) {
            Debug.LogWarning("TriggerViewVolume must have a Collider component");
            _collider = gameObject.AddComponent<BoxCollider>();
        }
        _collider.isTrigger = true;
        _target = CameraController.Instance.gameObject;
    } 

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