using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSAgent.Agent;
using System;
using System.Threading.Tasks;
using System.Threading;
public class Manager : MonoBehaviour
{

    FunctionAgent<DroneAdaptive,
        DroneReaction> _agent;

    public Drone _drone;

    private bool IsSliderFactorChanged;

    Wrapper _driver;

    public class Wrapper
    {
        private Drone _wrapped_entity;
        private List<double> _value;
        public bool IsChanged { get; private set; }

        public void CallEvent()
        {
            //mutex lock
            _wrapped_entity._slider_controller
                ._dl_slider.value += (float)_value[0];
            _wrapped_entity._slider_controller
                ._ul_slider.value += (float)_value[1];
            _wrapped_entity._slider_controller
                ._dr_slider.value += (float)_value[2];
            _wrapped_entity._slider_controller
                ._ur_slider.value += (float)_value[3];
            _wrapped_entity._interface.SpeedUpdate();
            IsChanged = false;
            //mutex unlock
        }

        public Wrapper(Drone wrapped_entity)
        {
            _wrapped_entity = wrapped_entity;
            _value = new List<double>();
        }


        /*
         * 0 - Left Back
         * 1 - Left Front
         * 2 - Right Back
         * 3 - Right Front
         */
        public void ChangeSliderValue(List<double> value)
        {
            //mutex lock
            _value = value;
            IsChanged = true;
            //mutex unlock
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        _driver = new Wrapper(_drone);
        Task adaptive_agent_task = Task.Factory.StartNew(Agent);
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (_driver.IsChanged)
        {
            _driver.CallEvent();
        }
    }


    void Agent()
    {
        _agent = new FunctionAgent<DroneAdaptive,
           DroneReaction>(_driver);
        _agent.UpdateAdaptive<LeftBackDown>();
        _agent.UpdateAdaptive<LeftFrontDown>();
        _agent.UpdateAdaptive<RightFrontDown>();
        _agent.UpdateAdaptive<RightBackDown>();

        _agent.UpdateAdaptive<LeftBackUp>();
        _agent.UpdateAdaptive<LeftFrontUp>();
        _agent.UpdateAdaptive<RightFrontUp>();
        _agent.UpdateAdaptive<RightBackUp>();
        _agent.CreateAdaptiveBehavior();
    }
}
