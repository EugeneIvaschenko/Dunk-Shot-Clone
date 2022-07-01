using System;
using System.Collections;
using UnityEngine;

public class Basket : MonoBehaviour {
    [SerializeField] private Transform markForBall;
    [SerializeField] private Collider2D catchCollider;
    [SerializeField] private NetCollider net;
    private bool ignoreCatching = false;
    private Ball ball;
    public event Action<Basket> BallThrowed;
    public event Action<Basket> BallCatched;

    private void Start() {
        net.BallCatched += OnBallCatched;
    }

    public void SetBall(Ball b) {
        ball = b;
        ball.DeactivatePhysics();
        ball.transform.SetParent(transform);
        ball.transform.position = markForBall.position;
    }

    public void ThrowBall(Vector2 force) {
        if (!ball) return;
        ball.Throw(force);
        ball = null;
        StartCoroutine(TemporarilyIgnoreCatching());
        BallThrowed?.Invoke(this);
    }

    public void OnAiming(Vector2 direction) {
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector3.up, direction));
    }

    private void OnBallCatched() {
        if (ignoreCatching || ball) return;
        BallCatched?.Invoke(this);
    }

    private IEnumerator TemporarilyIgnoreCatching() {
        ignoreCatching = true;
        yield return new WaitForSeconds(0.2f);
        ignoreCatching = false;
    }
}