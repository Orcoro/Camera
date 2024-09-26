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
        Vector3 position = Rail.GetPosition(Distance);
        Vector3 direction = position - Target.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        _rotation = rotation.eulerAngles;
        return new CameraConfiguration
        {
            Position = position,
            Rotation = _rotation,
            Offset = Vector3.zero,
            FieldOfView = FieldOfView
        };
    }
}