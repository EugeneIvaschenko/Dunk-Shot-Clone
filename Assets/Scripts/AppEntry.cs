using UnityEngine;

public class AppEntry : MonoBehaviour {
    [SerializeField] UIMediator mediator;
    void Start() {
        mediator.StartApp();
    }
}