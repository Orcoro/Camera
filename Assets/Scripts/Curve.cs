using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve
{
    [SerializeField] private Vector3 _a;
    [SerializeField] private Vector3 _b;
    [SerializeField] private Vector3 _c;
    [SerializeField] private Vector3 _d;
    private float _angle = 0f;

    public float Angle { get => _angle; set => _angle = value; }

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(_a, _b, _c, _d, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4  localToWorldMatrix)
    {
        Quaternion rotation = Quaternion.Euler(0, _angle, 0);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        localToWorldMatrix = localToWorldMatrix * rotationMatrix;
        Vector3 position = localToWorldMatrix.MultiplyPoint3x4(GetPosition(t));

        return position;
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
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint3x4(_a), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint3x4(_b), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint3x4(_c), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint3x4(_d), 0.1f);
    }
}