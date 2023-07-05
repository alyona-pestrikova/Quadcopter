using System.IO;
using FSAgent.Agent.Component;
using System;
using System.Collections.Generic;
namespace FSAgent.LogicObjects
{
    internal class Behavior<TargetType> where TargetType : BaseTargetType
    {
        internal string? _name;
        // Count of default action which constited by this behavior
        public int Size { private set; get; }
        // Level of action
        public int Level { private set; get; }

        private Queue<Behavior<TargetType>>?
            _compound_action;

        private Func<IEnumerable<int>>?
            _default_action;

        // Key - start condition hash
        // Value - end condition hash
        internal Dictionary<int, int> _conditions;

        internal Behavior(string? name = null,
            Queue<Behavior<TargetType>>?
            compound_action = null, Func<IEnumerable<int>>?
            default_action = null)
        {
            _default_action = default_action;
            _compound_action = compound_action;
            if (_default_action == null
                && _compound_action == null)
            {
                throw new ArgumentNullException();
            }
            _name = name;
            _conditions = new Dictionary<int, int>();

            if (_default_action != null)
            {
                Size = 1;
                Level = 1;
            }
            else
            {
                int max_level = 1;
                foreach (var behaviour in _compound_action)
                {
                    if (behaviour.Level > max_level)
                    {
                        max_level = behaviour.Level;
                    }
                    Size += behaviour.Size;
                }
                Level = max_level + 1;
            }
        }

        internal IEnumerable<int> Run()
        {
            // Executing action step by step
            if (_compound_action != null)
            {
                foreach (var action in
                    _compound_action)
                {
                    foreach (var move in
                    action.Run())
                    {
                        yield return 0;
                    }
                }
            }
            if (_default_action != null)
            {
                foreach (var move in
                    _default_action.Invoke())
                {
                    yield return 0;
                }
            }
        }

        internal List<string> GetCompoundNames()
        {
            List<string> names = new List<string>();
            if (_compound_action != null)
            {
                foreach (var action in
                    _compound_action)
                {
                    names.Add(action._name ??
                        "UnknownAction");
                }
            }
            return names;
        }

        // Imports condition Dict
        internal void Import(Dictionary<int, int> conditions)
        {
            _conditions = conditions;  
        }
        // Saves condition Dict
        internal void SaveConditions(string path)
        {
            string output = _name ??
                "UnknownAction";
            foreach (var condition in _conditions)
            {
                output += $" {condition.Key} {condition.Value}";
            }
            File.AppendAllText(path, output);
        }
        // Saves compound action
        internal void SaveCompoundAction(string path)
        {
            string output = _name ??
                "UnknownAction";
            foreach (var name in GetCompoundNames())
            {
                output += $" {name}";
            }
            File.AppendAllText(path, output);
        }
    }
}

