namespace Backend.Core.CalculatingEngines
{
    /// <summary>
    /// contains methods for calculating operator's values
    /// </summary>
    public interface ICalculatingEngine
    {
        public double EvaluateTNorm(double x, double y);

        public double EvaluateTConorm(double x, double y);

        public double EvaluateImplication(double x, double y);
    }
}
