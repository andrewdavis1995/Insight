using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Data_Fetchers
{
    public class AliasFetcher
    {
        public string FixtureNameToSkyBetName(string team)
        {
            //spl
            switch (team)
            {
                case "Hamilton Academical":
                    return "Hamilton";

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
        
        public string SkybetNameToFixtureName(string team)
        {
            switch (team)
            {
                case "Hamilton":
                    return "Hamilton Academical";

                // prem
                case "West Brom":
                    return "West Bromwich Albion";
                case "Stoke":
                    return "Stoke City";
                case "Tottenham":
                    return "Tottenham Hotspur";
                case "Swansea":
                    return "Swansea City";
                case "Huddersfield":
                    return "Huddersfield Town";
                case "Newcastle":
                    return "Newcastle United";
                case "West Ham":
                    return "West Ham United";
                case "Brighton":
                    return "Brighton and Hove Albion";
                case "Manchester Utd":
                    return "Manchester United";
                case "Leicester":
                    return "Leicester City";


                // championship
                case "Birmingham":
                    return "Birmingham City";
                case "Wolves":
                    return "Wolverhampton Wanderers";
                case "Burton":
                    return "Burton Albion";
                case "Cardiff":
                    return "Cardiff City";
                case "QPR":
                    return "Queens Park Rangers";
                case "Ipswich":
                    return "Ipswich Town";
                case "Preston":
                    return "Preston North End";
                case "Norwich":
                    return "Norwich City";
                case "Sheffield Utd":
                    return "Sheffield United";
                case "Derby":
                    return "Derby County";
                case "Leeds":
                    return "Leeds United";
                case "Hull":
                    return "Hull City";
                case "Bolton":
                    return "Bolton Wanderers";

                // league one
                case "Blackburn":
                    return "Blackburn Rovers";
                case "Oxford Utd":
                    return "Oxford United";
                case "Bradford City":
                    return "Bradford";
                case "Rotherham":
                    return "Rotherham United";
                case "Plymouth":
                    return "Plymouth Argyle";
                case "MK Dons":
                    return "Milton Keynes Dons";
                case "Oldham":
                    return "Oldham Athletic";
                case "Shrewsbury":
                    return "Shrewsbury Town";
                case "Fleetwood":
                    return "Fleetwood Town";
                case "Southend":
                    return "Southend United";
                case "Northampton":
                    return "Northampton Town";
                case "Peterborough":
                    return "Peterborough United";
                case "Wigan":
                    return "Wigan Athletic";
                case "Doncaster":
                    return "Doncaster Rovers";
                case "Scunthorpe":
                    return "Scunthorpe United";

                // league two
                case "Lincoln":
                    return "Lincoln City";
                case "Mansfield":
                    return "Mansfield Town";
                case "Cambridge":
                    return "Cambridge United";
                case "Coventry":
                    return "Coventry City";
                case "Cheltenham":
                    return "Cheltenham Town";
                case "Colchester":
                    return "Colchester United";
                case "Accrington":
                    return "Accrington Stanley";
                case "Crawley":
                    return "Crawley Town";
                case "Exeter":
                    return "Exeter City";
                case "Crewe":
                    return "Crewe Alexandra";
                case "Grimsby":
                    return "Grimsby Town";
                case "Yeovil":
                    return "Yeovil Town";
                case "Newport County":
                    return "Newport County AFC";
                case "Forest Green":
                    return "Forest Green Rovers";
                case "Swindon":
                    return "Swindon Town";
                case "Wycombe":
                    return "Wycombe Wanderers";

                // scottish championship
                case "Brechin":
                    return "Brechin City";
                case "Dunfermline":
                    return "Dunfermline Athletic";
                case "Dundee Utd":
                    return "Dundee United";
                case "Queen of South":
                    return "Queen Of The South";
                case "Greenock Morton":
                    return "Morton";

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
