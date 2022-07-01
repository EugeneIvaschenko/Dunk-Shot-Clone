using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour {
    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
    }

    public void Throw(Vector2 force) {
        transform.parent = null;
        rb.simulated = true;
        rb.velocity = force;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void DeactivatePhysics() {
        rb.simulated = false;
    }
}