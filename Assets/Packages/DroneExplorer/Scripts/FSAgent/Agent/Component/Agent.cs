using System;
using System.IO;
using FSAgent.LogicObjects;
using System.Collections.Generic;
using System.Linq;
namespace FSAgent.Agent.Component
{
    internal class Agent<TargetType> : AgentBase<TargetType> where
        TargetType : BaseTargetType, new()
    {
        internal List<Behavior<TargetType>> _behaviors;
        internal TargetType _target;
        internal Generator<TargetType> _generator;

        public Agent()
        {
            _behaviors = new List<Behavior<TargetType>>();
            _target = new TargetType();
            _generator = new Generator<TargetType>();
            RefreshGenerator();
        }

        private int FindBehaviorFromName(string name)
        {
            int pos = 0;
            foreach (var behavior in _behaviors)
            {
                if (behavior._name == name)
                {
                    return pos;
                }
                pos++;
            }
            return -1;
        }

        internal override void RefreshGenerator()
        {
            _generator = new Generator<TargetType>(_target,
                _behaviors);
        }

        internal override TargetType GetTarget()
        {
            return _target;
        }
        internal override void AddAction(Func<IEnumerable<int>>
            action, string? name)
        {
            _behaviors.Add(new Behavior<TargetType>(name,
                default_action : action));
        }

        internal void PrepareToStart(object driver)
        {
            _target.SetDriver(driver);
            _target.Start();
        }

        internal override void DropTarget()
        {
            _target.Drop();
        }

        internal override void CreateBehavior()
        {
            _generator.Create();
        }
        internal override void RunBehavior()
        {
            _generator.Run();
        }

        internal override void ClearPredicates()
        {
            _target._predicates.Clear();
            _target._predicates.Add(Predicate.CreateWithMemorization("ISFINISH",
                false, int.MaxValue));
            _target._predicates.Add(new Predicate("ISFAIL",
                false, int.MinValue));
            foreach (var behavior in _behaviors)
            {
                behavior._conditions.Clear();
            }
        }

        // Save order is important!!!
        internal override void Save(string compound_path,
            string conditions_path)
        {
            File.WriteAllText(compound_path, "");
            File.WriteAllText(conditions_path, "");
            foreach (var behavior in _behaviors)
            {
                behavior.SaveCompoundAction(compound_path);
                behavior.SaveConditions(conditions_path);
            }
        }

        internal override void Import(string compound_path,
            string condition_path)
        {
            /*
             * Gets compound behavior 
             * Name name1 name2 name3 ...
             */
            foreach (var str_behaviour in File.ReadLines(compound_path))
            {
                string name = str_behaviour.Split(' ').First();
                Queue<Behavior<TargetType>> compound_behavior =
                    new Queue<Behavior<TargetType>>();
                foreach (var elem in
                    (from str in str_behaviour.Split(' ')
                     where str != name
                     select str)
                    )
                {
                    int pos = FindBehaviorFromName(elem);
                    compound_behavior.Enqueue(_behaviors[pos]);
                }
                _behaviors.Add(new Behavior<TargetType>(compound_action:
                    compound_behavior, name: name));
            }

            /*
             * Gets condition (start-end pairs)
             * Name key1 value1 key2 value2 ...
             */
            foreach (var str_behaviour in File.ReadLines(condition_path))
            {
                string name = str_behaviour.Split(' ').First();
                Dictionary<int, int> conditions =
                    new Dictionary<int, int>();
                int it = 1;
                int key = 0, value = 0;
                foreach (var elem in
                    (from symbol in str_behaviour.Split(' ')
                     where symbol.Count() == 1
                     select ((short)symbol.First()))
                    )
                {
                    if (it == 0)
                    {
                        value = elem;
                        conditions.Add(key, value);
                    }
                    else
                    {
                        key = elem;
                    }
                    it++;
                    it %= 2;
                }
                _behaviors[FindBehaviorFromName(name)].Import(conditions);
            }
        }

        internal override void PrintBehavior(string name)
        {
            foreach (var behavior in _behaviors)
            {
                if(behavior._name == name)
                {
                    List<string> names = behavior.
                        GetCompoundNames();
                    foreach (var action_name in names)
                    {
                        Console.Write($"{action_name} -> ");
                    }
                    foreach (var cond in behavior._conditions)
                    {
                        for (int i = 0; i < _target._predicates.Count(); ++i)
                        {
                            bool start = _target.
                            GetPredicateState(i, cond.Key);
                            bool end = _target.
                            GetPredicateState(i, cond.Value);
                            Console.WriteLine(
                                _target.
                                _predicates[i].
                                _name
                            + $": {start} -> {end}");
                        }
                        Console.WriteLine('\n');
                    }
                }
            }
        }
    }
}

