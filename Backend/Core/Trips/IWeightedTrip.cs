using System;

namespace Backend.Core.Trips
{
    public interface IWeightedTrip : IComparable<IWeightedTrip>
    {
        public ITrip Trip { get; }
        public double Weight { get; }
    }
}
