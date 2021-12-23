using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Sleeper;

namespace Api {
    public class ApiHelper {

        private static readonly SleeperHelper sh = new SleeperHelper();

        public MasterStats GetLeagueStats(SupportedFantasySites league) {
            if (league == 0) {
                var sleeper = GetSleeper(738504232495411200, 15);

                var dto = sh.Sleeper2Master(sleeper);

                return dto;
            }

            return new MasterStats();
        }

        public static SleeperMaster GetSleeper(long leagueid, int week) {
            var retVal = new SleeperMaster();
            //get sleeper users
            retVal.Users = sh.GetSleeperUsers(leagueid).Result;
            //get sleeper rosters
            retVal.Rosters = sh.GetSleeperRoster(leagueid).Result;
            //get sleeper matchups
            retVal.Matchups = sh.GetSleeperMatchups(leagueid, week).Result;

            return retVal;
        }
    }
}