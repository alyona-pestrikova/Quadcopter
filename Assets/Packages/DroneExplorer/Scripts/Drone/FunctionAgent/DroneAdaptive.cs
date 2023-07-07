using FSAgent.Agent.Component;
using FSAgent.LogicObjects;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DroneAdaptive : BaseTargetType
{
    Condition _current_condition;

    public DroneAdaptive() : base()
	{
        _current_condition = new Condition(_predicates);
	}
    public override void Start()
    {
        
    }
    public override void Alarm()
    {
        Debug.Log("Alarm!");
    }
    public override void Drop()
    {
        foreach (var waiter in ((Manager.Wrapper)_driver).DroneDrop());
    } 
    public override void Freeze()
    {
        return;
    }
    public override void UnFreeze()
    {
        return;
    }
    public override string GetCompoundBehaviourName()
    {
        return "TurnLeft";
    }
    public override Condition GetCurrentCondition()
    {
        foreach (var waiter in ((Manager.Wrapper)_driver).GetCurrentState()) ;
        if (((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._ur_s_s_f != 0.3f ||
            ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._dr_s_s_f != 0.3f ||
            ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._ul_s_s_f != 0.3f)
        {
            return 2;
        }
        return 0;
    }
    public override void Log(string body)
    {
        Debug.Log(body);    
        return;
    }

    public override object GetPreviousTargetState()
    {
        foreach (var waiter in ((Manager.Wrapper)_driver).GetCurrentState());
        return ((Manager.Wrapper)_driver)._current_state;
    }

    public override void SetPreviousTargetState(object previous_state)
    {
        foreach (var waiter in ((Manager.Wrapper)_driver).DroneReset(previous_state));
    }

}

