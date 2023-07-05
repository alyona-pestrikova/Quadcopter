using FSAgent.Agent.Component;
using FSAgent.LogicObjects;
using UnityEngine;
public class DroneReaction : BaseTargetType
{
    public DroneReaction() : base()
    {

    }
    public override void Start()
    {
        Debug.Log("Start!");
        //_predicates.Add(new Predicate)
    }
    public override void Alarm()
    {
        throw new System.NotImplementedException();
    }
    public override void Drop()
    {
        throw new System.NotImplementedException();
    }
    public override void Freeze()
    {
        throw new System.NotImplementedException();
    }
    public override string GetCompoundBehaviourName()
    {
        throw new System.NotImplementedException();
    }
    public override Condition GetCurrentCondition()
    {
        throw new System.NotImplementedException();
    }
    public override void Log(string body)
    {
        throw new System.NotImplementedException();
    }
    public override void TargetReset()
    {
        throw new System.NotImplementedException();
    }
    public override void UnFreeze()
    {
        throw new System.NotImplementedException();
    }



}

