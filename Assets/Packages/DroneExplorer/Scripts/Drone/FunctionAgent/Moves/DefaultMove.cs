using System;
using FSAgent.Agent.Decorator;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEditor.Compilation;

public class DefaultMove : AgentDecorator<DroneAdaptive>
{
    protected virtual List<double> GetValue()
    {
        return new List<double> { 0 };
    }
    protected override IEnumerable<int> Action()
    {
        foreach (var waiter in ((Manager.Wrapper)_target._driver).
            ChangeSliderValue(GetValue()))
        {
            yield return 0;
        }
        yield return 0;
    }
}