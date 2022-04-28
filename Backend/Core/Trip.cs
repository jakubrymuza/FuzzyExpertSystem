using Backend.Core.Evaluables;
using System.Collections.Generic;

namespace Backend.Core
{
    public class Trip
    {
        public Dictionary<string, IEvaluable> Records { get; }

        public Trip(Dictionary<string, IEvaluable> records)
        {
            Records = records;
        }

        public Trip()
        {
            throw new System.NotImplementedException();
        }
    }
}
