using UnityEngine;

public class AViewVolume : MonoBehaviour
{
    static private int _nextUID = 0;
    protected int _uID = 0;
    protected int _priority = 0;
    protected AView _view;
    protected bool _isActive = false;
    protected bool _isCutOnSwitch = false;

    public int UID => _uID;
    public int Priority => _priority;
    public AView View => _view;
    public bool IsActive => _isActive;

    private void Awake()
    {
        _uID = _nextUID++;
        Init();
    }

    protected virtual void Init() { }

    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
    }

    protected virtual void SetActive(bool isActive)
    {
        _isActive = isActive;
        if (ViewVolumeBlender.Instance == null) {
            return;
        }
        if (_isActive) {
            ViewVolumeBlender.Instance.AddVolume(this);
        } else {
            ViewVolumeBlender.Instance.RemoveVolume(this);
        }
        if (_isCutOnSwitch) {
            ViewVolumeBlender.Instance.UpdateVolume();
            CameraController.Instance.Cut();
        }
    }
}