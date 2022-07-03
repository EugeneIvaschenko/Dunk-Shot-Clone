using System;
using UnityEngine;

public class NetCollider : MonoBehaviour {
    public event Action BallCatched;

    private void OnTriggerStay2D(Collider2D collision) {
        TryCatchBall(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        TryCatchBall(collision);
    }

    private void TryCatchBall(Collider2D collision) {
        if (collision.GetComponent<Ball>()) {
            BallCatched?.Invoke();
        }
    }
}