using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Insight.UI_Fetcher
{
    class BadgeFetcher
    {
        private string GetBadgeAddress(string team)
        {
            switch (team.ToLower())
            {
                //spl
                case "st johnstone":
                    return "192/529";
                case "celtic":
                    return "192/393";
                case "kilmarnock":
                    return "192/562";
                case "hamilton academical":
                    return "192/287";
                case "motherwell":
                    return "192/327";
                case "hearts":
                    return "192/265";
                case "partick thistle":
                    return "192/214";
                case "aberdeen":
                    return "192/209";
                case "dundee":
                    return "192/254";
                case "hibernian":
                    return "192/134";
                case "ross county":
                    return "192/578";
                case "rangers":
                    return "192/245";



                //scot champ
                case "dundee united":
                    return "192/251";
                case "st mirren":
                    return "192/501";
                case "brechin city":
                    return "192/213";
                case "inverness ct":
                    return "192/721";
                case "dumbarton":
                    return "192/394";
                case "dunfermline athletic":
                    return "192/291";
                case "falkirk":
                    return "192/303";
                case "queen of the south":
                    return "192/426";
                case "morton":
                    return "192/223";
                case "livingston":
                    return "192/580";




                // prem
                case "chelsea":
                    return "192/524";
                case "everton":
                    return "192/229";
                case "west bromwich albion":
                    return "192/275";
                case "stoke city":
                    return "192/384";
                case "liverpool":
                    return "192/155";
                case "arsenal":
                    return "192/413";
                case "tottenham hotspur":
                    return "192/608";
                case "burnley":
                    return "192/160";
                case "bournemouth":
                    return "192/333";
                case "manchester city":
                    return "192/345";
                case "crystal palace":
                    return "192/234";
                case "swansea city":
                    return "192/375";
                case "huddersfield town":
                    return "192/422";
                case "southampton":
                    return "192/392";
                case "newcastle united":
                    return "192/409";
                case "west ham united":
                    return "192/367";
                case "watford":
                    return "192/508";
                case "brighton and hove albion":
                    return "192/212";
                case "manchester united":
                    return "192/210";
                case "leicester city":
                    return "192/152";


                // championship
                case "barnsley":
                    return "192/191";
                case "sunderland":
                    return "192/415";
                case "birmingham city":
                    return "192/247";
                case "reading":
                    return "192/216";
                case "brentford":
                    return "192/194";
                case "wolverhampton wanderers":
                    return "192/206";
                case "burton albion":
                    return "192/294";
                case "sheffield wednesday":
                    return "192/376";
                case "cardiff city":
                    return "192/202";
                case "queens park rangers":
                    return "192/411";
                case "ipswich town":
                    return "192/157";
                case "fulham":
                    return "192/407";
                case "middlesbrough":
                    return "192/389";
                case "preston north end":
                    return "192/436";
                case "millwall":
                    return "192/704";
                case "norwich city":
                    return "192/290";
                case "sheffield united":
                    return "192/444";
                case "derby county":
                    return "192/295";
                case "nottingham forest":
                    return "192/522";
                case "leeds united":
                    return "192/183";
                case "bristol city":
                    return "192/236";
                case "aston villa":
                    return "192/238";
                case "hull city":
                    return "192/253";
                case "bolton wanderers":
                    return "192/555";



                // l1
                case "afc wimbledon":
                    return "192/419";
                case "doncaster rovers":
                    return "192/397";
                case "blackburn rovers":
                    return "192/262";
                case "milton keynes dons":
                    return "192/423";
                case "blackpool":
                    return "192/231";
                case "oldham athletic":
                    return "192/427";
                case "bristol rovers":
                    return "192/282";
                case "fleetwood town":
                    return "192/230";
                case "gillingham":
                    return "192/356";
                case "southend united":
                    return "192/765";
                case "northampton town":
                    return "192/533";
                case "peterborough united":
                    return "192/630";
                case "oxford united":
                    return "192/638";
                case "shrewsbury town":
                    return "192/773";
                case "plymouth argyle":
                    return "192/258";
                case "scunthorpe united":
                    return "192/398";
                case "rochdale":
                    return "192/425";
                case "bury":
                    return "192/156";
                case "rotherham united":
                    return "192/441";
                case "charlton athletic":
                    return "192/154";
                case "walsall":
                    return "192/521";
                case "bradford city":
                    return "192/448";
                case "wigan athletic":
                    return "192/457";
                case "portsmouth":
                    return "192/298";






                // l1
                case "cambridge united":
                    return "192/442";
                case "colchester united":
                    return "192/293";
                case "carlisle united":
                    return "192/179";
                case "mansfield town":
                    return "192/570";
                case "cheltenham town":
                    return "192/534";
                case "stevenage":
                    return "192/410";
                case "chesterfield":
                    return "192/447";
                case "coventry city":
                    return "192/571";
                case "crawley town":
                    return "192/261";
                case "yeovil town":
                    return "192/483";
                case "exeter city":
                    return "192/185";
                case "newport county afc":
                    return "192/778";
                case "grimsby town":
                    return "192/187";
                case "crewe alexandra":
                    return "192/503";
                case "lincoln city":
                    return "192/181";
                case "luton town":
                    return "192/226";
                case "morecambe":
                    return "192/273";
                case "accrington stanley":
                    return "192/579";
                case "port vale":
                    return "192/751";
                case "notts county":
                    return "192/623";
                case "swindon town":
                    return "192/391";
                case "barnet":
                    return "192/412";
                case "wycombe wanderers":
                    return "192/421";
                case "forest green rovers":
                    return "192/235";



                default:
                    return "192/0";
            }
        }

        private string GetCompetitionIconAddress(string competition)
        {
            switch (competition.ToLower())
            {
                case "premier league":
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/f/f2/Premier_League_Logo.svg/1280px-Premier_League_Logo.svg.png";
                case "championship":
                    return "https://cdn.footballleagueworld.co.uk/wp-content/uploads/2014/05/Sky_Bet_Championship.png";
                case "league one":
                    return "http://vignette3.wikia.nocookie.net/logopedia/images/e/e4/Sky_Bet_League_One.png/revision/latest?cb=20150104235046";
                case "league two":
                    return "https://vignette2.wikia.nocookie.net/logopedia/images/8/82/Sky_Bet_League_Two.png/revision/latest?cb=20150104235100";
                case "scottish premier league":
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/3/33/Scottish_Professional_Football_League.svg/1280px-Scottish_Professional_Football_League.svg.png";
                case "scottish championship":
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/1/17/Scottish_Championship.svg/1280px-Scottish_Championship.svg.png";
                default:
                    return "http://e1.365dm.com/football/badges/192/0.png"; 
            }
        }

        public BitmapImage GetBadge(string team)
        {
            var path = GetBadgeAddress(team);

            return new BitmapImage(new Uri($"http://e1.365dm.com/football/badges/{path}.png", UriKind.Absolute));
        }

        public BitmapImage GetLeagueIcon(string competition)
        {
            var path = GetCompetitionIconAddress(competition);

            return new BitmapImage(new Uri(path, UriKind.Absolute));
        }

    }
}
