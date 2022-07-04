using UnityEngine;
using UnityEngine.UI;

public class BallSkins : MonoBehaviour {
    [field: SerializeField] public Sprite[] Skins { get; private set; }
    [SerializeField] private RectTransform uiContent;
    [SerializeField] private Button skinnButtonPrefab;
    public static Sprite CurrentSkin { get; private set; }

    private static BallSkins instance;

    private void Awake() {
        if(!instance) instance = this;
        SetSkin(0);
        InitMenu();
    }

    private void InitMenu() {
        for (int i = 0; i < Skins.Length; i++) {
            Button button = Instantiate(skinnButtonPrefab, uiContent);
            button.image.sprite = Skins[i];
            int num = i;
            button.onClick.AddListener(() => { SetSkin(num); });
        }
    }

    public static void SetSkin(int index) {
        if (instance.Skins.Length > index) {
            CurrentSkin = instance.Skins[index];
        }
    }
    public static void SetSkin(Sprite sprite) {
        CurrentSkin = sprite;
    }
}