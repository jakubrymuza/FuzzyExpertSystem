namespace Backend.Core.Fuzzification
{
    public interface IFuzzificator
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }  
        public double Fuzzify(int crispValue);
    }
}
