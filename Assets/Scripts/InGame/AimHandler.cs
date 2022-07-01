using System;
using UnityEngine;

public class AimHandler : MonoBehaviour {
    [SerializeField, Min(0)] private float aimSensibility = 1;

    private bool isMouseHolded = false;
    private Vector3 mouseDownPos;

    public event Action<Vector2> AimUpdated;
    public event Action<Vector2> AimReleased;

    private void Update() {
        if(Gameplay.CanThrow)
            InputRead();
    }

    private void InputRead() {
        if (!isMouseHolded && Input.GetMouseButtonDown(0)) {
            StartAim();
        } else if (isMouseHolded && Input.GetMouseButton(0)) {
            Aiming();
        } else if (isMouseHolded && !Input.GetMouseButton(0)) {
            ReleaseAim();
        }
    }

    private void StartAim() {
        isMouseHolded = true;
        mouseDownPos = Input.mousePosition;
    }

    private void Aiming() {
        AimUpdated?.Invoke(GetAimVector());
    }

    private void ReleaseAim() {
        isMouseHolded = false;
        AimReleased?.Invoke(GetAimVector());
    }

    private Vector2 GetAimVector() {
        return (mouseDownPos - Input.mousePosition) / Screen.dpi * aimSensibility;
    }
}