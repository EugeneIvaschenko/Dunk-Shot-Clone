using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour {
    [field: SerializeField] public float ThrowPower { get; private set; } = 2;
    private Rigidbody2D rb;
    private TrajectoryRenderer tr;

    public bool IsSimulated => rb.simulated;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        tr = GetComponent<TrajectoryRenderer>();
    }

    public void Throw(Vector2 direction) {
        transform.parent = null;
        rb.simulated = true;
        rb.velocity = GetThrowForce(direction);
    }

    public Vector2 GetThrowForce(Vector2 direction) => direction * ThrowPower;

    public void ActivatePhysics() => rb.simulated = true;

    public void DeactivatePhysics() => rb.simulated = false;


    public void EraseTrajectory(Vector2 vector2) {
        if (tr) tr.EraseTrajectory();
    }

    public void DrawTrajectory(Vector2 direction) {
        if (tr ) {
            if (direction.magnitude < Gameplay.MinThrowForce) {
                EraseTrajectory(direction);
                return;
            }
            direction = Vector2.ClampMagnitude(direction, Gameplay.MaxThrowForce);
            tr.DrawTrajectory(GetThrowForce(direction)); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out Soundable component)) {
            BallSounds.PlaySound(component.SoundType);
        }
    }
}