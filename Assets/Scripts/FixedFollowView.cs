using UnityEngine;

public class FixedFollowView : AView
{
    private float _yawOffsetMax = 45f;
    private float _pitchOffsetMax = 45f;
    private Vector3 _rotation;
    public float FieldOfView;
    public GameObject Target;
    public Vector3 CentralPoint;

    private float Pitch { get => _rotation.x; set => _rotation.x = value; }
    private float Yaw { get => _rotation.y; set => _rotation.y = value; }
    public float Roll => _rotation.z;

    public override CameraConfiguration GetConfiguration()
    {
        CentralPoint = Target.transform.position - transform.position;
        _rotation = transform.eulerAngles;
        Vector3 direction = CentralPoint.normalized;
        float distance = CentralPoint.magnitude;
        float pitch = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        // pitch = Mathf.Clamp(pitch, -_pitchOffsetMax, _pitchOffsetMax);
        // yaw = Mathf.Clamp(yaw, -_yawOffsetMax, _yawOffsetMax);
        _rotation = new Vector3(pitch, yaw, 0f);
        return new CameraConfiguration
        {
            Position = transform.position,
            Rotation = _rotation,
            Offset = Vector3.zero,
            FieldOfView = FieldOfView
        };
    }
}