using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Score : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI commonScoreText;
    [SerializeField] private TextMeshProUGUI starScoreText;

    private readonly Dictionary<ScorableType, ScoreCounter> scoreCounters = new Dictionary<ScorableType, ScoreCounter>();

    private void Awake() {
        scoreCounters.Add(ScorableType.Common, new ScoreCounter(ScorableType.Common, commonScoreText));
        scoreCounters.Add(ScorableType.Star, new ScoreCounter(ScorableType.Star, starScoreText));
    }
    public int Add(IScorable scored) => scoreCounters[scored.ScorableType].Add(scored);
    public int GetScoreNum(ScorableType scorableType) => scoreCounters[scorableType].ScoreNum;
    public void Clear() => scoreCounters[ScorableType.Common].Clear();
}