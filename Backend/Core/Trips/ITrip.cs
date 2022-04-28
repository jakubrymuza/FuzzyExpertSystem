using Backend.Core.Evaluables;
using System.Collections.Generic;

namespace Backend.Core.Trips
{
    public interface ITrip
    {
        public Dictionary<string, IEvaluable> Records { get; }
    }
}
