namespace Backend.Core.Fuzzification
{
    public class LinearFuzzificator : IFuzzificator
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public LinearFuzzificator(int min, int max)
        {
            MinValue = min;
            MaxValue = max;
        }

        public double Fuzzify(int crispValue)
        {
            if (crispValue > MaxValue || crispValue < MinValue)
                throw new System.ArgumentOutOfRangeException();

            return (crispValue - MinValue) / (MaxValue - MinValue);
        }
    }
}
