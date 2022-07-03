using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    [SerializeField, Min(0)] private float aimSensibility = 1;

    private bool isMouseHolded = false;
    private Vector3 mouseDownPos;

    public event Action<Vector2> AimUpdated;
    public event Action<Vector2> AimReleased;

    private void Update() {
        if(BasketManager.CanThrow)
            MouseRead();
    }

    private void MouseRead() {
        if (isMouseHolded) {
            Aiming();
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
        if(isMouseHolded) AimReleased?.Invoke(GetAimVector());
        isMouseHolded = false;
    }

    private void CancelAim() {
        isMouseHolded = false;
        AimReleased?.Invoke(Vector3.zero);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!isMouseHolded) StartAim();
    }

    public void OnPointerExit(PointerEventData eventData) {
        CancelAim();
    }

    public void OnPointerUp(PointerEventData eventData) {
        ReleaseAim();
    }

    private Vector2 GetAimVector() {
        return (Camera.main.ScreenToWorldPoint(mouseDownPos) - Camera.main.ScreenToWorldPoint(Input.mousePosition)) * aimSensibility;
    }
}