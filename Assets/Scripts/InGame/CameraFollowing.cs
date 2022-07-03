using UnityEngine;

public class CameraFollowing : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 offset;
    private Vector3 _velocity = Vector3.zero;

    private void Start() {
        if (target) offset = target.position - transform.position;
    }

    private void LateUpdate() {
        if (!target) return;
        Vector3 newPosition = target.position - offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
    }
}