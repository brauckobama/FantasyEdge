using System;
using System.Collections.Generic;
using System.Linq;
public class SleeperMaster {
    public List<SleeperMatchup> Matchups { get; set; }
    public List<SleeperUser> Users { get; set; }
    public List<SleeperRoster> Rosters { get; set; }

    public Score GetHighScore() {
        var score = Matchups
                    .OrderByDescending(x => x.points)
                    .FirstOrDefault();

        var retVal = new Score(){
            total = score.points,
            rosterId = score.roster_id
        };

        return retVal;
    }

    public Score GetLowScore() {
        var score = Matchups
                    .OrderBy(x => x.points)
                    .FirstOrDefault();

        var retVal = new Score(){
            total = score.points,
            rosterId = score.roster_id
        };

        return retVal;
    }

    public decimal GetMeanScore() {
        return Matchups
                .Select(x => x.points)
                .Average();
    }

    public double GetStdDev() {
        double retVal = 0;
        var count = Matchups.Count();
        if (count < 1) {
            return 0;
        }
        var values = Matchups
                    .Select(x => (double)x.points)
                    .ToList();

        double avg = values.Average();
        double sum = values.Sum(d => (d - avg) * (d - avg));

        retVal = Math.Sqrt(sum / count);
        return retVal;
    }
}