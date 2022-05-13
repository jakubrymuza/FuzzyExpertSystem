using System;

namespace Backend.Core.Trips
{
    public interface IWeightedTrip : IComparable<IWeightedTrip>, ITrip
    {
        public double Weight { get; }
    }
}
