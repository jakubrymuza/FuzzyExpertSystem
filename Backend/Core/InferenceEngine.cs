using Backend.Auxiliary;
using Backend.Core.CalculatingEngines;
using Backend.Core.Evaluables;
using Backend.Core.Trips;
using System;
using System.Text.Json;
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
        private IEvaluable _RootEvaluable;

        private readonly List<ITrip> _Trips;

        private static readonly string RULES_PATH = "../../../../Backend/KnowledgeBase/rules.json";
        private static readonly string TRIPS_PATH = "../../../../Backend/KnowledgeBase/trips.json";

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

        public void AddRule(string firstColumn, OperatorType operatorType, string secondColumn, string ruleName, bool isRoot)
        {
            Rule rule = new(ruleName, firstColumn, secondColumn, operatorType, isRoot);
            _Rules.TryAdd(ruleName, rule);
            if (isRoot) _RootEvaluable = rule; 
            SaveRules();
        }

        public void RemoveRule(string key)
        {
            _Rules.Remove(key);
            SaveRules();
        }

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
            System.IO.File.WriteAllText(RULES_PATH, jsonString);
        }

        private Dictionary<string, IEvaluable> LoadRules()
        {
            string jsonString = System.IO.File.ReadAllText(RULES_PATH);
            System.Text.Json.Nodes.JsonObject rootObject = System.Text.Json.Nodes.JsonNode.Parse(jsonString)!.AsObject();
            Dictionary<string, IEvaluable> rules = new();
            foreach (var rule in rootObject)
            {
                Rule newRule = new Rule(rule.Key, rule.Value!["FirstArgumentName"]!.ToString(),
                    rule.Value!["SecondArgumentName"]!.ToString(),
                    (OperatorType)(int)rule!.Value!["OperatorType"]!,
                    (bool)rule!.Value!["IsRoot"]!);
                rules.Add(rule.Key, newRule);
                if (newRule.IsRoot) _RootEvaluable = newRule;
            }
            return rules;
        }

        private Dictionary<string, IEvaluable> LoadQuizAnswers()
        {
            throw new NotImplementedException();
        }

        private List<ITrip> LoadTrips()
        {
            string jsonString = System.IO.File.ReadAllText(TRIPS_PATH);
            System.Text.Json.Nodes.JsonArray rootObject = System.Text.Json.Nodes.JsonNode.Parse(jsonString)!.AsArray();
            string[] propertyNames = new string[]
            {
                "Mountains",
                "Sea",
                "Temperature",
                "Distance",
                "Antiques",
                "LowPrices"
            };
            List<ITrip> trips = new();
            foreach (var item in rootObject)
            {
                Dictionary<string, IEvaluable> records = new();
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    string key1 = propertyNames[i];
                    records.Add(key1, new Record(key1, (double)item!["Records"]![key1]!["Value"]!));
                }
                trips.Add(new Trip(item!["Name"]!.ToString(), records));
            }
            return trips;
        }

        private IEvaluable LoadRoot()
        {
            if (_RootEvaluable == null) throw new Exception("No root evaluable set.");
            return _RootEvaluable;
        }
    }
}
