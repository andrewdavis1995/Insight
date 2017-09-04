using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Data_Fetchers
{
    public class AliasFetcher
    {
        public string GetAlternativeTeamName(string team)
        {
            //spl
            switch (team)
            {
                case "Hamilton Academical": return "Hamilton";

                // prem
                case "West Bromwich Albion":
                    return "West Brom";
                case "Stoke City":
                    return "Stoke";
                case "Tottenham Hotspur":
                    return "Tottenham";
                case "Swansea City":
                    return "Swansea";
                case "Huddersfield Town":
                    return "Huddersfield";
                case "Newcastle United":
                    return "Newcastle";
                case "West Ham United":
                    return "West Ham";
                case "Brighton and Hove Albion":
                    return "Brighton";
                case "Manchester United":
                    return "Manchester Utd";
                case "Leicester City":
                    return "Leicester";


                // championship
                case "Birmingham City":
                    return "Birmingham";
                case "Wolverhampton Wanderers":
                    return "Wolves";
                case "Burton Albion":
                    return "Burton";
                case "Cardiff City":
                    return "Cardiff";
                case "Queens Park Rangers":
                    return "QPR";
                case "Ipswich Town":
                    return "Ipswich";
                case "Preston North End":
                    return "Preston";
                case "Norwich City":
                    return "Norwich";
                case "Sheffield United":
                    return "Sheffield Utd";
                case "Derby County":
                    return "Derby";
                case "Leeds United":
                    return "Leeds";
                case "Hull City":
                    return "Hull";
                case "Bolton Wanderers":
                    return "Bolton";

            }
            return team; 
        }
        
        public string GetAlternativeLeagueName(string league)
        {
            switch (league)
            {
                case "Premier League": return "premier-league";
                case "Championship": return "sky-bet-championship";
                case "League One": return "sky-bet-league-1";
                case "League Two": return "sky-bet-league-two";
                case "Scottish Premier League": return "scottish-premiership";
                case "Scottish Championship": return "scottish-championship";
            }

            return league;
        }
    }
}
