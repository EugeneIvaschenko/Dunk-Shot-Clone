using UnityEngine;

public class Gameplay : MonoBehaviour {
    [SerializeField] private BasketManager basketManager;
    [SerializeField] private Score score;
    [SerializeField] private BallZoneScroller ballZoneScroller;
    [SerializeField] private AimHandler aim;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private UIMediator mediator;
    [SerializeField] private float loseHeigth = 3f;
    [SerializeField] private float minThrowForce = 1f;
    [SerializeField] private float maxThrowForce = 6f;

    private Vector3 ballZoneScrollerStartPos;
    private Ball Ball;

    public static float MinThrowForce { get; private set; }
    public static float MaxThrowForce { get; private set; }

    private void Awake() {
        ballZoneScrollerStartPos = ballZoneScroller.transform.position;
        MinThrowForce = minThrowForce;
        MaxThrowForce = maxThrowForce;
    }

    private void Update() {
        if (Ball && Ball.IsSimulated && (Ball.transform.position.y < basketManager.GetLowestBasket().transform.position.y - loseHeigth)) {
            mediator.OpenLoseMenu();
        }
    }

    public void CreateBall() {
        Ball = Instantiate(ballPrefab);
        aim.AimUpdated += Ball.DrawTrajectory;
        aim.AimReleased += Ball.EraseTrajectory;
        basketManager.Ball = Ball;
        BasketManager.CanThrow = true;
    }

    public void DestroyBall() {
        if (Ball) {
            aim.AimUpdated -= Ball.DrawTrajectory;
            aim.AimReleased -= Ball.EraseTrajectory;
            basketManager.Ball = null;
            Destroy(Ball.gameObject);
            Ball = null;
            BasketManager.CanThrow = false;
        }
    }

    public void StartGame() {
        score.Clear();
        ballZoneScroller.transform.position = ballZoneScrollerStartPos;
        CreateBall();
        basketManager.PrepareBaskets();
    }

    public void EndGame() {
        DestroyBall();
        basketManager.DestroyAllBaskets();
    }

    public void RestartGame() {
        EndGame();
        StartGame();
    }

    public void PauseGame() => Ball.DeactivatePhysics();

    public void UnpauseGame() => Ball.ActivatePhysics();
}