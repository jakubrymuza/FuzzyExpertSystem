namespace Backend.Core.Evaluables
{

    /// <summary>
    /// Represents field of a trip or a quiz answer
    /// </summary>
    public class Record : IEvaluable
    {
        public string Name { get; }
        private readonly int Value;

        public Record(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public double Evaluate() => Value;
    }
}
