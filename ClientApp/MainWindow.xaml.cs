using System.Windows;
using Backend.Core;
using System.Collections.Generic;
using Backend.Core.QuizAnswers;
using Backend.Core.QuizQuestions;
using Backend.Data;
using System;
using ClientApp.Models;

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
            foreach (var question in questions) {
                items.Add(new QuizItem(question.Name, question.Description));
            }


            UserFormList.ItemsSource = items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SuggestionListView.ItemsSource = null;

            List<IQuizAnswer> answerList = new List<IQuizAnswer>();

            foreach (QuizItem tmp in UserFormList.Items) {
                answerList.Add(new QuizAnswer(tmp.name,tmp.val));
            }

            InferenceEngine engine = InferenceEngine.GetInstance();

            var TripsList = engine.GetSortedTrips(answerList);


            SuggestionListView.ItemsSource = TripsList;


            return;
        }


    }


}
