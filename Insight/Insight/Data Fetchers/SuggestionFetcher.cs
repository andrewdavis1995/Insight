using Insight.Holders;
using Insight.Models;
using Insight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Data_Fetchers
{
    public class SuggestionFetcher
    {
        public List<PossibleBet> GetBets(TipsterSetup setup)
        {
            var selections = new List<PossibleBet>();

            foreach (var league in setup.SelectedLeagues)
            {
                if (league.Value)
                {
                    var currentLeague = LeagueHolder.LeagueList[league.Key];
                    var fixturesToFetch = currentLeague.NumTeams;

                    var fixtures = currentLeague.Fixtures.Take(fixturesToFetch);

                    foreach (var fixture in fixtures)
                    {
                        var bets = currentLeague.FindBets(fixture);

                        foreach (var bet in bets)
                        {
                            //for (int i = 0; i < setup.Configs.Count; i++)
                            //{
                            // TEMP
                            //if (setup.Ids[i] == 0)
                            //{
                            //continue;

                            if (bet.Type == BetType.ToScoreIn90)
                            {
                                var setupList = new List<Dictionary<string, object>>();
                                for (var i = 0; i < setup.Ids.Count; i++)
                                {
                                    if (setup.Ids[i] == 1)
                                    {
                                        setupList.Add(setup.Configs[i]);
                                    }
                                }

                                foreach (var v in setupList)
                                {
                                    var valid = CalculateScorerConfidence(fixture, v, currentLeague, bet);

                                    if (valid != null)
                                    {
                                        foreach (var selection in valid)
                                        {
                                            if (selection.Confidence > 100/1.5)
                                            {
                                                selections.Add(selection);
                                            }
                                        }
                                    }

                                }
                            }

                            if (bet.Type == BetType.HomeWin || bet.Type == BetType.AwayWin)
                            {

                                var valid = ValidateFixture(fixture, setup, bet.Type, currentLeague);

                                if (valid != null)
                                {
                                    selections.AddRange(valid);
                                }
                            }
                            //}
                            //}
                        }
                    }
                }
            }

            return selections;
            //return selections.Where(s => s.Confidence > 50).ToList();
        }

        List<PossibleBet> ValidateFixture(Fixture fixture, TipsterSetup bot, BetType bet, League league)
        {
            var id = -1;
            if (bet == BetType.HomeWin || bet == BetType.AwayWin || bet == BetType.Draw)
            {
                id = 0;
            }
            if (bet == BetType.ToScoreIn90)
            {
                id = 1;
            }

            var configs = new List<Dictionary<string, object>>();
            for (int i = 0; i < bot.Configs.Count && i < bot.Ids.Count; i++)
            {
                if (bot.Ids[i] == id)
                {
                    configs.Add(bot.Configs[i]);
                }
            }

            // win
            // btts
            // over 1
            // player to score
            // half with most girls


            foreach (var config in configs)
            {
                switch (bet)
                {
                    case BetType.HomeWin:
                        var possibleBet = CalculateBetConfidence(fixture, config, league, BetType.HomeWin);
                        if (possibleBet.Confidence > int.Parse(config["Confidence"].ToString())) { return new List<PossibleBet> { possibleBet }; }
                        break;
                    case BetType.AwayWin:
                        possibleBet = CalculateBetConfidence(fixture, config, league, BetType.AwayWin);
                        if (possibleBet.Confidence > int.Parse(config["Confidence"].ToString())) { return new List<PossibleBet> { possibleBet }; }
                        break;
                }
            }

            return null;
        }

        private List<PossibleBet> CalculateScorerConfidence(Fixture fixture, Dictionary<string, object> config, League league, PossibleBet bet)
        {
            var list = new List<PossibleBet>();

            var minConfidence = int.Parse(config["Confidence"].ToString());
            var games = int.Parse(config["Games_Considered"].ToString());

            // get all bets
            var bets = league.FindBets(fixture);
            var betList = bets.Where(b => b.Type == BetType.ToScoreIn90).ToList();

            var scorerSplit = bet.Player.Split(' ');
            var scorer = bet.Player.Substring(0, 1) + " " + scorerSplit[scorerSplit.Length - 1];

            var allResults = league.Results.Where(r => r.HomeTeam == bet.HomeTeam || r.HomeTeam == bet.AwayTeam || r.AwayTeam == bet.HomeTeam || r.AwayTeam == bet.AwayTeam).ToList();
            var results = allResults.Take(2 * games).ToList();

            var totalGoals = 0;
            var relevantTeam = "";

            foreach (var game in results)
            {
                foreach (var hScorer in game.HomeScorers)
                {
                    if (hScorer.Scorer.Trim() == scorer)
                    {
                        totalGoals++;
                        relevantTeam = game.HomeTeam;
                    }
                }
                foreach (var aScorer in game.AwayScorers)
                {
                    if (aScorer.Scorer.Trim() == scorer)
                    {
                        totalGoals++;
                        relevantTeam = game.AwayTeam;
                    }
                }
            }

            var gpg = (float)totalGoals / games;
                      
            if (totalGoals > 1)
            {
                var oppositionTeam = fixture.HomeTeam == relevantTeam 
                    ? fixture.AwayTeam 
                    : fixture.HomeTeam;

                var table = league.Table;
                var selPos = 0;
                var oppPos = 0;
                for (int i = 0; i < table.Count; i++)
                {
                    if (table[i].Team == relevantTeam)
                    {
                        selPos = i;
                    }
                    if (table[i].Team == oppositionTeam)
                    {
                        oppPos = i;
                    }
                }

                var tableDiff = oppPos - selPos;
                if(tableDiff < 0)
                {
                    if(tableDiff > -(table.Count / 5)) { tableDiff *= -1; }
                }

                var oppositionGames = allResults.Where(res => res.HomeTeam == oppositionTeam || res.AwayTeam == oppositionTeam).ToList();
                var totalOppositionGoals = 0;
                foreach (var game in oppositionGames)
                {
                    totalOppositionGoals += game.HomeTeam == oppositionTeam ? game.AwayScore : game.HomeScore;
                }

                var tableFactor = ((float)tableDiff) / ((float)league.NumTeams * 2.5f);
                var concededFactor = (float) totalOppositionGoals / (oppositionGames.Count*2.5f);

                const int TABLE_WEIGHT = 24;
                const int GOALS_PER_GAME_WEIGHT = 36;
                const int OPPOSITION_CONCEDED = 40;

                var confidence = (GOALS_PER_GAME_WEIGHT * gpg)
                               + (OPPOSITION_CONCEDED * concededFactor)
                               + (TABLE_WEIGHT * tableFactor);

                bet.Confidence = confidence;
                list.Add(bet);
            }


            return list;
        }

        private PossibleBet CalculateBetConfidence(Fixture fixture, Dictionary<string, object> config, League league, BetType type)
        {
            var bets = league.FindBets(fixture);
            var theBet = bets.Where(b => b.Type == type).FirstOrDefault();
            if (theBet == null) { return null; }

            var games = int.Parse(config["Games_Considered"].ToString());

            var selectedTeam = fixture.HomeTeam;
            var oppositionTeam = fixture.AwayTeam;

            if (type == BetType.AwayWin)
            {
                selectedTeam = fixture.AwayTeam;
                oppositionTeam = fixture.HomeTeam;
            }

            var table = league.Table;
            var selPos = 0;
            var oppPos = 0;
            for (int i = 0; i < table.Count; i++)
            {
                if (table[i].Team == selectedTeam)
                {
                    selPos = i;
                }
                if (table[i].Team == oppositionTeam)
                {
                    oppPos = i;
                }
            }

            var fullTeamResults = league.Results.Where(l => l.HomeTeam == selectedTeam || l.AwayTeam == selectedTeam).ToList();
            var teamResults = fullTeamResults.Take(games);

            var fullTeamLocationalResults = new List<Fixture>();
            if (type == BetType.HomeWin) { fullTeamLocationalResults = fullTeamResults.Where(r => r.HomeTeam == selectedTeam).ToList(); }
            if (type == BetType.AwayWin) { fullTeamLocationalResults = fullTeamResults.Where(r => r.AwayTeam == selectedTeam).ToList(); }
            var recentTeamLocationalResults = fullTeamLocationalResults.Take((games / 2) + 1).ToList();

            var fullOppositionResults = league.Results.Where(l => l.HomeTeam == oppositionTeam || l.AwayTeam == oppositionTeam).ToList();
            var oppositionResults = fullOppositionResults.Take(games);

            var fullOppositionLocationalResults = new List<Fixture>();
            if (type == BetType.HomeWin) { fullOppositionLocationalResults = fullOppositionResults.Where(r => r.AwayTeam == oppositionTeam).ToList(); }
            if (type == BetType.AwayWin) { fullOppositionLocationalResults = fullOppositionResults.Where(r => r.HomeTeam == oppositionTeam).ToList(); }
            var recentOppositionLocationalResults = fullOppositionLocationalResults.Take((games / 2) + 1).ToList();


            var teamWinPoints = teamResults.Where(m => m.GetOutcome(selectedTeam) == Fixture.ResultType.Win).Count() * 3;
            var teamDrawPoints = teamResults.Where(m => m.GetOutcome(selectedTeam) == Fixture.ResultType.Draw).Count();
            var totalTeamPoints = teamWinPoints + teamDrawPoints;

            var oppositionWinPoints = oppositionResults.Where(m => m.GetOutcome(oppositionTeam) == Fixture.ResultType.Win).Count() * 3;
            var oppositionDrawPoints = oppositionResults.Where(m => m.GetOutcome(oppositionTeam) == Fixture.ResultType.Draw).Count();
            var totalOppositionPoints = oppositionWinPoints + oppositionDrawPoints;

            var teamWinPointsLoc = recentTeamLocationalResults.Where(m => m.GetOutcome(selectedTeam) == Fixture.ResultType.Win).Count() * 3;
            var teamDrawPointsLoc = recentTeamLocationalResults.Where(m => m.GetOutcome(selectedTeam) == Fixture.ResultType.Draw).Count();
            var totalTeamPointsLoc = teamWinPointsLoc + teamDrawPointsLoc;

            var oppositionWinPointsLoc = recentOppositionLocationalResults.Where(m => m.GetOutcome(oppositionTeam) == Fixture.ResultType.Win).Count() * 3;
            var oppositionDrawPointsLoc = recentOppositionLocationalResults.Where(m => m.GetOutcome(oppositionTeam) == Fixture.ResultType.Draw).Count();
            var totalOppositionPointsLoc = oppositionWinPointsLoc + oppositionDrawPointsLoc;

            var totalAvailablePoints = Math.Max(teamResults.Count(), oppositionResults.Count()) * 3;
            var pointDifference = totalTeamPoints - totalOppositionPoints;
            if (pointDifference < 0)
            {
                if (pointDifference >= -3) { pointDifference *= -1; }
                else { pointDifference = 0; }
            }
            var pointsFactor = (float)pointDifference / ((float)totalAvailablePoints / 1.4f);


            var tablePos = (oppPos - selPos);
            if (tablePos < 0)
            {
                if (tablePos >= -5) { tablePos *= -1; }
                else { tablePos = 0; }
            }
            var tableFactor = ((float)tablePos) / ((float)league.NumTeams * 2.5f);


            var totalAvailablePointsLoc = Math.Max(recentTeamLocationalResults.Count(), recentOppositionLocationalResults.Count()) * 3;
            var pointDifferenceLoc = totalTeamPointsLoc - totalOppositionPointsLoc;
            if (pointDifferenceLoc < 0)
            {
                if (pointDifferenceLoc >= -3) { pointDifferenceLoc *= -1; }
                else { pointDifferenceLoc = 0; }
            }
            var pointsFactorLoc = (float)pointDifferenceLoc / ((float)totalAvailablePointsLoc / 1.4f);

            const int TABLE_WEIGHT = 19;
            const int POINTS_WEIGHT = 46;
            const int LOCATIONAL_POINTS_WEIGHT = 35;

            var confidence = (POINTS_WEIGHT * pointsFactor)
                           + (LOCATIONAL_POINTS_WEIGHT * pointsFactorLoc)
                           + (TABLE_WEIGHT * tableFactor);

            theBet.Confidence = confidence;

            return theBet;
        }
    }
}
