using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ColoredBackground : MonoBehaviour {
    [SerializeField] private Color32 LightColor;
    [SerializeField] private Color32 DarkColor;

    private Camera cam;
    public bool IsDark { get; private set; } = false;

    private void Awake() {
        cam = GetComponent<Camera>();
        cam.backgroundColor = LightColor;
    }

    public bool ToggleColor() {
        IsDark = !IsDark;
        if (IsDark) {
            cam.backgroundColor = DarkColor;
        } else
            cam.backgroundColor = LightColor;
        return IsDark;
    }
}