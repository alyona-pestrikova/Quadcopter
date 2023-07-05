using FSAgent.LogicObjects;
namespace FSAgent.Agent.Component
{
    internal class AgentAdaptive<TargetType> : Agent<TargetType> where
        TargetType : BaseTargetType, new()
    {
        public AgentAdaptive() : base() { }

        public void CancelExecute()
        {
            _generator.IsCancel = true;
        }
    }
}

