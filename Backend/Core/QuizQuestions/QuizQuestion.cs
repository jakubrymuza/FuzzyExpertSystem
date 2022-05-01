namespace Backend.Core.QuizQuestions
{
    internal class QuizQuestion
    {
        public string Name { get; }
        public string Description { get; }

        public QuizQuestion(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
