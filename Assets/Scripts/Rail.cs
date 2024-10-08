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
    private Vector2 movement = new Vector2(0, 50);
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
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            movement.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            movement.x = 1;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            movement.y += 1 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            movement.y -= 1 * Time.deltaTime;
        }
        if (movement.x != 0) {
            _dollyView.Distance += movement.x * movement.y * Time.deltaTime;

            if (_dollyView.Distance < 0) {
                _dollyView.Distance = _totalLength;
            }
            _dollyView.transform.position = GetPosition(_dollyView.Distance % _totalLength);
        }
    }

    private void AutoMovement()
    {
        Vector3 point = Vector3.zero;
        for (int i = 0; i < _nodes.Count; i++) {
            if (i == _nodes.Count - 1 && !IsLooped) {
                break;
            }
            Vector3 temp  = MathUtils.GetNearestPointOnSegment(_nodes[i].position, _nodes[(i + 1) % _nodes.Count].position, _dollyView.Target.transform.position);
            float distanceA = Vector3.Distance(_dollyView.Target.transform.position, temp);
            float distanceB = Vector3.Distance(_dollyView.Target.transform.position, point);
            if (i == 0 || distanceA < distanceB) {
                point = temp;
                _targetedIndex = i;
            }
        }
        TargetPosition = point;
        _dollyView.Distance = GetDistanceOnRail(point);
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
        Transform nextNode = _nodes[(i + 1) % _nodes.Count];
        if (i == _nodes.Count - 1 && !IsLooped) {
            return distance + Vector3.Distance(node.position, position);
        }
        float segmentLength = Vector3.Distance(node.position, nextNode.position);
        if (IsPointOnSegment(node.position, nextNode.position, position)) {
            return distance + Vector3.Distance(node.position, position);
        }
        distance += segmentLength;
    }
    return distance;
}

private bool IsPointOnSegment(Vector3 start, Vector3 end, Vector3 point)
{
    float segmentLength = Vector3.Distance(start, end);
    float startToPoint = Vector3.Distance(start, point);
    float endToPoint = Vector3.Distance(end, point);

    return Mathf.Approximately(segmentLength, startToPoint + endToPoint);
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