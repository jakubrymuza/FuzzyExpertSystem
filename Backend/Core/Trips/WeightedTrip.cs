namespace Backend.Core.Trips
{
    public class WeightedTrip : IWeightedTrip
    {
        public ITrip Trip { get; }
        public double Weight { get; }

        public WeightedTrip(ITrip trip, double weight)
        {
            Trip = trip;
            Weight = weight;
        }

        public int CompareTo(IWeightedTrip? other) => Weight.CompareTo(other!.Weight);
    }
}
