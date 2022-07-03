using System;
using UnityEngine;

public class BallZoneTrigger : MonoBehaviour {
    public event Action<float> BallExit;
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.TryGetComponent(out Rigidbody2D ball)) {
            float yShift = ball.velocity.y;
            BallExit?.Invoke(yShift);
        }
    }
}