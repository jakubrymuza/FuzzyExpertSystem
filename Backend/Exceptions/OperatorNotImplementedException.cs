using System;

namespace Backend.Exceptions
{
    internal class OperatorNotImplementedException : Exception
    {
        public OperatorNotImplementedException(string operatorName) : base($"No operator found with name {operatorName}")
        {
            OperatorName = operatorName;
        }

        public string OperatorName { get; init; }
    }
}
