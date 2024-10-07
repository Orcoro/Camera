using UnityEngine;

public class FreeFollowView : AView
{
    private Vector3 _rotation;
    [SerializeField] private Vector3[] _value = new Vector3[3];
    private float _yaw = 60f;
    private float _yawSpeed = 0.5f;
    private GameObject _target;
    [SerializeField] private Curve _curve;
    private float _curvePosition;
    private float _curveRotation;
    private float _curveSpeed = 0.2f;
    public GameObject Target;

    public float GetPitch(PositionType positionType)
    {
        return _value[(int)positionType].x;
    }
    public float GetPitch(float t)
    {
        float result = 0f;

        for (int i = 0; i < _value.Length - 1; i++) {
            float a = _value[i].x;
            float b = _value[(i + 1)].x;
            if ((i == 0 || t >= (float)i / (_value.Length - 1))  && t <= ((float)i + 1f) / (_value.Length - 1)) {
                result = Mathf.Lerp(a, b, (t - (float)i / (_value.Length - 1)) * (_value.Length - 1));
            }
        }
        return result;

    }

    public float GetRoll(PositionType positionType)
    {
        return _value[(int)positionType].y;
    }

    public float GetRoll(float t)
    {
        float result = 0f;
        
        for (int i = 0; i < _value.Length - 1; i++) {
            float a = _value[i].y;
            float b = _value[(i + 1)].y;
            if ((i == 0 || t >= (float)i / (_value.Length - 1)) && t <= ((float)i + 1) / (_value.Length - 1)) {
                result = Mathf.Lerp(a, b, (t - (float)i / (_value.Length - 1)) * (_value.Length - 1));
            }
        }
        return result;
    }

    public float GetFov(PositionType positionType)
    {
        return _value[(int)positionType].z;
    }

    public float GetFov(float t)
    {
        float result = 0f;

        for (int i = 0; i < _value.Length - 1; i++) {
            float a = _value[i].z;
            float b = _value[(i + 1)].z;
            if ((i == 0 || t >= (float)i / (_value.Length - 1)) && t <= ((float)i + 1) / (_value.Length - 1)) {
                result = Mathf.Lerp(a, b, (t - (float)i / (_value.Length - 1)) * (_value.Length - 1));
            }
        }

        return result;
    }

    public void SetPitch(PositionType positionType, float pitch)
    {
        _value[(int)positionType].x = pitch;
    }

    public void SetRoll(PositionType positionType, float roll)
    {
        _value[(int)positionType].y = roll;
    }

    public void SetFov(PositionType positionType, float fov)
    {
        _value[(int)positionType].z = fov;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            _curvePosition += _curveSpeed * Time.deltaTime;
        } else if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            _curvePosition -= _curveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            _curveRotation -= _curveSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            _curveRotation += _curveSpeed * Time.deltaTime;
        }
        _curvePosition = Mathf.Clamp01(_curvePosition);
        _curveRotation = Mathf.Clamp01(_curveRotation);
        _curve.Angle = _curveRotation * 360f;
    }
    
    public override CameraConfiguration GetConfiguration()
    {
        transform.position = _curve.GetPosition(_curvePosition, Target.transform.localToWorldMatrix);
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        _yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        _rotation = new Vector3(GetPitch(_curvePosition), _yaw, GetRoll(_curvePosition));
        return new CameraConfiguration
        {
            Position = transform.position,
            Rotation = _rotation,
            Offset = Vector3.zero,
            FieldOfView = GetFov(_curvePosition)
        };
    }

    void OnDrawGizmos()
    {
        _curve.DrawGizmo(Color.red, Target.transform.localToWorldMatrix);
    }

}

public enum PositionType
{
    Bottom = 0,
    Middle = 1,
    Top = 2
}