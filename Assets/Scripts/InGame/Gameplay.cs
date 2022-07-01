using UnityEngine;

public class Gameplay : MonoBehaviour {
    public static bool CanThrow = true;
    [SerializeField] private AimHandler aim;
    [SerializeField] private Ball ball;
    [SerializeField] private Basket currentBasket;
    void Start() {
        currentBasket.BallCatched += OnBallCatched;
        currentBasket.BallThrowed += OnBallThrowed;
        SetActiveBasket(currentBasket);
    }

    private void SetActiveBasket(Basket basket) {
        currentBasket = basket;
        currentBasket.SetBall(ball);
        aim.AimUpdated += currentBasket.Aim;
        aim.AimReleased += currentBasket.ThrowBall;
    }

    private void OnBallThrowed(Basket basket) {
        aim.AimUpdated -= currentBasket.Aim;
        aim.AimReleased -= currentBasket.ThrowBall;
        currentBasket = null;
        CanThrow = false;
    }

    private void OnBallCatched(Basket basket) {
        SetActiveBasket(basket);
        CanThrow = true;
    }
}