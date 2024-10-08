using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : AViewVolume
{
    public static ViewVolumeBlender Instance { get; private set; }
    private List<AViewVolume> _activeViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> _volumesPerViews = new Dictionary<AView, List<AViewVolume>>();
    private float _totalWeight = 0;

    protected override void Init()
    {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        if (_activeViewVolumes.Count == 0) {
            return;
        }
        _activeViewVolumes.Sort((a, b) =>
        {
            int priorityComparison = a.Priority.CompareTo(b.Priority);
            if (priorityComparison == 0)
            {
                return a.UID.CompareTo(b.UID);
            }
            return priorityComparison;
        });
        foreach (AViewVolume volume in _activeViewVolumes) {
            float weight = volume.ComputeSelfWeight();
            float remainingWeight = 1.0f - weight;
            foreach (AView view in _volumesPerViews.Keys) {
                view.Weight *= remainingWeight;
            }
            volume.View.Weight += weight;
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        if (!_volumesPerViews.ContainsKey(volume.View)) {
            _volumesPerViews[volume.View] = new List<AViewVolume>();
            volume.View.Weight = 0f;
            volume.View.SetActive(true);
        } else {
            _volumesPerViews[volume.View].Add(volume);
            _activeViewVolumes.Add(volume);
        }
        _totalWeight += volume.ComputeSelfWeight();
    }

    public void RemoveVolume(AViewVolume volume)
    {
        if (_volumesPerViews.ContainsKey(volume.View)) {
            _volumesPerViews[volume.View].Remove(volume);
            if (_volumesPerViews[volume.View].Count == 0) {
                _volumesPerViews.Remove(volume.View);
                volume.View.SetActive(false);
            } else {
                _activeViewVolumes.Remove(volume);
            }
            _totalWeight -= volume.ComputeSelfWeight();
        }
    }
}