using System;
using FSAgent.Agent.Decorator;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class RightFrontDown : DefaultMove
{
    protected override List<double> GetValue()
    {
        return new List<double> { 0, 0, 0, -0.05 };
    }
}