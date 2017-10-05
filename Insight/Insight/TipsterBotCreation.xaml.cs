using Insight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Insight
{
    /// <summary>
    /// Interaction logic for TipsterBot.xaml
    /// </summary>
    public partial class TipsterBotCreation : Window
    {
        List<Dictionary<string, object>> _items = new List<Dictionary<string, object>>();
        List<int> _itemIds = new List<int>();

        int _selectedId = -1;
        int _selectedIndex = -1;

        public TipsterBotCreation()
        {
            InitializeComponent();
            InitialiseButtons();
        }

        private void InitialiseButtons()
        {
            cmdBtts.SetValues("Both Teams To Score", "Will both teams score in the match?");
            cmdHalfWithMostGoals.SetValues("Half with most Goals", "Which half will have the most goals?");
            cmdOutright.SetValues("Outright Win", "Which team will win?");
            cmdOver1.SetValues("Over 1.5 Goals", "Will there be 2 or more goals in the match?");
            cmdPlayerToScore.SetValues("Player to Score in 90 minutes", "Which players will score in the match?");
        }

        public void ShowNewSetupOptions(object sender, MouseEventArgs e)
        {

            UpdateDetails();

            try
            {
                ((Label)SelectedElements.Children[_selectedIndex + 2]).Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
            catch (Exception) { }

            _selectedIndex = -1;

            NewComponentButton.Background = new SolidColorBrush(Color.FromArgb(67, 32, 195, 179));
            ConfigurationGrid.Visibility = Visibility.Hidden;
            NewItemGrid.Visibility = Visibility.Visible;
        }

        private void UpdateDetails()
        {
            if (_selectedIndex > -1)
            {
                _items[_selectedIndex].Clear();
            }

            foreach (var view in StckConfig.Children)
            {
                var name = "";
                object value = null;

                if (view.GetType() == typeof(SliderBar))
                {
                    name = ((SliderBar)view).Name;
                    value = ((SliderBar)view).TheSlider.Value;
                }
                else if (view.GetType() == typeof(SliderBarWithToggle))
                {
                    name = ((SliderBarWithToggle)view).Name;
                    var topChecked = ((SliderBarWithToggle)view).rdBtn1.IsChecked;
                    if (topChecked == true) { value = -1; }
                    else
                    {
                        value = ((SliderBarWithToggle)view).TheSlider.TheSlider.Value;
                    }
                }
                else if (view.GetType() == typeof(ConfigCheckbox))
                {
                    name = ((ConfigCheckbox)view).Name;
                    value = ((ConfigCheckbox)view).checkBox.IsChecked;
                }

                if (_selectedIndex > -1)
                {
                    _items[_selectedIndex].Add(name, value);
                }
            }
        }

        private void BetTypeClicked(object sender, MouseButtonEventArgs e)
        {
            ConfigurationGrid.Visibility = Visibility.Visible;
            NewItemGrid.Visibility = Visibility.Hidden;

            var tag = ((BetTypeSelection)sender).Tag.ToString();
            var id = int.Parse(tag);

            _items.Add(new Dictionary<string, object>());
            _itemIds.Add(id);

            // add item to list
            Label l = new Label();
            l.Content = ((BetTypeSelection)sender).txtName.Content;
            l.Tag = (_items.Count - 1).ToString();

            Uri Uri = new Uri("pack://application:,,,/Insight;component/Fonts/", UriKind.RelativeOrAbsolute);

            l.FontFamily = new FontFamily(Uri, "Abel-Regular.tff");
            l.FontSize = 12;
            l.MouseDown += ShowConfig;
            l.Height = 40;
            l.Padding = new Thickness(10, 3, 10, 3);
            l.VerticalContentAlignment = VerticalAlignment.Center;
            SelectedElements.Children.Add(l);

            // load UI objects
        }

        private void ShowConfig(object sender, MouseButtonEventArgs e)
        {

            UpdateDetails();

            ConfigurationGrid.Visibility = Visibility.Visible;
            NewItemGrid.Visibility = Visibility.Hidden;

            var tag = ((Label)sender).Tag.ToString();
            var index = int.Parse(tag);

            NewComponentButton.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

            try
            {
                ((Label)SelectedElements.Children[_selectedIndex + 2]).Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
            catch (Exception)
            { }
            _selectedId = _itemIds[index];
            _selectedIndex = index;
            ((Label)SelectedElements.Children[_selectedIndex + 2]).Background = new SolidColorBrush(Color.FromArgb(67, 32, 195, 179));


            DisplayElements();
        }

        private void DisplayElements()
        {
            StckConfig.Children.Clear();

            // outright
            if (_selectedId == 0)
            {
                // todo: get any existing details
                AddSliderBarWithToggle("Number of games to consider", 1, 10, 1, 3, "Consider full season", "Consider recent form", "Games_Considered");
                AddCheckbox("Home games only", false, "Home_Only");
                AddCheckbox("Combine bets into Accumulators", true, "Accumulators");
                AddSlider("Minimum confidence level", 1, 100, 1, 50, "Confidence");
            }

            // btts
            if (_selectedId == 1)
            {
                // todo: get any existing details
                AddSliderBarWithToggle("Number of games to consider", 1, 10, 1, 3, "Consider full season", "Consider recent form", "Games_Considered");
            }

            // over 1
            if (_selectedId == 2)
            {
                // todo: get any existing details

                AddSliderBarWithToggle("Number of games to consider", 1, 10, 1, 3, "Consider full season", "Consider recent form", "Games_Considered");
            }
            // to score in 90 minutes
            if (_selectedId == 3)
            {
                // todo: get any existing details

                AddSliderBarWithToggle("Number of games to consider", 1, 10, 1, 3, "Consider full season", "Consider recent form", "Games_Considered");
                AddCheckbox("Consider Opponent Position", true, "Opponent_Position");
            }
            // half with most goals
            if (_selectedId == 4)
            {
                // todo: get any existing details
                AddSlider("Minimum percentage of goals in a half", 55, 100, 5, 60, "Minimum_Percentage");
                AddSliderBarWithToggle("Number of games to consider", 55, 100, 5, 60, "Consider full season", "Consider recent form", "Games_Considered");
            }
        }

        private void AddSlider(string title, int minVal, int maxVal, int steps, int current, string name)
        {
            SliderBar sBar = new SliderBar();
            sBar.SetValues(minVal, maxVal, steps, title, current);
            sBar.lblTitle.Text = title;
            sBar.Height = 80;
            sBar.TheSlider.Minimum = minVal;
            sBar.TheSlider.Maximum = maxVal;
            sBar.TheSlider.SmallChange = steps;
            sBar.Name = name;
            StckConfig.Children.Add(sBar);
        }

        private void AddCheckbox(string text, bool isChecked, string name)
        {
            ConfigCheckbox sBar = new ConfigCheckbox();
            sBar.Name = name;
            sBar.SetValues(text, isChecked);
            sBar.Height = 40;
            StckConfig.Children.Add(sBar);
        }

        private void AddSliderBarWithToggle(string title, int minVal, int maxVal, int steps, int current, string opt1, string opt2, string name)
        {
            SliderBarWithToggle sBar = new SliderBarWithToggle();
            sBar.SetValues(minVal, maxVal, steps, title, current, opt1, opt2);
            sBar.rdBtn1.Content = opt1;
            sBar.rdBtn2.Content = opt2;
            sBar.Height = 150;
            sBar.TheSlider.lblTitle.Text = title;
            sBar.TheSlider.TheSlider.Minimum = minVal;
            sBar.TheSlider.TheSlider.Maximum = maxVal;
            sBar.TheSlider.TheSlider.SmallChange = steps;
            sBar.Name = name;
            StckConfig.Children.Add(sBar);
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            UpdateDetails();
                        
            var tipsterSetup = new TipsterSetup();
            tipsterSetup.Name = txtName.Text;

            tipsterSetup.UpdateTeams(SelectedLeagues.Children);
            tipsterSetup.AddSetupRange(_items, _itemIds);
            tipsterSetup.SaveSetup();

            MainWindow.BotConfigs.Add(tipsterSetup);
                
            Owner.Show();
            this.Close();
        }

        private void cmdLeagues_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedLeagues.Visibility = Visibility.Visible;
            SelectedElements.Visibility = Visibility.Hidden;
            cmdLeagues.BorderThickness = new Thickness(0, 0, 0, 4);
            cmdComponents.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void cmdComponents_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedLeagues.Visibility = Visibility.Hidden;
            SelectedElements.Visibility = Visibility.Visible;
            cmdLeagues.BorderThickness = new Thickness(0, 0, 0, 0);
            cmdComponents.BorderThickness = new Thickness(0, 0, 0, 4);
        }



        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

    }
}
