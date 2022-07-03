using TMPro;

public class ScoreCounter {
    public ScorableType TypeCounter { get; }
    private readonly TextMeshProUGUI scoreText;
    public ScoreCounter(ScorableType type, TextMeshProUGUI scoreText) {
        TypeCounter = type;
        this.scoreText = scoreText;
    }
    public int ScoreNum { get; private set; } = 0;

    public int Add(IScorable scored) {
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