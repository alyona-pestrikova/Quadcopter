using FSAgent.Agent.Component;
using FSAgent.LogicObjects;
using UnityEngine;
public class DroneAdaptive : BaseTargetType
{
    Condition _current_condition;

    public DroneAdaptive() : base()
	{
        _current_condition = new Condition(_predicates);
	}
    public override void Start()
    {
        //_predicates.Add(new Predicate)
    }
    public override void Alarm()
    {
        Debug.Log("Alarm!");
    }
    public override void Drop()
    {
        return;
    }
    public override void Freeze()
    {
        return;
    }
    public override string GetCompoundBehaviourName()
    {
        return "New";
    }
    public override Condition GetCurrentCondition()
    {
        return _current_condition;
    }
    public override void Log(string body)
    {
        return;
    }
    public override void TargetReset()
    {
        return;
    }
    public override void UnFreeze()
    {
        return;
    }
}

