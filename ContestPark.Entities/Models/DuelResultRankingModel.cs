using System.Collections.Generic;

namespace ContestPark.Entities.Models
{
    public class DuelResultRankingModel
    {
        public int RankIndex { get; set; }
        public IEnumerable<ScoreRankingModel> ScoreRankings { get; set; }
    }
}