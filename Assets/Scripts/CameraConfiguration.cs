using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraConfiguration
{
    // private float _debugSize = 10f;

    private Vector3 _rotation;
    public Vector3 Position;
    public Vector3 Offset;
    public float FieldOfView;

    public Vector3 Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
    public float Pitch => _rotation.x;
    public float Yaw => _rotation.y;
    public float Roll => _rotation.z;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(_rotation);
    }

    public Vector3 GetPosition()
    {
        return Position + Offset;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(Position, 0.25f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(Position, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), new Vector3(10f, 10f, 10f));
        Gizmos.DrawFrustum(Vector3.zero, FieldOfView, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
