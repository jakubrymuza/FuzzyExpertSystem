using Backend.Core.Evaluables;
using System.Collections.Generic;

namespace Backend.Core.Trips
{
    public class Trip : ITrip
    {
        public string Name { get; }

        public Dictionary<string, IEvaluable> Records => _Records;

        public Dictionary<string, IEvaluable> _Records;

        public Trip(string name, Dictionary<string, IEvaluable> records)
        {
            Name = name;
            _Records = records;
        }
    }
}
