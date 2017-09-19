using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Insight.Views
{
    public class TipsterSetup
    {
        public string Name { get; set; } = "Default";
        public List<Dictionary<string, object>> Configs { get; } = new List<Dictionary<string, object>>();
        public List<int> Ids { get; } = new List<int>();
        public Dictionary<string, bool> SelectedLeagues { get; } = new Dictionary<string, bool>();

        public void AddSetup(Dictionary<string, object> item, int id)
        {
            Configs.Add(item);
            Ids.Add(id);
        }

        public void AddSetupRange(List<Dictionary<string, object>> items, List<int> ids)
        {
            foreach (var item in items)
            {
                Configs.Add(item);
            }
            Ids.AddRange(ids);
        }

        public void Reset()
        {
            Name = "";
            Configs.Clear();
        }

        public void SaveSetup()
        {
            var localPath = Directory.GetCurrentDirectory() + "/Configs/" + Name;
            Directory.CreateDirectory(localPath);

            var id = 1;
            foreach (var item in Configs)
            {
                var sw = new StreamWriter(localPath + "/" + id + ".config");
                foreach (var setup in item)
                {
                    sw.WriteLine(setup.Key + "@" + setup.Value);
                }
                sw.Close();
                id++;
            }

            var idWriter = new StreamWriter(localPath + "/Ids.txt");
            foreach (var currId in Ids)
            {
                idWriter.WriteLine(currId);
            }

            idWriter.Close();


            var leagueWriter = new StreamWriter(localPath + "/Leagues.txt");
            foreach (var currId in SelectedLeagues)
            {
                leagueWriter.WriteLine(currId.Key + "@" + currId.Value);
            }

            leagueWriter.Close();
        }

        public void Load(string name)
        {
            Name = name;
            var localPath = Directory.GetCurrentDirectory() + "/Configs/" + Name;
            var files = Directory.GetFiles(localPath);

            try
            {
                var ids = new List<int>();
                var reader = new StreamReader(localPath + "/Ids.txt");
                var liner = reader.ReadLine();
                while (liner != null)
                {
                    var id = int.Parse(liner);
                    ids.Add(id);
                    liner = reader.ReadLine();
                }
                reader.Close();
            }
            catch (Exception) { }


            try
            {
                int index = 0;
                foreach (var file in files)
                {
                    var sr = new StreamReader(file);

                    var items = new Dictionary<string, object>();

                    var line = sr.ReadLine();
                    while (line != null)
                    {
                        var split = line.Split('@');
                        items.Add(split[0], split[1]);
                        line = sr.ReadLine();
                    }

                    AddSetup(items, index);

                    index++;

                    sr.Close();
                }
            }
            catch (Exception) { }


            try
            {
                var leagueReader = new StreamReader(localPath + "/Leagues.txt");
                var line = leagueReader.ReadLine();
                while (line != null)
                {
                    var split = line.Split('@');
                    SelectedLeagues.Add(split[0], bool.Parse(split[1]));
                    line = leagueReader.ReadLine();
                }
                leagueReader.Close();
            }
            catch (Exception) { }

        }

        public void UpdateTeams(UIElementCollection children)
        {
            foreach (var element in children)
            {
                var checkbox = (CheckBox)element;
                SelectedLeagues.Add(checkbox.Content.ToString(), (bool)checkbox.IsChecked);
            }
        }
    }
}