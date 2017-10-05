using Insight.Converters;
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
        public List<Selection> GetBets(TipsterSetup setup)
        {
            var selections = new List<PossibleBet>();

            foreach (var league in setup.SelectedLeagues)
            {
                if (league.Value)
                {
                    var currentLeague = LeagueHolder.LeagueList[league.Key];
                    var fixturesToFetch = currentLeague.NumTeams/2;

                    var fixtures = currentLeague.Fixtures.Take(fixturesToFetch);

                    foreach (var fixture in fixtures)
                    {
                        var bets = currentLeague.FindBets(fixture);

                        foreach (var bet in bets)
                        {
                            if (bet.Type == BetType.BTTS)
                            {
                                var valid = CheckBtts(currentLeague, bet);
                                if (valid.Confidence > 60) { selections.Add(valid); }
                            }
                            if (bet.Type == BetType.Over1AndAHalf)
                            {
                                var valid = CheckOver1(currentLeague, bet);
                                if (valid.Confidence > 70) { selections.Add(valid); }
                            }

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
                                            if (selection.Confidence > 80)
                                            {
                                                selections.Add(selection);
                                            }
                                        }
                                    }
                                }
                            }

                            if (bet.Type == BetType.HomeWin || bet.Type == BetType.AwayWin || bet.Type == BetType.Draw)
                            {

                                var valid = ValidateFixture(fixture, setup, bet.Type, currentLeague, bet);

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

            var sorted = MixAndMatch(selections);
            
            return sorted;
            //return selections.Where(s => s.Confidence > 50).ToList();
        }


        private string ClassifyItem(PossibleBet item)
        {
            if (item.Confidence >= 65 && item.GetOddsDouble() >= 9d / 10) { return "Value"; }
            if (item.Confidence >= 45 && item.GetOddsDouble() >= 1.2d / 1) { return "Long Shot"; }
            if (item.Confidence > 55 && item.GetOddsDouble() >= 7d / 10) { return "Double"; }
            if (item.Confidence >= 70) { return "Acca"; }

            return "Ignore";
        }

        public List<Selection> MixAndMatch(List<PossibleBet> list)
        {
            var query = list.GroupBy(x => x.When);

            var selections = new List<Selection>();

            foreach (var q in query)
            {
                var lister = q.ToList();
                var outrights = lister.Where(x => x.Type == BetType.HomeWin || x.Type == BetType.AwayWin).ToList();
                var scorers = lister.Where(x => x.Type == BetType.ToScoreIn90).ToList();
                var theDouble = new List<PossibleBet>();
                var theAcca = new List<PossibleBet>();

                foreach(var scorer in scorers)
                {
                    selections.Add(new Selection(scorer.Player + " to score", q.Key, scorer));
                }

                foreach (var item in outrights)
                {
                    var classification = ClassifyItem(item);

                    // to do - add field to acca class - set type
                    if (classification == "Value") { selections.Add(new Selection("Value", q.Key, item)); }
                    if (classification == "Long Shot") { selections.Add(new Selection("Long Shot", q.Key, item)); }
                    if (classification == "Acca") { theAcca.Add(item); }
                    if (classification == "Double") { theDouble.Add(item); }

                    if (theDouble.Count >= 2 && new Selection("temp", "", theDouble).GetOverallOdds() > 1.5 && theDouble.Count < 4)
                    {
                        var copy = new List<PossibleBet>();
                        copy.AddRange(theDouble);
                        selections.Add(new Selection("Double", q.Key, copy));
                        theDouble.Clear();
                    }

                }

                if (theDouble.Count > 0)
                {
                    if (theDouble.Count == 1)
                    {
                        var selection = new Selection("Single", q.Key, theDouble);
                        if (selection.GetOverallOdds() > 1.75d)
                        {
                            selections.Add(selection);
                        }
                    }
                    else
                    {
                        selections.Add(new Selection("Double", q.Key, theDouble));
                    }
                }
                if (theAcca.Count > 0)
                {
                    if (theAcca.Count == 1)
                    {
                        var selection = new Selection("Outright", q.Key, theAcca);
                        var odds = selection.GetOverallOdds();
                        if (odds >= 0.5d)
                        {
                            selections.Add(selection);
                        }
                    }
                    else if (theAcca.Count == 2)
                    {
                        var selection = new Selection("Double", q.Key, theAcca);
                        var odds = selection.GetOverallOdds();
                        if (odds >= 0.8d)
                        {
                            selections.Add(selection);
                        }
                    }
                    else
                    {
                        selections.Add(new Selection("Accumulator", q.Key, theAcca));
                    }
                }

                // sort into categories

                var over = lister.Where(x => x.Type == BetType.Over1AndAHalf).ToList();
                var btts = lister.Where(x => x.Type == BetType.BTTS).ToList();


                if (over.Count() + btts.Count() < 4)
                {
                    over.AddRange(btts);
                    btts.Clear();
                }

                if (over.Count > 0) { selections.Add(new Selection("Goals Acca", q.Key, over)); }
                if (btts.Count > 0) { selections.Add(new Selection("BTTS Acca", q.Key, btts)); }

                var stop = 0;

            }

            return selections;

        }




        PossibleBet CheckBtts(League league, PossibleBet bet)
        {
            var homeTeam = bet.HomeTeam;
            var awayTeam = bet.AwayTeam;

            var someResults = league.Results.Take(3 * league.NumTeams);

            var allResults = someResults.Where(res => res.HomeTeam == homeTeam || res.AwayTeam == homeTeam).ToList();
            allResults.AddRange(someResults.Where(res => res.HomeTeam == awayTeam || res.AwayTeam == awayTeam));

            var successCount = 0;
            var gameCount = 0;

            foreach (var v in allResults)
            {
                gameCount++;
                if (v.HomeScore > 0 && v.AwayScore > 0) { successCount++; }
            }

            var perc = (float)successCount / gameCount;


            // league pos
            var homePos = 0;
            var awayPos = 0;

            for (int i = 0; i < league.Table.Count; i++)
            {
                if (league.Table[i].Team == bet.HomeTeam)
                {
                    homePos = i;
                }
                if (league.Table[i].Team == bet.AwayTeam)
                {
                    awayPos = i;
                }
            }

            var tableDiff = Math.Abs(homePos - awayPos);
            var tableFactor = ((float)tableDiff) / ((float)league.NumTeams * 2.5f);

            var TABLE_WEIGHT = 30;
            var OVER_GOALS_WEIGHT = 70;

            var confidence = (OVER_GOALS_WEIGHT * perc)
                           + (TABLE_WEIGHT * tableFactor);

            bet.Confidence = confidence;
            return bet;
            
        }

        PossibleBet CheckOver1(League league, PossibleBet bet)
        {
            var homeTeam = bet.HomeTeam;
            var awayTeam = bet.AwayTeam;

            var someResults = league.Results.Take(3 * league.NumTeams);

            var allResults = someResults.Where(res => res.HomeTeam == homeTeam || res.AwayTeam == homeTeam).ToList();
            allResults.AddRange(someResults.Where(res => res.HomeTeam == awayTeam || res.AwayTeam == awayTeam));

            var successCount = 0;
            var gameCount = 0;

            foreach (var v in allResults)
            {
                gameCount++;
                if (v.HomeScore + v.AwayScore > 1) { successCount++; }
            }

            var perc = (float)successCount / gameCount;

            // league pos
            var homePos = 0;
            var awayPos = 0;

            for (int i = 0; i < league.Table.Count; i++)
            {
                if (league.Table[i].Team == bet.HomeTeam)
                {
                    homePos = i;
                }
                if (league.Table[i].Team == bet.AwayTeam)
                {
                    awayPos = i;
                }
            }

            var tableDiff = Math.Abs(homePos - awayPos);
            var tableFactor = ((float)tableDiff) / ((float)league.NumTeams * 2.5f);
            
            var TABLE_WEIGHT = 20;
            var OVER_GOALS_WEIGHT = 80;

            var confidence = (OVER_GOALS_WEIGHT * perc)
                           + (TABLE_WEIGHT * tableFactor);

            bet.Confidence = confidence;
                return bet;
            

        }

        List<PossibleBet> ValidateFixture(Fixture fixture, TipsterSetup bot, BetType bet, League league, PossibleBet theBet)
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
            // half with most goals


            foreach (var config in configs)
            {
                switch (bet)
                {
                    case BetType.HomeWin:
                        var possibleBet = CalculateBetConfidence(fixture, config, league, BetType.HomeWin);
                        if (possibleBet.Confidence > 54) { return new List<PossibleBet> { possibleBet }; }
                        break;
                    case BetType.AwayWin:
                        possibleBet = CalculateBetConfidence(fixture, config, league, BetType.AwayWin);
                        if (possibleBet.Confidence > 65) { return new List<PossibleBet> { possibleBet }; }
                        break;
                    //case BetType.Draw:
                    //    var possibleBetH = CalculateBetConfidence(fixture, config, league, BetType.HomeWin);
                    //    var possibleBetA = CalculateBetConfidence(fixture, config, league, BetType.AwayWin);
                    //    theBet.Confidence = 100 - (5 * Math.Abs(possibleBetH.Confidence - possibleBetA.Confidence));
                    //    if (Math.Abs(possibleBetH.Confidence - possibleBetA.Confidence) < 3) { return new List<PossibleBet> { theBet }; }
                    //    break;
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
                    if(tableDiff > -(table.Count / 5)) { tableDiff *= -1; }else { tableDiff = 0; }
                }

                int locOppositionGoals = 0;
                List<Fixture> oppositionGamesLoc;
                if(fixture.HomeTeam == oppositionTeam)
                {
                    // get home games only
                    oppositionGamesLoc = allResults.Where(res => res.HomeTeam == oppositionTeam).ToList();
                    foreach (var game in oppositionGamesLoc)
                    {
                        locOppositionGoals += game.AwayScore;
                    }
                }
                else
                {
                    // get away games only
                    oppositionGamesLoc = allResults.Where(res => res.AwayTeam == oppositionTeam).ToList();
                    foreach (var game in oppositionGamesLoc)
                    {
                        locOppositionGoals += game.HomeScore;
                    }
                }

                var oppositionGames = allResults.Where(res => res.HomeTeam == oppositionTeam || res.AwayTeam == oppositionTeam).ToList();
                var totalOppositionGoals = 0;
                foreach (var game in oppositionGames)
                {
                    totalOppositionGoals += game.HomeTeam == oppositionTeam ? game.AwayScore : game.HomeScore;
                }

                var tableFactor = ((float)tableDiff) / ((float)league.NumTeams * 2.5f);
                var concededFactor = (float) totalOppositionGoals / (oppositionGames.Count*2.25f);
                var concededFactorLoc = (float) locOppositionGoals / (oppositionGamesLoc.Count*2.25f);

                const int TABLE_WEIGHT = 22;
                const int GOALS_PER_GAME_WEIGHT = 36;
                const int OPPOSITION_CONCEDED_WEIGHT = 18;
                const int OPPOSITION_CONCEDED_LOC_WEIGHT = 24;

                var confidence = (GOALS_PER_GAME_WEIGHT * gpg)
                               + (OPPOSITION_CONCEDED_WEIGHT * concededFactor)
                               + (OPPOSITION_CONCEDED_LOC_WEIGHT * concededFactorLoc)
                               + (TABLE_WEIGHT * tableFactor);


                confidence += new Random().Next(-5, 3);

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
                else
                {
                    pointDifference = 0;
                }
            }
            var pointsFactor = (float)pointDifference / ((float)totalAvailablePoints / 1.4f);


            var tablePos = (oppPos - selPos);
            if (tablePos < 0)
            {
                if (tablePos >= -5) { tablePos *= -1; }
                else
                {
                    tablePos = 0;
                }
            }
            var tableFactor = ((float)tablePos) / ((float)league.NumTeams * 2.5f);


            var totalAvailablePointsLoc = Math.Max(recentTeamLocationalResults.Count(), recentOppositionLocationalResults.Count()) * 3;
            var pointDifferenceLoc = totalTeamPointsLoc - totalOppositionPointsLoc;
            if (pointDifferenceLoc < 0)
            {
                if (pointDifferenceLoc >= -3) { pointDifferenceLoc *= -1; }
                else
                {
                    pointDifferenceLoc = 0;
                }
            }
            var pointsFactorLoc = (float)pointDifferenceLoc / ((float)totalAvailablePointsLoc / 1.4f);

            const int TABLE_WEIGHT = 32;
            const int POINTS_WEIGHT = 24;
            const int LOCATIONAL_POINTS_WEIGHT = 44;

            var confidence = (POINTS_WEIGHT * pointsFactor)
                           + (LOCATIONAL_POINTS_WEIGHT * pointsFactorLoc)
                           + (TABLE_WEIGHT * tableFactor);

            //confidence += new Random().Next(-5, 5);

            if(theBet.HomeTeam == "Notts County" || theBet.AwayTeam == "Notts County" || theBet.HomeTeam == "Forest Green")
            {
                var v = 0;
            }

            theBet.Confidence = confidence;

            return theBet;
        }
    }
}
