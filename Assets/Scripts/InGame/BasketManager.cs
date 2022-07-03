using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasketManager : MonoBehaviour {
    [SerializeField] private AimHandler aim;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Basket basketPrefab;

    private Ball ball;
    private Basket currentBasket;
    
    public static bool CanThrow = true;
    public List<Basket> baskets { get; private set; } = new List<Basket>();
    public event Action<int> HigherBasketReached;
    public int reachTopBasketsToCreateNew;

    public void PrepareBall() {
        ball = Instantiate(ballPrefab);
        aim.AimUpdated += ball.DrawTrajectory;
        aim.AimReleased += ball.EraseTrajectory;
    }

    public Basket CreateBasket(Vector3 pos, Quaternion rot) {
        Basket basket = Instantiate(basketPrefab, pos, rot);
        if (baskets.Count == 0) {
            currentBasket = basket;
            SetActiveBasket(currentBasket);
        }
        basket.BallCatched += OnBallCatched;
        basket.BallThrowed += OnBallThrowed;
        baskets.Add(basket);
        return basket;
    }

    public void DestroyLowestBasket() {
        Basket lowest = baskets.OrderBy(b => b.transform.position.y).First();
        baskets.Remove(lowest);
        aim.AimUpdated -= lowest.OnAiming;
        aim.AimReleased -= lowest.ThrowBall;
        Destroy(lowest.gameObject);
    }

    private void SetActiveBasket(Basket basket) {
        basket.SetBall(ball);
        aim.AimUpdated += basket.OnAiming;
        aim.AimReleased += basket.ThrowBall;
        currentBasket = basket;
    }

    private void OnBallThrowed(Basket basket) {
        aim.AimUpdated -= basket.OnAiming;
        aim.AimReleased -= basket.ThrowBall;
        currentBasket = null;
        CanThrow = false;
    }

    private void OnBallCatched(Basket basket) {
        SetActiveBasket(basket);
        CanThrow = true;

        int index = NumberFromTop(basket);
        if (index < reachTopBasketsToCreateNew) {
            HigherBasketReached?.Invoke(reachTopBasketsToCreateNew - index);
        }
    }

    private int NumberFromTop(Basket basket) {
        Basket[] baskets = this.baskets.OrderByDescending(b => b.transform.position.y).ToArray();
        int index = baskets.Length;
        for (int i = 0; i < baskets.Length; i++) {
            if (baskets[i] == basket) index = i;
        }
        return index;
    }
}