using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private List<Transform> _nodes = new List<Transform>();
    private float _totalLength;

    public bool IsLooped;

    private void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child != transform) {
                _nodes.Add(child);
            }
        }
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
            if (i == _nodes.Count - 1) {
                return distance + Vector3.Distance(node.position, position);
            }
            float length = Vector3.Distance(node.position, _nodes[i + 1].position);
            if (Vector3.Distance(node.position, position) < length) {
                return distance + Vector3.Distance(node.position, position);
            }
            distance += length;
        }
        return distance;
    }

    public Vector3 GetPosition(float distance)
    {
        if (IsLooped) {
            distance = Mathf.Repeat(distance, _totalLength);
        } else {
            distance = Mathf.Clamp(distance, 0, _totalLength);
        }
        for (int i = 0; i < _nodes.Count; i++) {
            Transform node = _nodes[i];
            if (i == _nodes.Count - 1) {
                return node.position;
            }
            float length = Vector3.Distance(node.position, _nodes[i + 1].position);
            if (distance < length) {
                return Vector3.Lerp(node.position, _nodes[i + 1].position, distance / length);
            }
            distance -= length;
        }
        return Vector3.zero;
    }
}