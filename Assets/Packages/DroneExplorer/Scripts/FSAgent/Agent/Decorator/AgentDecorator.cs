using FSAgent.Agent.Component;
using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace FSAgent.Agent.Decorator
{
    public abstract class AgentDecorator<TargetType> :
        AgentBase<TargetType> where TargetType : BaseTargetType, new()
    {
        private AgentBase<TargetType> _wrapped_entity;
        protected TargetType _target;

        public AgentDecorator()
        {
            _target = new TargetType();
            _wrapped_entity = new NullAgent<TargetType>();
        }

        internal AgentDecorator<TargetType>
            Wrap(AgentBase<TargetType> wrapped_entity)
        {
            _wrapped_entity = wrapped_entity;
            _target = GetTarget();
            AddAction(Action, TypeDescriptor.GetClassName(this));
            return this;
        }

        protected abstract IEnumerable<int> Action();

        internal override void RefreshGenerator()
        {
            _wrapped_entity.RefreshGenerator();
        }

        internal override void ClearPredicates()
        {
            _wrapped_entity.ClearPredicates();
        }

        internal override TargetType GetTarget()
        {
            return _wrapped_entity.GetTarget();
        }
        internal override void AddAction(Func<IEnumerable<int>> action, string? name)
        {
            _wrapped_entity.AddAction(action, name);
        }
        internal override void Save(string compound_path,
            string condition_path)
        {
            _wrapped_entity.Save(compound_path,
                condition_path);
        }
        internal override void Import(string compound_path,
            string condition_path)
        {
            _wrapped_entity.Import(compound_path,
                condition_path);
        }
        internal override void CreateBehavior()
        {
            _wrapped_entity.CreateBehavior();
        }
        internal override void RunBehavior()
        {
            _wrapped_entity.RunBehavior();
        }
        internal override void PrintBehavior(string name)
        {
            _wrapped_entity.PrintBehavior(name);
        }
        internal override void DropTarget()
        {
            _wrapped_entity.DropTarget();
        }
    }
}

