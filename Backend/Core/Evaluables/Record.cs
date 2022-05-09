using Backend.Core.Fuzzification;
using Backend.Core.QuizAnswers;

namespace Backend.Core.Evaluables
{

    /// <summary>
    /// Represents field of a trip or a quiz answer
    /// </summary>
    public class Record : IEvaluable
    {
        public string Name { get; }
        private readonly double Value;

        public Record(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public Record(IQuizAnswer quizAnswer)
        {
            Name = quizAnswer.Name;

            var fuzzificator = new LinearFuzzificator(1, 10);

            Value = fuzzificator.Fuzzify(quizAnswer.CrispValue);
        }

        public Record(IQuizAnswer quizAnswer, IFuzzificator fuzzificator)
        {
            Name = quizAnswer.Name;

            Value = fuzzificator.Fuzzify(quizAnswer.CrispValue);
        }

        public double Evaluate() => Value;
    }
}
