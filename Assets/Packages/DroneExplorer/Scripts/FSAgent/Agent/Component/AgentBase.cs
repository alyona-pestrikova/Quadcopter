using System;
using System.Collections.Generic;
namespace FSAgent.Agent.Component
{
    public abstract class AgentBase<TargetType> where TargetType : BaseTargetType
    {
        internal abstract void DropTarget();
        internal abstract void CreateBehavior();
        internal abstract void Save(string compound_path,
            string condition_path);
        internal abstract void Import(string compound_path,
            string condition_path);
        internal abstract void RunBehavior();
        internal abstract void RefreshGenerator();
        internal abstract void ClearPredicates();
        internal abstract void PrintBehavior(string name);
        internal abstract void AddAction(Func<IEnumerable<int>>
            action, string? name);
        internal abstract TargetType GetTarget();
    }
}
