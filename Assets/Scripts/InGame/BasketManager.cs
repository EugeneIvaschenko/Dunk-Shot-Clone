using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasketManager : MonoBehaviour {
    [SerializeField] private Basket basketPrefab;
    [SerializeField] private Score score;
    [SerializeField] private AimHandler aim;
    [SerializeField] private Transform startBasketPos;
    [SerializeField] private Star starPrefab;
    [Header("Baskets Spawn Settings")]
    [SerializeField] private float basketXRange = 2;
    [SerializeField] private float minYBasketOffset = 1;
    [SerializeField] private float maxYBasketOffset = 4;
    [SerializeField, Min(3)] private int maxBasketsOnScene = 5;
    [SerializeField, Min(2)] private int reachTopBasketsForDeleting = 3;
    [SerializeField, Min(2)] private int starSpawnOneFor = 5;

    private Vector3 lastBasketPos;
    private Basket currentBasket;
    public static bool CanThrow = true;
    public List<Basket> Baskets { get; private set; } = new List<Basket>();
    public Ball Ball { get; set; }

    public void PrepareBaskets() {
        lastBasketPos = startBasketPos.position;
        for (int i = 0; i < maxBasketsOnScene; i++) {
            CreateNextBasket();
        }
    }

    private void CreateNextBasket() {
        Vector2 nextPos;
        if (Baskets.Count == 0) {
            nextPos = lastBasketPos;
        } else {
            float nextX = Random.Range(-basketXRange, basketXRange);
            float nextY = lastBasketPos.y + Random.Range(minYBasketOffset, maxYBasketOffset);
            nextPos = new Vector2(nextX, nextY);
        }
        CreateBasket(nextPos, Quaternion.identity);
        lastBasketPos = nextPos;
    }

    public Basket CreateBasket(Vector3 pos, Quaternion rot) {
        Basket basket = Instantiate(basketPrefab, pos, rot);
        if (Baskets.Count == 0) {
            currentBasket = basket;
            currentBasket.ConfirmScored();
            SetActiveBasket(currentBasket);
        } else {
            TryCreateStar(basket);
        }
        basket.BallCatched += OnBallCatched;
        basket.BallThrowed += OnBallThrowed;
        Baskets.Add(basket);
        return basket;
    }

    private void TryCreateStar(Basket basket) {
        if (starSpawnOneFor == Random.Range(1, starSpawnOneFor+1)) {
            Instantiate(starPrefab, basket.transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity, basket.transform).Scored += Score;
        }
    }

    private void Score(IScorable scorable) => score.Add(scorable);

    private void SetActiveBasket(Basket basket) {
        basket.SetBall(Ball);
        aim.AimUpdated += basket.OnAiming;
        aim.AimReleased += basket.ThrowBall;
        currentBasket = basket;
    }

    public void DestroyAllBaskets() {
        Basket[] baskets = Baskets.ToArray();
        foreach(var basket in baskets) {
            DestroyBasket(basket);
        }
    }

    public void DestroyLowestBasket() {
        Basket lowest = Baskets.OrderBy(b => b.transform.position.y).First();
        DestroyBasket(lowest);
    }

    public Basket GetLowestBasket() => Baskets.OrderBy(b => b.transform.position.y).First();

    public void DestroyBasket(Basket basket) {
        Baskets.Remove(basket);
        aim.AimUpdated -= basket.OnAiming;
        aim.AimReleased -= basket.ThrowBall;
        Destroy(basket.gameObject);
    }

    private void OnBallThrowed(Basket basket) {
        aim.AimUpdated -= basket.OnAiming;
        aim.AimReleased -= basket.ThrowBall;
        BallSounds.PlaySound(SoundType.Throw);
        currentBasket = null;
        CanThrow = false;
    }

    private void OnBallCatched(Basket basket) {
        SetActiveBasket(basket);
        CanThrow = true;
        Score(basket);
        OnHigherBasketReached(NumberFromTop(basket));
        BallSounds.PlaySound(SoundType.Net);
    }

    private int NumberFromTop(Basket basket) {
        Basket[] baskets = Baskets.OrderByDescending(b => b.transform.position.y).ToArray();
        int numberFromTop = baskets.Length;
        for (int i = 0; i < baskets.Length; i++) {
            if (baskets[i] == basket) {
                numberFromTop = i + 1;
                break;
            }
        }
        return numberFromTop;
    }

    private void OnHigherBasketReached(int numberFromTop) {
        int basketsToDelete = reachTopBasketsForDeleting - numberFromTop + 1;
        for (int i = 0; i < basketsToDelete; i++) {
            DestroyLowestBasket();
            CreateNextBasket();
        }
    }
}