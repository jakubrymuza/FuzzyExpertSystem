namespace Backend.Core.QuizAnswers
{
    internal class QuizAnswer : IQuizAnswer
    {
        public string Name { get; }

        public int CrispValue { get; }

        public QuizAnswer(string name, int crispValue)
        {
            Name = name;
            CrispValue = crispValue;
        }
    }
}
