using System;
using FSAgent.Agent.Decorator;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class RightFrontDown : AgentDecorator<DroneAdaptive>
{
    protected override IEnumerable<int> Action()
    {
        ((Manager.Wrapper)_target._driver).
            ChangeSliderValue(new List<double> { 0, 0, 0, -0.05 });
        yield return 0;
    }
}