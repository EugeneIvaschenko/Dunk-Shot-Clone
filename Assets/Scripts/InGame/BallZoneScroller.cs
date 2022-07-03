using UnityEngine;

public class BallZoneScroller : MonoBehaviour {
    [SerializeField] private BallZoneTrigger topScrollTrigger;
    [SerializeField] private BallZoneTrigger downScrollTrigger;

    private void Awake() {
        topScrollTrigger.BallExit += MoveZoneUp;
        downScrollTrigger.BallExit += MoveZoneDown;
    }

    private void MoveZoneUp(float shift) {
        if(shift < 0) {
            shift = Mathf.Abs(shift);
        }
        MoveZone(shift);
    }

    private void MoveZoneDown(float shift) {
        if (shift > 0) {
            shift *= -1;
        }
        MoveZone(shift);
    }

    private void MoveZone(float shift) {
        transform.Translate(new Vector3(0, shift * Time.fixedDeltaTime, 0));
    }
}