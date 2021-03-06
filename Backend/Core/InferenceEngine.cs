using Backend.Auxiliary;
using Backend.Core.Evaluables;
using Backend.Core.QuizAnswers;
using Backend.Core.Trips;
using Backend.Exceptions;
using System.Collections.Generic;

namespace Backend.Core
{
    /// <summary>
    /// represents main inference engine
    /// </summary>
    public class InferenceEngine
    {
        // ---------- INTERFACE ----------

        /// <summary>
        /// returns instance of the inference engine
        /// </summary>
        public static InferenceEngine GetInstance()
        {
            if (_Instance == null)
                _Instance = new InferenceEngine();

            return _Instance;
        }

        /// <summary>
        /// returns trips sorted in accordance to rules from knowledge base and quiz answers
        /// </summary>
        public List<IWeightedTrip> GetSortedTrips(List<IQuizAnswer> quizAnswers)
        {
            SetAnswers(quizAnswers);

            return SortTrips(CalculateWeights());
        }

        // ---------- INTERNAL ---------- 

        internal IEvaluable FindEvaluable(string name)
        {
            var evaluable = _Evaluables[name];

            if (evaluable == null)
                throw new EvaluableNotFoundException(name);

            return evaluable;
        }

        // ---------- PRIVATE ---------- 

        private KnowledgeBase _KnowledgeBase;
        private static InferenceEngine? _Instance;
        private Dictionary<string, IEvaluable> _Evaluables;
        private readonly Dictionary<string, IEvaluable> _QuizAnswers;

        private InferenceEngine()
        {
            _KnowledgeBase = KnowledgeBase.GetInstance();
            _QuizAnswers = new();
            _Evaluables = new();
        }

        private List<IWeightedTrip> SortTrips(double[] weights)
        {
            var trips = _KnowledgeBase.GetTripsList();
            var tripWithWeight = new WeightedTrip[trips.Count];

            for (int i = 0; i < tripWithWeight.Length; ++i)
                tripWithWeight[i] = new WeightedTrip(((Trip)trips[i]).Name, trips[i].Records, weights[i]);

            var tripWithWeightL = new List<IWeightedTrip>(tripWithWeight);

            tripWithWeightL.Sort();
            tripWithWeightL.Reverse();

            return tripWithWeightL;
        }

        private double[] CalculateWeights()
        {
            var trips = _KnowledgeBase.GetTripsList();
            double[] result = new double[trips.Count];

            for (int i = 0; i < trips.Count; i++)
                result[i] = CalculateTrip(trips[i]);

            return result;
        }

        private double CalculateTrip(ITrip trip)
        {
            _Evaluables.Clear();
            _Evaluables.Merge(_QuizAnswers);
            _Evaluables.Merge(trip.Records);
            _Evaluables.Merge(_KnowledgeBase.GetRulesDict());

            return _KnowledgeBase.GetRoot().Evaluate();
        }

        private void SetAnswers(List<IQuizAnswer> quizAnswers)
        {
            _QuizAnswers.Clear();

            foreach (var answer in quizAnswers)
                _QuizAnswers.Add(answer.Name, new Record(answer));
        }

    }
}
