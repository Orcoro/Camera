using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Camera Camera;
    private CameraConfiguration _configuration;
    private List<AView> _activeViews = new List<AView>();

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
        _configuration.DrawGizmos(Color.green);
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
    }

    public void RemoveView(AView view)
    {
        _activeViews.Remove(view);
        _configuration = ComputeAverage();
    }
}