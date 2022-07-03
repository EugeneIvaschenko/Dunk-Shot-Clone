using UnityEngine;

public class GameplayController : MonoBehaviour {
    [SerializeField] private BasketManager basketManager;
    [SerializeField] private float basketXRange = 2;
    [SerializeField] private float minYBasketOffset = 1;
    [SerializeField] private float maxYBasketOffset = 4;
    [SerializeField, Min(3)] private int maxBasketsOnScene = 5;
    [SerializeField, Min(2)] private int reachTopBasketsToCreateNew = 3;
    [SerializeField] private Transform startPos;
    private Vector3 lastPos;

    private void Awake() {
        basketManager.reachTopBasketsToCreateNew = reachTopBasketsToCreateNew;
        lastPos = startPos.position;
        basketManager.PrepareBall();
        PrepareBaskets();
        basketManager.HigherBasketReached += OnHigherBasketReached;
    }

    private void OnHigherBasketReached(int number) {
        for(int i = 0; i < number; i++) {
            basketManager.DestroyLowestBasket();
            CreateNextBasket();
        }
    }

    private void PrepareBaskets() {
        for (int i = 0; i < maxBasketsOnScene; i++) {
            CreateNextBasket();
        }
    }

    private void CreateNextBasket() {
        Vector2 nextPos;
        if (basketManager.baskets.Count == 0) {
            nextPos = lastPos;
        } else {
            float nextX = Random.Range(-basketXRange, basketXRange);
            float nextY = lastPos.y + Random.Range(minYBasketOffset, maxYBasketOffset);
            nextPos = new Vector2(nextX, nextY);
        }
        basketManager.CreateBasket(nextPos, Quaternion.identity);
        lastPos = nextPos;
    }
}