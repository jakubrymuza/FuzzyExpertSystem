using System.Collections.Generic;
using Backend.Core.Evaluables;

namespace Backend.Core.Trips
{
    public class WeightedTrip : Trip, IWeightedTrip
    {
        public double Weight { get; }

        public WeightedTrip(string name, Dictionary<string, IEvaluable> records, double weight) : base(name, records)
        {
            Weight = weight;
        }

        public int CompareTo(IWeightedTrip? other) => Weight.CompareTo(other!.Weight);
    }
}
