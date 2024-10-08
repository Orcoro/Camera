using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    private GameObject _target;
    private float _outerRadius;
    private float _innerRadius;
    private float _distance;

    public GameObject Target { get => _target; }
    public float OuterRadius { get => _outerRadius; }
    public float InnerRadius { get => _innerRadius; }

    private void Update()
    {
        if (_target == null) {
            return;
        }
        _distance = Vector3.Distance(_target.transform.position, transform.position);
        if (_distance <= _innerRadius && !_isActive) {
            SetActive(true);
        } else if (_distance > _outerRadius && _isActive) {
            SetActive(false);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label($"SphereViewVolume {_uID}");
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _innerRadius);
    }
}