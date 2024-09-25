using UnityEngine;

public class FixedView : AView
{
    private Vector3 _rotation;
    public float FieldOfView;

    public float Pitch => _rotation.x;
    public float Yaw => _rotation.y;
    public float Roll => _rotation.z;

    public override CameraConfiguration GetConfiguration()
    {
        _rotation = transform.eulerAngles;
        return new CameraConfiguration
        {
            Position = transform.position,
            Rotation = _rotation,
            Offset = Vector3.zero,
            FieldOfView = FieldOfView
        };
    }
}