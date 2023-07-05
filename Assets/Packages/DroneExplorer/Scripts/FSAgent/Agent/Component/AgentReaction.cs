using FSAgent.LogicObjects;

namespace FSAgent.Agent.Component
{
    internal class AgentReaction<RTargetType> : Agent<RTargetType> where

        RTargetType : BaseTargetType, new()
    {
        public AgentReaction() : base() { }

        /* 
         * Consider that status "NeedReaction" 
         * is true when the all off predicates is false (hash equal 0)
         */
        public bool IsNeedReaction()
        {
            if (_target.GetCurrentCondition() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

