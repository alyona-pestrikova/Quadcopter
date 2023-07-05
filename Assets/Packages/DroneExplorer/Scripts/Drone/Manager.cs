using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSAgent.Agent;
using System;
public class Manager : MonoBehaviour
{

    MoveDrone _driver;
    FunctionAgent<DroneAdaptive,
        DroneReaction> _agent;

    // Start is called before the first frame update
    void Start()
    {
        _agent = new FunctionAgent<DroneAdaptive,
            DroneReaction>(_driver);
        _agent.CreateAdaptiveBehavior();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("kek");
    }
}
