using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sleeper {
    public class SleeperHelper {
        private static readonly HttpClient client = new HttpClient();
        public async Task<List<SleeperUser>> GetSleeperUsers(long leagueid) {
            var retVal = client.GetStreamAsync($"https://api.sleeper.app/v1/league/{leagueid}/users");
            var users = await JsonSerializer.DeserializeAsync<List<SleeperUser>>(await retVal);

            return users;
        }

        public async Task<List<SleeperRoster>> GetSleeperRoster(long leagueid) {
            var retVal = client.GetStreamAsync($"https://api.sleeper.app/v1/league/{leagueid}/rosters");
            var rosters = await JsonSerializer.DeserializeAsync<List<SleeperRoster>>(await retVal);

            return rosters;
        }

        public async Task<List<SleeperMatchup>> GetSleeperMatchups(long leagueid, int week) {
            var retVal = client.GetStreamAsync($"https://api.sleeper.app/v1/league/{leagueid}/matchups/{week}");
            var matchups = await JsonSerializer.DeserializeAsync<List<SleeperMatchup>>(await retVal);

            return matchups;
        }

        public MasterStats Sleeper2Master(SleeperMaster master) {
            var retVal = new MasterStats();

            retVal.LeagueStats = League2Master(master);
            retVal.UserStats = User2Master(master);

            return retVal;
        }

        private LeagueStats League2Master(SleeperMaster master) {
            var retVal = new LeagueStats();

            retVal.highestScore = master.GetHighScore();
            retVal.lowestScore = master.GetLowScore();
            retVal.meanScore = master.GetMeanScore();
            retVal.stdDev = master.GetStdDev();

            return retVal;
        }

        private List<UserStats> User2Master(SleeperMaster master) {
            var stdDev = (decimal)master.GetStdDev();
            var mean = master.GetMeanScore();

            var usrLst = new List<UserStats>();

            foreach(var m in master.Matchups) {
                var stat = new UserStats();

                var roster = master.Rosters
                    .Where(x => x.roster_id == m.roster_id)
                    .FirstOrDefault();
                
                var owner = master.Users
                    .Where(x => x.user_id == roster.owner_id)
                    .FirstOrDefault();
                
                stat.score = m.points;
                stat.userId = roster.owner_id;
                stat.teamName = owner.display_name;
                stat.oppScore = master.Matchups
                    .Where(x => x.matchup_id == m.matchup_id && x.roster_id != m.roster_id)
                    .FirstOrDefault()
                    .points;
                
                stat.zIndex = (m.points - mean) / stdDev;

                usrLst.Add(stat);
            }

            return usrLst;
        }
    }
}