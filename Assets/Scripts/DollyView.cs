using UnityEngine;

public class DollyView : AView
{
    private Vector3 _rotation;
    public float Distance;
    public float FieldOfView;
    public GameObject Target;
    public Rail Rail;
    public float Speed;

    public float Roll { get => _rotation.z; set => _rotation.z = value; }
    public float DistanceOnRail => Rail.GetDistanceOnRail(Target.transform.position);

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
            FieldOfView = FieldOfView
        };
    }
}