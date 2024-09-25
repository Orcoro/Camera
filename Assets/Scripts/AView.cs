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
    }

    public virtual CameraConfiguration GetConfiguration() { return new CameraConfiguration(); }
    public void SetActive(bool isActive) => CameraController.Instance.AddView(this);
}