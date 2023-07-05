using FSAgent.LogicObjects;
using System.Collections.Generic;
namespace FSAgent.Agent.Component
{
    public abstract class BaseTargetType
    {
        // We need these only for seeing
        public readonly List<Predicate> _predicates;

        // Accessor between agent and executer
        public object _driver;

        public BaseTargetType()
        {
            _predicates = new List<Predicate>()
            {
                Predicate.CreateWithMemorization("ISFINISH",
                false, int.MaxValue),
                new Predicate("ISFAIL",
                false, int.MinValue)
            };
            _driver = new object();
        }


        // Reset last movement done by agent
        public abstract void TargetReset();

        // Calls when agent doesn't know what it should to do
        public abstract void Alarm();

        // Should return current target condition
        public abstract Condition GetCurrentCondition();

        // Writes log
        public abstract void Log(string body);

        // Runs when agent is ready to work
        public abstract void Start();

        // Should return to default state
        public abstract void Drop();

        // Should requests --|-- name from user
        public abstract string GetCompoundBehaviourName();

        // Should to freeze learning platform
        public abstract void Freeze();

        // Should to unfreeze learning platform
        public abstract void UnFreeze();

        public int FindPredicate(string name)
        {
            for (int i = 0; i < _predicates.Count; i++)
            {
                if (_predicates[i]._name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        internal bool GetPredicateState(int
            position, Condition condition)
        {
            int pow = 2;
            for (int i = 0; i < position; i++)
            {
                pow *= 2;
            }
            return condition % pow == 1 ? true : false;
        }

        internal int GetRewardFromCodition(Condition condition)
        {
            int pos = 0;
            int reward = 0;
            foreach (var predicate in _predicates)
            {
                if (GetPredicateState(pos,
                    condition))
                {
                    reward += _predicates[pos]._reward;
                }
                pos++;
            }
            return reward;
        }

        // Considers that finish bit is first bit
        internal bool IsFinish(Condition condition)
        {
            return condition % 2 == 1 ? true : false;
        }
        // Considers that fail bit is second bit
        internal bool IsFail(Condition condition)
        {
            return condition % 4 == 1 ? true : false;
        }

        internal bool IsNeedToRemember(Condition condition)
        {
            int pos = 0;
            foreach (var predicate in _predicates)
            {
                if (predicate.IsNeedToRemember &&
                    GetPredicateState(pos,
                    condition))
                {
                    return true;
                }
                pos++;
            }
            return false;
        }

        internal void SetDriver(object driver)
        {
            _driver = driver ?? new object();
        }
    }
}

