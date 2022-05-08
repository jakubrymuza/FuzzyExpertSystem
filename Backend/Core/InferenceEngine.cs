using Backend.Auxiliary;
using Backend.Core.CalculatingEngines;
using Backend.Core.Evaluables;
using Backend.Core.QuizAnswers;
using Backend.Core.QuizQuestions;
using Backend.Core.Trips;
using Backend.Exceptions;
using System.Collections.Generic;
using System.Text.Json;

namespace Backend.Core
{
    /// <summary>
    /// represents main inference engine
    /// </summary>
    public class InferenceEngine
    {
        // ------- INTERFACE ----------

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
        public List<ITrip> GetSortedTrips(List<IQuizAnswer> quizAnswers)
        {
            SetAnswers(quizAnswers);

            return SortTrips(CalculateWeights());
        }

        /// <summary>
        /// adds rule to knowledge base
        /// </summary>
        public void AddRule(Rule rule)
        {
            _Rules.TryAdd(rule.Name, rule);

            if (rule.IsRoot)
                _RootEvaluable = rule;

            SaveRules();
        }

        /// <summary>
        /// removes rule from knowledge base by name
        /// </summary>
        public void RemoveRule(string key)
        {
            _Rules.Remove(key);
            SaveRules();
        }

        /// <summary>
        /// returns column names of trips' fuzzy description
        /// </summary>
        public string[] GetTrips() => _PropertyNames;


        /// <summary>
        /// returns rules
        /// </summary>
        public List<IEvaluable> GetRuleNames()
        {
            List<IEvaluable> result = new List<IEvaluable>();

            foreach (var rule in _Rules)
                result.Add(rule.Value);

            return result;
        }

        /// <summary>
        /// sets new root
        /// </summary>
        public void SetNewRoot(string name)
        {
            ((Rule)_RootEvaluable!).IsRoot = false;
            Rule newRoot = (Rule)FindEvaluable(name);
            newRoot.IsRoot = true;
            _RootEvaluable = newRoot;
        }

        /// <summary>
        /// return questions names
        /// </summary>
        internal List<QuizQuestion> GetQuestions()
        {
            string jsonString = System.IO.File.ReadAllText(_QuestionsPath);
            return JsonSerializer.Deserialize<List<QuizQuestion>>(jsonString)!;
        }
        

        // ------- PRIVATE FIELDS ----------

        private static InferenceEngine? _Instance;

        private Dictionary<string, IEvaluable> _Evaluables;
        private readonly Dictionary<string, IEvaluable> _Rules;
        private Dictionary<string, IEvaluable> _QuizAnswers;
        private IEvaluable? _RootEvaluable;
        private readonly List<ITrip> _Trips;

        // ------- CONSTANTS ----------

        private static readonly string _RulesPath = "../../../../Backend/KnowledgeBase/rules.json";
        private static readonly string _TripsPath = "../../../../Backend/KnowledgeBase/trips.json";
        private static readonly string _QuestionsPath = "../../../../Backend/KnowledgeBase/questions.json";
        private static readonly string[] _PropertyNames = new string[]
            {
                "Mountains",
                "Sea",
                "Temperature",
                "Distance",
                "Antiques",
                "LowPrices"
            };

        // ------- INTERNAL ----------

        internal IEvaluable FindEvaluable(string name) => _Evaluables[name];

        // ------- PRIVATE ----------

        private InferenceEngine()
        {
            _Rules = LoadRules();
            _Trips = LoadTrips();
            _QuizAnswers = new();
            _Evaluables = new();
        }

        private List<ITrip> SortTrips(double[] weights)
        {
            var tripWithWeight = new List<(double, ITrip)>(_Trips.Count);

            for (int i = 0; i < tripWithWeight.Count; ++i)
                tripWithWeight[i] = (weights[i], _Trips[i]);

            tripWithWeight.Sort();

            var results = new List<ITrip>();

            foreach (var obj in tripWithWeight)
                results.Add(obj.Item2);
            return results;
        }

        private double[] CalculateWeights()
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

            return _RootEvaluable!.Evaluate();
        }

        private void SetAnswers(List<IQuizAnswer> quizAnswers)
        {
            foreach (var answer in quizAnswers)
                _QuizAnswers.Add(answer.Name, new Record(answer));
        }

        // ------- DATA PERSISTENCE ----------
        // TODO: refactor: export to another class?

        private void SaveRules()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            // The following code is necessary to serialize all properties from Rule class
            Dictionary<string, Rule> _Rules2 = new();
            foreach (var rule in _Rules)
            {
                _Rules2.Add(rule.Key, (Rule)rule.Value);
            }
            string jsonString = JsonSerializer.Serialize(_Rules2, options);
            System.IO.File.WriteAllText(_RulesPath, jsonString);
        }

        private Dictionary<string, IEvaluable> LoadRules()
        {
            string jsonString = System.IO.File.ReadAllText(_RulesPath);
            System.Text.Json.Nodes.JsonObject rootObject = System.Text.Json.Nodes.JsonNode.Parse(jsonString)!.AsObject();
            Dictionary<string, IEvaluable> rules = new();

            foreach (var rule in rootObject)
            {
                Rule newRule = new Rule(rule.Key, rule.Value!["FirstArgumentName"]!.ToString(),
                    rule.Value!["SecondArgumentName"]!.ToString(),
                    (OperatorType)(int)rule!.Value!["OperatorType"]!,
                    (bool)rule!.Value!["IsRoot"]!);

                rules.Add(rule.Key, newRule);

                if (newRule.IsRoot)
                    _RootEvaluable = newRule;
            }

            if (_RootEvaluable == null)
                throw new RootRuleNotSet();

            return rules;
        }

        private List<ITrip> LoadTrips()
        {
            string jsonString = System.IO.File.ReadAllText(_TripsPath);
            System.Text.Json.Nodes.JsonArray rootObject = System.Text.Json.Nodes.JsonNode.Parse(jsonString)!.AsArray();
            List<ITrip> trips = new();

            foreach (var item in rootObject)
            {
                Dictionary<string, IEvaluable> records = new();

                for (int i = 0; i < _PropertyNames.Length; i++)
                {
                    string key1 = _PropertyNames[i];
                    records.Add(key1, new Record(key1, (double)item!["Records"]![key1]!["Value"]!));
                }

                trips.Add(new Trip(item!["Name"]!.ToString(), records));
            }
            return trips;
        }

    }
}
