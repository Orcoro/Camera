using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _outerRadius;
    [SerializeField] private float _innerRadius;
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
        if (_distance <= _outerRadius && !_isActive) {
            SetActive(true);
        } else if (_distance > _outerRadius && _isActive) {
            SetActive(false);
        }
    }

    public override float ComputeSelfWeight()
    {
        if (_distance <= _innerRadius) {
            return base.ComputeSelfWeight();
        }
        if (_distance >= _outerRadius) {
            return 0;
        }
        return 1.0f - (_distance - _innerRadius) / (_outerRadius - _innerRadius);
    }

    private void OnGUI()
    {
        GUILayout.Label($"SphereViewVolume {_uID}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _innerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _outerRadius);
    }
}