using UnityEngine;

public abstract class AView : MonoBehaviour
{
    [Min(0.01f)]
    public float Weight;
    public bool IsActiveOnStart;

    private void Start()
    {
        if (IsActiveOnStart) {
            SetActive(IsActiveOnStart);
        }
        Init();
    }

    protected virtual void Init() { }
    public virtual CameraConfiguration GetConfiguration() { return new CameraConfiguration(); }
    public void SetActive(bool isActive)
    {
        if (isActive) {
            CameraController.Instance.AddView(this);
        } else {
            CameraController.Instance.RemoveView(this);
        }
    }

}