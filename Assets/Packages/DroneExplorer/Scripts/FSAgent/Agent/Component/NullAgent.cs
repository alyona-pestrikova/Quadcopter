using System.Collections.Generic;
using System;

namespace FSAgent.Agent.Component
{
    // Default verison of agentbase object
    internal class NullAgent<TargetType> : AgentBase<TargetType>
        where TargetType : BaseTargetType, new()
    {
        internal override void DropTarget() { }
        internal override void Import(string compound_path,
            string condition_path) { }
        internal override void Save(string compound_path,
            string condition_path) { }
        internal override void RefreshGenerator() { }
        internal override void CreateBehavior() { }
        internal override void RunBehavior() { }
        internal override void ClearPredicates() { }
        internal override void PrintBehavior(string name) { }
        internal override void AddAction(Func<IEnumerable<int>>
            action, string? name)
        { }
        internal override TargetType GetTarget() => new TargetType();
    }
}

