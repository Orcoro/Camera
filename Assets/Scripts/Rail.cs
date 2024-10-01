using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private int _targetedIndex = 0;
    private List<Transform> _nodes = new List<Transform>();
    private DollyView _dollyView;
    private float _totalLength;
    private Vector2 movement = Vector2.zero;
    private Vector3 TargetPosition;

    public bool IsLooped;
    public DollyView DollyView { get => _dollyView; set => SetDollyCam(value); }

    private void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child != transform) {
                _nodes.Add(child);
            }
        }
        CalculateTotalLength();
    }

    private void CalculateTotalLength()
    {
        _totalLength = 0;
        for (int i = 0; i < _nodes.Count; i++) {
            Transform node = _nodes[i];
            if (i == _nodes.Count - 1 && !IsLooped) {
                break;
            }
            _totalLength += Vector3.Distance(node.position, _nodes[(i + 1) % _nodes.Count].position);
        }
    }

    private void SetDollyCam(DollyView dollyView)
    {
        _dollyView = dollyView;
        _dollyView.transform.position = GetPosition(_dollyView.Distance);
    }

    private void Update()
    {
        if (_dollyView.IsAuto) {
            AutoMovement();
        } else {
            ManualMovement();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            IsLooped = !IsLooped;
            CalculateTotalLength();
        }
    }

    private void ManualMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            movement.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            movement.x = 1;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            movement.y += 1 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            movement.y -= 1 * Time.deltaTime;
        }
        if (movement.x != 0) {
            _dollyView.Distance += movement.x * movement.y * Time.deltaTime;
            _dollyView.transform.position = GetPosition(_dollyView.Distance);
        }
    }

    private void AutoMovement()
    {
        float totalDistance = 0f;
        int index = 0;
        for (int i = 0; i < _nodes.Count; i++) {
            if (i == _nodes.Count - 1 && !IsLooped) {
                break;
            }
            float distanceA = Vector3.Distance(_nodes[i].position, _dollyView.Target.transform.position);
            float distanceB = Vector3.Distance(_nodes[(i + 1) % _nodes.Count].position, _dollyView.Target.transform.position);
            if (distanceA + distanceB <= totalDistance || totalDistance == 0f) {
                totalDistance = distanceA + distanceB;
                index = i;
                _targetedIndex = i;
                Debug.Log(index);
            }
        }
        Vector3 point = MathUtils.GetNearestPointOnSegment(_nodes[index].position, _nodes[(index + 1) % _nodes.Count].position, _dollyView.Target.transform.position);
        TargetPosition = point;
        float distance = GetDistanceOnRail(point);
        _dollyView.Distance = distance;
        _dollyView.transform.position = GetPosition(_dollyView.Distance);
    }

    public float GetTotalLength()
    {
        return _totalLength;
    }

    public float GetDistanceOnRail(Vector3 position)
    {
        float distance = 0;
        for (int i = 0; i < _nodes.Count; i++) {
            Transform node = _nodes[i];
            if (i == _nodes.Count - 1 && !IsLooped) {
                return distance + Vector3.Distance(node.position, position);
            }
            float length = Vector3.Distance(node.position, _nodes[(i + 1) % _nodes.Count].position);
            if (Vector3.Distance(node.position, position) <= length && Vector3.Distance(_nodes[(i + 1) % _nodes.Count].position, position) <= length) {
                return distance + Vector3.Distance(node.position, position);
            }
            distance += length;
        }
        return distance;
    }


    public Vector3 GetPosition(float distance)
    {
        for (int i = 0; i < _nodes.Count; i++) {
            Transform node = _nodes[i];
            if (i == _nodes.Count - 1 && !IsLooped) {
                break;
            }
            float length = Vector3.Distance(node.position, _nodes[(i + 1) % _nodes.Count].position);
            if (distance < length) {
                return Vector3.Lerp(node.position, _nodes[(i + 1) % _nodes.Count].position, distance / length);
            }
            distance -= length;
        }
        return _nodes[0].position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < _nodes.Count; i++) {
            Transform node = _nodes[i];
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(node.position, 0.5f);
            if (i == _nodes.Count - 1 && !IsLooped) {
                break;
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(node.position, _nodes[(i + 1) % _nodes.Count].position);
        }
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(TargetPosition, 1f);
        if (_dollyView == null) {
            return;
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_dollyView.Target.transform.position, _nodes[_targetedIndex].position);
        Gizmos.DrawLine(_dollyView.Target.transform.position, _nodes[(_targetedIndex + 1) % _nodes.Count].position);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_dollyView.Target.transform.position, TargetPosition);
    }
}