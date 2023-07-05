using System.Collections.Generic;
namespace FSAgent.LogicObjects
{
    public class Condition
	{
        // Value of predicate chain in decimal number system
        internal int _hash;

        public Condition(int hash)
        {
            _hash = hash;
        }

        public Condition(List<Predicate> predicates)
        {
            int pow = 1;
            _hash = 0;
            foreach (var predicate in predicates)
            {
                if(predicate._state)
                {
                    _hash += pow;
                }
                pow *= 2;
            }
        }

        // condition hash ~ condition
        public static implicit operator
            int(Condition condition)
        {
            return condition._hash;
        }

        public static implicit operator
            Condition(int condition)
        {
            return new Condition(condition);
        }

    }
}

