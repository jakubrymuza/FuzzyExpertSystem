using System;

namespace Backend.Exceptions
{
    internal class EvaluableNotFoundException : Exception
    {
        public EvaluableNotFoundException(string evaluableName) : base($"No evaluable found with name {evaluableName}")
        {
            OperatorName = evaluableName;
        }

        public string OperatorName { get; init; }
    }
}