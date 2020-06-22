using System.Collections.Generic;

public class Player
{
    public string UserName { get; set; }
    public Dictionary<string, int> UserColor { get; set; }
    public string SolveCount { get; set; }
    public string StrikeCount { get; set; }
    public string SolveScore { get; set; }
    public string Rank { get; set; }
    public string TotalSoloClears { get; set; }
    public string SoloRank { get; set; }
}