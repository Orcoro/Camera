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
        return new CameraConfiguration
        {
            Position = transform.position,
            Offset = Vector3.zero,
        };
    }
}