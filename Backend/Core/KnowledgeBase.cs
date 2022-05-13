using Backend.Core.CalculatingEngines;
using Backend.Core.Evaluables;
using Backend.Core.QuizQuestions;
using Backend.Core.Trips;
using Backend.Exceptions;
using System.Collections.Generic;
using System.Text.Json;


namespace Backend.Core
{
    public class KnowledgeBase
    {
        // ---------- INTERFACE ---------- 

        /// <summary>
        /// returns instance of the knowledge base
        /// </summary>
        public static KnowledgeBase GetInstance()
        {
            if (_Instance == null)
                _Instance = new KnowledgeBase();

            return _Instance;
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
        /// returns trips list
        /// </summary>
        public List<ITrip> GetTripsList() => _Trips;


        /// <summary>
        /// returns rules
        /// </summary>
        public List<IEvaluable> GetRules()
        {
            List<IEvaluable> result = new List<IEvaluable>();

            foreach (var rule in _Rules)
                result.Add(rule.Value);

            return result;
        }

        /// <summary>
        /// returns rules dictionary
        /// </summary>
        public Dictionary<string, IEvaluable> GetRulesDict() => _Rules;

        /// <summary>
        /// sets new root
        /// </summary>
        public void SetNewRoot(string name)
        {
            ((Rule)_RootEvaluable!).IsRoot = false;
            Rule newRoot = (Rule)_Rules[name];
            newRoot.IsRoot = true;
            _RootEvaluable = newRoot;
            SaveRules();
        }

        /// <summary>
        /// returns questions names
        /// </summary>
        public List<QuizQuestion> GetQuestions()
        {
            string jsonString = System.IO.File.ReadAllText(_QuestionsPath);
            return JsonSerializer.Deserialize<List<QuizQuestion>>(jsonString)!;
        }

        /// <summary>
        /// returns root evaluable
        /// </summary>
        public IEvaluable GetRoot() => _RootEvaluable!;

        // ---------- PRIVATE ---------- 

        private static KnowledgeBase? _Instance;
        private readonly Dictionary<string, IEvaluable> _Rules;
        private Dictionary<string, IEvaluable> _QuizAnswers;
        private IEvaluable? _RootEvaluable;
        private readonly List<ITrip> _Trips;

        private static readonly string _RulesPath = "../../../../Backend/Data/rules.json";
        private static readonly string _TripsPath = "../../../../Backend/Data/trips.json";
        private static readonly string _QuestionsPath = "../../../../Backend/Data/questions.json";
        private static readonly string[] _PropertyNames = new string[]
            {
                "Mountains",
                "Sea",
                "Temperature",
                "Distance",
                "Antiques",
                "LowPrices"
            };

        private KnowledgeBase()
        {
            _Rules = LoadRules();
            _Trips = LoadTrips();
            _QuizAnswers = new();
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