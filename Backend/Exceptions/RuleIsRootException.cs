using System;


namespace Backend.Exceptions
{
    public class RuleIsRootException : Exception
    {
        public RuleIsRootException(string ruleName) : base($"Can not remove root rule: {ruleName}")
        {
            RuleName = ruleName;
        }

        public string RuleName { get; init; }
    }
}
