using System;
using System.Collections;
using UnityEngine;

public class Basket : MonoBehaviour, IScorable {
    [SerializeField] private Transform markForBall;
    [SerializeField] private Collider2D catchCollider;
    [SerializeField] private NetCollider net;
    [SerializeField] private GameObject[] walls;
    [SerializeField] private float wallSpawnProbability = 0.1f;
    private bool ignoreCatching = false;
    private Ball ball;

    public bool IsScored { get; private set; } = false;

    public ScorableType ScorableType { get; } = ScorableType.Common;

    public event Action<Basket> BallThrowed;
    public event Action<Basket> BallCatched;

    private void Start() {
        net.BallCatched += OnBallCatched;
        Debug.Log("Start");
        if (UnityEngine.Random.Range(0f, 1f) < wallSpawnProbability) {
            Debug.Log("Spawned");
            GameObject wall = walls[UnityEngine.Random.Range(0, walls.Length)];
            wall.transform.SetParent(null);
            wall.SetActive(true);
        }
    }

    public void SetBall(Ball b) {
        ball = b;
        ball.DeactivatePhysics();
        ball.transform.SetParent(transform);
        ball.transform.position = markForBall.position;
    }

    public void ThrowBall(Vector2 force) {
        if (!ball || force.magnitude < Gameplay.MinThrowForce)
            return;
        force = Vector2.ClampMagnitude(force, Gameplay.MaxThrowForce);
        ball.Throw(force);
        ball = null;
        StartCoroutine(TemporarilyIgnoreCatching());
        BallThrowed?.Invoke(this);
    }

    public void OnAiming(Vector2 direction) {
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector3.up, direction));
    }

    private void OnBallCatched() {
        if (ignoreCatching || ball)
            return;
        BallCatched?.Invoke(this);
    }

    private IEnumerator TemporarilyIgnoreCatching() {
        ignoreCatching = true;
        yield return new WaitForSeconds(0.2f);
        ignoreCatching = false;
    }

    public void ConfirmScored() => IsScored = true;

    private void OnDestroy() {
        foreach(var wall in walls) {
            Destroy(wall);
        }
    }
}