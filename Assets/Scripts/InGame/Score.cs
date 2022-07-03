using UnityEngine;
using TMPro;

public class Score : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;

    public static int ScoreNum { get; private set; } = 0;

    public int Add(IScored scored) {
        if (!scored.IsScored) {
            UpdateScore(++ScoreNum);
            scored.ConfirmScored();
        }
        return ScoreNum;
    }

    public void Clear() => UpdateScore(ScoreNum = 0);

    private void UpdateScore(int score) {
        scoreText.text = score.ToString();
    }
}