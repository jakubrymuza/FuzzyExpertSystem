using Backend.Core.Evaluables;
using System.Collections.Generic;

namespace Backend.Core.Trips
{
    public class Trip : ITrip
    {
        public string Name { get; }
        public Dictionary<string, IEvaluable> Records { get; }

        public Trip(string name, Dictionary<string, IEvaluable> records)
        {
            Name = name;
            Records = records;
        }

        public Trip()
        {
            throw new System.NotImplementedException();
        }
    }
}
