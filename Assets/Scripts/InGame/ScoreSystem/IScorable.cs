public interface IScorable {
    public ScorableType ScorableType { get; }
    public bool IsScored { get; }
    public void ConfirmScored();
}

public enum ScorableType {
    Common,
    Star
}