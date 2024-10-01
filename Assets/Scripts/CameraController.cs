using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float _computeTimer = 10f;
    private float _timer = 0f;
    public static CameraController Instance;
    public Camera Camera;
    [SerializeField] private CameraConfiguration _configuration;
    private CameraConfiguration _targetConfiguration;
    private List<AView> _activeViews = new List<AView>();
    private float _speed = 1f;

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
        }
        if (Camera == null) {
            Camera = Camera.main;
        }
        ApplyConfiguration();
    }

    private void Update()
    {
        _targetConfiguration = ComputeAverage();
        if (_timer >= _computeTimer && _targetConfiguration.IsEqual(_configuration)) {
            _timer = 0f;
            ApplyConfiguration();
        } else if (!_targetConfiguration.IsEqual(_configuration)) {
            _timer = 0f;
            SmoothCamera();
        } else
            SmoothCamera();
    }

    private void SmoothCamera()
    {
        _timer += Time.deltaTime;
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, _targetConfiguration.GetPosition(), _speed * Time.deltaTime);
        Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, _targetConfiguration.GetRotation(), _speed * Time.deltaTime);
        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, _targetConfiguration.FieldOfView, _speed * Time.deltaTime);
        _configuration = _targetConfiguration;
    }

    private void ApplyConfiguration()
    {
        Camera.transform.position = _configuration.GetPosition();
        Camera.transform.rotation = _configuration.GetRotation();
        Camera.fieldOfView = _configuration.FieldOfView;
    }

    private CameraConfiguration ComputeAverage()
    {
        Vector3 position = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        Vector2 sum = Vector2.zero;
        float fieldOfView = 0f;
        float weight = 0f;
        foreach (AView view in _activeViews) {
            CameraConfiguration configuration = view.GetConfiguration();
            position += configuration.Position * view.Weight;
            rotation += configuration.Rotation * view.Weight;
            sum += new Vector2(Mathf.Cos(configuration.Yaw * Mathf.Deg2Rad),
                Mathf.Sin(configuration.Yaw * Mathf.Deg2Rad)) * view.Weight;
            fieldOfView += configuration.FieldOfView * view.Weight;
            weight += view.Weight;
        }
        if (weight > 0) {
            position /= weight;
            rotation /= weight;
            rotation.y = Vector2.SignedAngle(Vector2.right, sum);
            fieldOfView /= weight;
        }
        return new CameraConfiguration {
            Position = position,
            Rotation = rotation,
            FieldOfView = fieldOfView,
        };
    }

    public void AddView(AView view)
    {
        _activeViews.Add(view);
        _configuration = ComputeAverage();
        _targetConfiguration = ComputeAverage();
    }

    public void RemoveView(AView view)
    {
        _activeViews.Remove(view);
        _configuration = ComputeAverage();
        _targetConfiguration = ComputeAverage();
    }

    private void OnDrawGizmos()
    {
        _configuration.DrawGizmos(Color.red);
        for (int i = 0; i < _activeViews.Count; i++) {
            CameraConfiguration configuration = _activeViews[i].GetConfiguration();
            configuration.DrawGizmos(Color.green);
            // Draw a line between the camera and the view
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_configuration.GetPosition(), configuration.GetPosition());
        }
    }
}