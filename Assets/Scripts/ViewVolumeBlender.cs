using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : AViewVolume
{
    public static ViewVolumeBlender Instance { get; private set; }
    private List<AViewVolume> _activeViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> _volumesPerViews = new Dictionary<AView, List<AViewVolume>>();

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
        float totalWeight = 0.0f;
        foreach (AViewVolume volume in _activeViewVolumes) {
            totalWeight += volume.ComputeSelfWeight();
            volume.View.Weight = 0f;
        }
        CameraConfiguration configuration = new CameraConfiguration();
        foreach (AViewVolume volume in _activeViewVolumes) {
            float weight = volume.ComputeSelfWeight();
            CameraConfiguration volumeConfiguration = volume.View.GetConfiguration();
            configuration.Position += volumeConfiguration.Position * weight / totalWeight;
            configuration.Rotation += volumeConfiguration.Rotation * weight / totalWeight;
            configuration.Offset += volumeConfiguration.Offset * weight / totalWeight;
            configuration.FieldOfView += volumeConfiguration.FieldOfView * weight / totalWeight;
        }
        CameraController.Instance.SetConfiguration(configuration);
    }

    public void AddVolume(AViewVolume volume)
    {
        if (!_volumesPerViews.ContainsKey(volume.View)) {
            _volumesPerViews[volume.View] = new List<AViewVolume>();
            volume.View.SetActive(true);
        }
        _volumesPerViews[volume.View].Add(volume);
        _activeViewVolumes.Add(volume);
    }

    public void RemoveVolume(AViewVolume volume)
    {
        if (_volumesPerViews.ContainsKey(volume.View)) {
            _volumesPerViews[volume.View].Remove(volume);
            if (_volumesPerViews[volume.View].Count == 0) {
                _volumesPerViews.Remove(volume.View);
                volume.View.SetActive(false);
            }
            _activeViewVolumes.Remove(volume);
        }
    }
}