using Backend.Core;
using Backend.Core.CalculatingEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ExpertApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        InferenceEngine engine; 
        public MainWindow()
        {
            InitializeComponent();
            engine = InferenceEngine.GetInstance();
            RefreshAllRulesLists();
            this.FirstPreference.SelectedIndex = 0;
            this.ProcessMethod.SelectedIndex = 0;
            this.SecondPreference.SelectedIndex = 0;
            this.RulesToEdit.SelectedIndex = 0;

            this.ProcessMethod.ItemsSource = Enum.GetValues(typeof(OperatorType)).Cast<OperatorType>();
            
        }
        private void Save(object sender, RoutedEventArgs e)
        {

            if (NewPreferenceName.Text == "")
            {
                MessageBox.Show(this, "Nie podano nazwy reguły","Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                engine.AddRule(new Backend.Core.Evaluables.Rule(NewPreferenceName.Text, (string)FirstPreference.SelectedItem, (string)SecondPreference.SelectedItem, (OperatorType)ProcessMethod.SelectedItem, false));
                SetRoot(NewPreferenceName.Text);
                RefreshAllRulesLists();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            engine.RemoveRule((string)this.RulesToEdit.SelectedItem);
            RefreshAllRulesLists();
        }
        private void RootButton_Click(object sender, RoutedEventArgs e)
        {
            SetRoot((string)this.RulesToEdit.SelectedItem);
        }
        private void RefreshAllRulesLists()
        {
            var Preferences = engine.GetRuleNames().Select(x => x.Name);
            var Rules = engine.GetRuleNames().Select(x => x.Name);
            this.FirstPreference.ItemsSource = Preferences;
            this.SecondPreference.ItemsSource = Preferences;
            this.RulesToEdit.ItemsSource = Rules;
            this.FirstPreference.Items.Refresh();
            this.SecondPreference.Items.Refresh();
            this.RulesToEdit.Items.Refresh();
        }
        private void SetRoot(string name)
        {
            //engine.SetNewRoot(name);
            CurrentRoot.Text=name;
        }

        
    }
}
