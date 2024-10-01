using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve
{
    private Vector3 _a;
    private Vector3 _b;
    private Vector3 _c;
    private Vector3 _d;

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