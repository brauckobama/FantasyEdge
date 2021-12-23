using System;
using Api;

namespace FantasyEdge
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new ApiHelper();
            var test = helper.GetLeagueStats(0);
        }
    }
}
