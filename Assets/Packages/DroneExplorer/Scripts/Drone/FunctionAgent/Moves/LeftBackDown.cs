using System;
using FSAgent.Agent.Decorator;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class LeftBackDown : AgentDecorator<DroneAdaptive>
{
    protected override IEnumerable<int> Action()
    {

        ((Manager.Wrapper)_target._driver).
            ChangeSliderValue(new List<double> { -0.05, 0, 0, 0 });
        yield return 0;
    }
}