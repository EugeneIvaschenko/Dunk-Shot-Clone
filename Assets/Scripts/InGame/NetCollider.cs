using System;
using UnityEngine;

public class NetCollider : MonoBehaviour {
    public event Action BallCatched;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Ball>()) {
            BallCatched?.Invoke();
        }
    }
}