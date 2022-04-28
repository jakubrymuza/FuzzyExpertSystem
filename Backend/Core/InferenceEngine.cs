using Backend.Auxiliary;
using Backend.Core.Evaluables;
using Backend.Core.Trips;
using System;
using System.Collections.Generic;


namespace Backend.Core
{
    /// <summary>
    /// represents main inference engine
    /// </summary>
    public class InferenceEngine
    {
        private static InferenceEngine? _Instance;

        private Dictionary<string, IEvaluable>? _Evaluables;
        private readonly Dictionary<string, IEvaluable> _Rules;
        private readonly Dictionary<string, IEvaluable> _QuizAnswers;
        private readonly IEvaluable _RootEvaluable;

        private readonly List<ITrip> _Trips;

        public static InferenceEngine GetInstance()
        {
            if (_Instance == null)
                _Instance = new InferenceEngine();

            return _Instance;
        }

        public IEvaluable FindEvaluable(string name) => _Evaluables![name];

        public double[] Calculate()
        {
            double[] result = new double[_Trips.Count];

            for (int i = 0; i < _Trips.Count; i++)
                result[i] = CalculateTrip(_Trips[i]);

            return result;
        }

        private double CalculateTrip(ITrip trip)
        {
            var tripData = trip.Records;

            _Evaluables = new Dictionary<string, IEvaluable>(_Rules);
            _Evaluables.Merge(_QuizAnswers);
            _Evaluables.Merge(tripData);

            return _RootEvaluable.Evaluate();
        }

        private InferenceEngine()
        {
            // TODO: read data
            _Rules = LoadRules();
            _QuizAnswers = LoadQuizAnswers();
            _Trips = LoadTrips();
            _RootEvaluable = LoadRoot();
        }

        private Dictionary<string, IEvaluable> LoadRules()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, IEvaluable> LoadQuizAnswers()
        {
            throw new NotImplementedException();
        }

        private List<ITrip> LoadTrips()
        {
            throw new NotImplementedException();
        }

        private IEvaluable LoadRoot()
        {
            throw new NotImplementedException();
        }
    }
}
