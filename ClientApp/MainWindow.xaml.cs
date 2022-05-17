using Backend.Core;
using Backend.Core.QuizAnswers;
using Backend.Core.Trips;
using ClientApp.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            KnowledgeBase knowledgebase = KnowledgeBase.GetInstance();
            var questions = knowledgebase.GetQuestions();

            List<QuizItem> items = new List<QuizItem>();
            foreach (var question in questions)
            {
                items.Add(new QuizItem(question.Name, question.Description));
            }


            UserFormList.ItemsSource = items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SuggestionListView.ItemsSource = null;

            List<IQuizAnswer> answerList = new List<IQuizAnswer>();

            foreach (QuizItem tmp in UserFormList.Items)
            {
                answerList.Add(new QuizAnswer(tmp.name, tmp.val));
            }

            InferenceEngine engine = InferenceEngine.GetInstance();

            var TripsList = engine.GetSortedTrips(answerList);


            SuggestionListView.ItemsSource = TripsList;


            return;
        }

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = ((ListViewItem)sender).Content as WeightedTrip;
            string message = string.Empty;
            message += $"Góry: {item!.Records["Góry - wycieczka"].Evaluate() * 10}\n";
            message += $"Morze: {item.Records["Morze - wycieczka"].Evaluate() * 10}\n";
            message += $"Odległość: {item.Records["Odległość - wycieczka"].Evaluate() * 10}\n";
            message += $"Temperatura: {item.Records["Temperatura - wycieczka"].Evaluate() * 10}\n";
            message += $"Zabytki: {item.Records["Zabytki - wycieczka"].Evaluate() * 10}\n";
            message += $"Niskie ceny: {item.Records["Niskie ceny - wycieczka"].Evaluate() * 10}\n";
            MessageBox.Show(message, item.Name, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }


}
