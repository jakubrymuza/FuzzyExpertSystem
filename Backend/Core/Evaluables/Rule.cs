using Backend.Core.CalculatingEngines;
using Backend.Exceptions;

namespace Backend.Core.Evaluables
{
    /// <summary>
    /// Represents an inference rule
    /// </summary>
    public class Rule : IEvaluable
    {
        public string Name { get; }
        public string FirstArgumentName { get; }
        public string SecondArgumentName { get; }
        public OperatorType OperatorType { get; }
        public bool IsRoot { get; }
        public Rule(string name, string firstArgumentName, string secondArgumentName, OperatorType operatorType, bool isRoot)
        {
            Name = name;
            FirstArgumentName = firstArgumentName;
            SecondArgumentName = secondArgumentName;
            OperatorType = operatorType;
            IsRoot = isRoot;
        }

        public double Evaluate() => Evaluate(new LukasiewiczCalculatingEngine());

        public double Evaluate(ICalculatingEngine calculatingEngine)
        {
            var inferenceEngine = InferenceEngine.GetInstance();
            double arg1 = inferenceEngine.FindEvaluable(FirstArgumentName).Evaluate();
            double arg2 = inferenceEngine.FindEvaluable(SecondArgumentName).Evaluate();

            return OperatorType switch
            {
                OperatorType.TNorm => calculatingEngine.EvaluateTNorm(arg1, arg2),
                OperatorType.TConorm => calculatingEngine.EvaluateTConorm(arg1, arg2),
                OperatorType.Implication => calculatingEngine.EvaluateImplication(arg1, arg2),
                _ => throw new OperatorNotImplementedException(),
            };
        }

    }
}
