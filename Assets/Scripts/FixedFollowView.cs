using UnityEngine;

public class FixedFollowView : AView
{
    private float _yawOffsetMax = 45f;
    private float _pitchOffsetMax = 45f;
    private Vector3 _rotation;
    public float FieldOfView;
    public GameObject Target;
    public GameObject CentralPoint;
    public bool _clamped = true;

    private float Pitch { get => _rotation.x; set => _rotation.x = value; }
    private float Yaw { get => _rotation.y; set => _rotation.y = value; }
    public float Roll => _rotation.z;

    public override CameraConfiguration GetConfiguration()
    {
        _rotation = transform.eulerAngles;
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        float pitch = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (_clamped)
        {
            Vector3 directionToCentralPoint = (CentralPoint.transform.position - transform.position).normalized;
            float pitchToCentralPoint = -Mathf.Asin(directionToCentralPoint.y) * Mathf.Rad2Deg;
            float yawToCentralPoint = Mathf.Atan2(directionToCentralPoint.x, directionToCentralPoint.z) * Mathf.Rad2Deg;
            float pitchOffset = pitch - pitchToCentralPoint;
            float yawOffset = yaw - yawToCentralPoint;
            pitchOffset = Mathf.Clamp(pitchOffset, -_pitchOffsetMax, _pitchOffsetMax);
            pitch = pitchToCentralPoint + pitchOffset;
            yawOffset = (yawOffset > 180f) ? yawOffset - 360f : (yawOffset < -180f) ? yawOffset + 360f : yawOffset;
            yawOffset = Mathf.Clamp(yawOffset, -_yawOffsetMax, _yawOffsetMax);
            yaw = yawOffset + yawToCentralPoint;
        }
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