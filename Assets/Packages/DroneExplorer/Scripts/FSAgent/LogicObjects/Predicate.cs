namespace FSAgent.LogicObjects
{
	public class Predicate
	{
        internal string _name;
        public bool _state;

        /*
            If equal true -
            we remember chain where _state=true
        */
        internal bool IsNeedToRemember { get; private set;  }
        internal int _reward;

        public Predicate(string name,
            bool state, int reward)
        {
            _name = name;
            _reward = reward;
            _state = state;
            IsNeedToRemember = false;
        }

        public static Predicate
            CreateWithMemorization(string name,
            bool state, int reward)
        {
            return new Predicate(name,
                state, reward).Memorize();
        }

        private Predicate Memorize()
        {
            IsNeedToRemember = true;
            return this;
        }
    }
}


