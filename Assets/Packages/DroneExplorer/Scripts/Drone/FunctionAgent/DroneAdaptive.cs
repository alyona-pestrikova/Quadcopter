using FSAgent.Agent.Component;
using FSAgent.LogicObjects;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class DroneAdaptive : BaseTargetType
{
    public DroneAdaptive() : base()
	{
       
	}
    public override void Start()
    {
        _target_state_stack.Push(((Manager.Wrapper)_driver)._current_state);
        _predicates.Add(new Predicate("ISFirstQuarter", false, 0));
        _predicates.Add(new Predicate("ISSecondQuarter", false, 0));
        _predicates.Add(new Predicate("ISThirdQuarter", false, 0));
        _predicates.Add(new Predicate("ISFourthQuarter", false, 0));
        _predicates.Add(new Predicate("ISTargetDirectionApproaching", false, 1));

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
        return "OrientYourself";
    }

    static bool IsApproximatelyEqual(double value1, double value2, double epsilon)
    {
     
        if (value1.Equals(value2))
            return true;

        if (Double.IsInfinity(value1) | Double.IsNaN(value1))
            return value1.Equals(value2);
        else if (Double.IsInfinity(value2) | Double.IsNaN(value2))
            return value1.Equals(value2);

        double divisor = Math.Max(value1, value2);
        if (divisor.Equals(0))
            divisor = Math.Min(value1, value2);

        return Math.Abs((value1 - value2) / divisor) <= epsilon;
    }

    public override Condition GetCurrentCondition()
    {
        foreach (var waiter in ((Manager.Wrapper)_driver).GetCurrentState()) ;
        ((Manager.Wrapper)_driver)._wrapped_entity._axis_mutex.WaitOne();
        float previous_angle = ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._angle;
        float pr_previous_angle = ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._pr_angle;
        Vector3 real_direction = ((Manager.Wrapper)_driver)._wrapped_entity._arrow._axis;
        Vector3 norm = ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._up;
        Vector3 position = ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._position;
        Vector3 needed_direction = ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._goal_position - 
            position;
        Vector3 forward = ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._forward;
        Vector3 real_direction_projection = Drone.GetProjection(norm, position, real_direction);
        Vector3 needed_direction_projection = Drone.GetProjection(norm, position, needed_direction);
        Vector3 ox = ((Manager.Wrapper)_driver)._wrapped_entity._ox._axis;
        Vector3 oy = ((Manager.Wrapper)_driver)._wrapped_entity._oy._axis;
        int vert_quater = ((Manager.Wrapper)_driver)._wrapped_entity._quarter;
        int hor_quater = Drone.GetQuarter(ox, oy, forward);
        List<Predicate> predicates = new List<Predicate>();
        foreach (var item in _predicates)
        {
            predicates.Add(new Predicate(item._name, false, item._reward));
        }
        switch (hor_quater)
        {
            case 1:
                predicates[FindPredicate("ISFirstQuarter")]._state = true;
                break;
            case 2:
                predicates[FindPredicate("ISSecondQuarter")]._state = true;
                break;
            case 3:
                predicates[FindPredicate("ISThirdQuarter")]._state = true;
                break;
            case 4:
                predicates[FindPredicate("ISFourthQuarter")]._state = true;
                break;
        }

        float cur_angle = Vector3.Angle(forward, oy);

        if (cur_angle < 3)
        {
            predicates[FindPredicate("ISFINISH")]._state = true;
        }

        
        ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._angle = cur_angle;
        ((Manager.Wrapper.DroneInformation)((Manager.Wrapper)_driver)._current_state)._pr_angle = previous_angle;
        ((Manager.Wrapper)_driver)._wrapped_entity._axis_mutex.ReleaseMutex();
        if (previous_angle == 0)
        {
            return new Condition(predicates);
        }
        if (cur_angle < previous_angle)
        {
            predicates[FindPredicate("ISTargetDirectionApproaching")]._state = true;
        }
        if (cur_angle > previous_angle && (cur_angle - previous_angle) >= (previous_angle - pr_previous_angle))
        {
            predicates[FindPredicate("ISFAIL")]._state = true;
        }
        return new Condition(predicates);
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

