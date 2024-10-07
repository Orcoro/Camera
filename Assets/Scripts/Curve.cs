using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve
{
    private List<Vector3> _points = new List<Vector3>();
    [SerializeField] private Vector3 _a;
    [SerializeField] private Vector3 _b;
    [SerializeField] private Vector3 _c;
    [SerializeField] private Vector3 _d;

    public void AddPoint(Vector3 point)
    {
        _points.Add(point);
        if (_points.Count == 4) {
            _a = _points[0];
            _b = _points[1];
            _c = _points[2];
            _d = _points[3];
        }
    }

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(_a, _b, _c, _d, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4  localToWorldMatrix)
    {
        return  localToWorldMatrix.MultiplyPoint3x4(GetPosition(t));
    }

    public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;
        Vector3 lastPos = GetPosition(0, localToWorldMatrix);
        for (int i = 1; i <= 20; i++) {
            Vector3 newPos = GetPosition(i / 20f, localToWorldMatrix);
            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }
    }
}