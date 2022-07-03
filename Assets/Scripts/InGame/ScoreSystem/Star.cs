using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Star : MonoBehaviour, IScorable {
    public event Action<IScorable> Scored;
    public ScorableType ScorableType => ScorableType.Star;

    public bool IsScored { get; private set; } = false;

    public void ConfirmScored() {
        IsScored = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Ball ball)) {
            Scored?.Invoke(this);
        }
    }
}