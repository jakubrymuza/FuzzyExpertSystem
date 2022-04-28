using System.Collections.Generic;
using System.Windows;

namespace ExpertApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.FirstPreference.Items.Clear();
            this.SecondPreference.Items.Clear();
            this.FirstPreference.ItemsSource = Preferences;
            this.SecondPreference.ItemsSource = Preferences;
            this.ProcessMethod.ItemsSource = ProcessMethods;
        }
        List<string> Preferences = new List<string>{ "Pref1", "Pref2" }, ProcessMethods = new List<string>{ "Method1", "Method2" };
        private void Save(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(this, (string)FirstPreference.SelectedItem+ (string)ProcessMethod.SelectedItem + (string)SecondPreference.SelectedItem + NewPreferenceName.Text,
"nono", MessageBoxButton.OK, MessageBoxImage.Information);
            if(false)
            {
                MessageBox.Show(this, "bad as fuck",
"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            Preferences.Add(this.NewPreferenceName.Text);
            this.FirstPreference.Items.Refresh();
            this.SecondPreference.Items.Refresh();
        }
    }
}
