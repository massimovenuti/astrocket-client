
interface IScoreable
{
    long Score { get; set; }
    void addScore(string token, long score, ScoreManager scm);
}
