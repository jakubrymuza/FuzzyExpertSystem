using System;

namespace Backend.Core.CalculatingEngines
{
    /// <summary>
    /// contains Lukasiewicz fuzzy operations
    /// </summary>
    internal class LukasiewiczCalculatingEngine : ICalculatingEngine
    {
        public double EvaluateTNorm(double x, double y) =>
            Math.Max(0, x + y - 1);

        public double EvaluateTConorm(double x, double y) =>
            Math.Min(1, x + y);

        public double EvaluateImplication(double x, double y) =>
            Math.Min(1, 1 - x + y);
    }
}
