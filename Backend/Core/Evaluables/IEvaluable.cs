namespace Backend.Core.Evaluables
{
    /// <summary>
    /// represents an entity evaluable by inference engine
    /// </summary>
    public interface IEvaluable
    {
        public string Name { get; }
        public double Evaluate();
    }
}
