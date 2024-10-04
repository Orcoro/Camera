using UnityEngine;

public class FreeFollowView : AView
{
    private Vector3 _rotation;
    private Vector3[] _value = new Vector3[3];
    private float _yaw = 0f;
    private float _yawSpeed = 0.5f;
    private GameObject _target;
    private Curve curve;
    private Vector3 _curvePosition;
    private float _curveSpeed;
    private float _fieldOfView;

    public GameObject Target;

    public float GetPitch(PositionType positionType)
    {
        return _value[(int)positionType].x;
    }

    public float GetFov(PositionType positionType)
    {
        return _value[(int)positionType].y;
    }

    public float GetRoll(PositionType positionType)
    {
        return _value[(int)positionType].z;
    }

    public void SetPitch(PositionType positionType, float pitch)
    {
        _value[(int)positionType].x = pitch;
    }

    public void SetFov(PositionType positionType, float fov)
    {
        _value[(int)positionType].y = fov;
    }

    public void SetRoll(PositionType positionType, float roll)
    {
        _value[(int)positionType].z = roll;
    }

    public override CameraConfiguration GetConfiguration()
    {
        _rotation = transform.eulerAngles;
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        float pitch = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        _rotation = new Vector3(pitch, yaw, 0f);
        return new CameraConfiguration
        {
            Position = transform.position,
            Rotation = _rotation,
            Offset = Vector3.zero,
            FieldOfView = _fieldOfView
        };
    }

}

public enum PositionType
{
    Bottom = 0,
    Middle = 1,
    Top = 2
}