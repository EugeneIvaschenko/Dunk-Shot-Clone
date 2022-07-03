using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrajectoryRenderer : MonoBehaviour {
    [SerializeField] private uint steps = 100;
    private LineRenderer lr;
    private Rigidbody2D rb;

    private void Start() {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void DrawTrajectory(Vector2 velocity) {
        if (!lr) return;
        Vector3[] trajectory = GetTrajectory(rb, rb.position, velocity, steps);

        lr.positionCount = trajectory.Length;

        lr.SetPositions(trajectory);
    }

    public void EraseTrajectory() {
        if (!lr) return;
        lr.positionCount = 0;
    }

    public Vector3[] GetTrajectory(Rigidbody2D rb, Vector3 pos, Vector3 velocity, uint steps) {
        Vector3[] result = new Vector3[steps];

        float timestep = Time.fixedDeltaTime;
        Vector3 gravityAccel = rb.gravityScale * timestep * timestep * Physics2D.gravity;
        
        float drag = 1f - timestep * rb.drag;
        Vector3 moveStep = velocity * timestep;
        Vector3 newPos = pos;

        RaycastHit2D[] hits = new RaycastHit2D[1];
        ContactFilter2D filter = new ContactFilter2D {
            layerMask = LayerMask.GetMask("Obstacles")
        };
        float bounciness = (1 - rb.sharedMaterial.bounciness) / 1.3f + rb.sharedMaterial.bounciness; //crutch for bounciness simulation
        for (int i = 0; i < steps; i++) {
            moveStep += gravityAccel;
            moveStep *= drag;

            rb.position = newPos;
            int hitCount = rb.Cast(moveStep, filter, hits, moveStep.magnitude);
            if (hitCount > 0) {
                moveStep = Vector2.Reflect(moveStep, hits[0].normal) * bounciness;
                
            }

            newPos += moveStep;
            result[i] = newPos;

        }

        rb.position = pos;

        return result;
    }
}