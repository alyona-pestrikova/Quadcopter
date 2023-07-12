using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSAgent.Agent;
using System;
using System.Threading.Tasks;
using System.Threading;
using static Manager.Wrapper;
using Unity.VisualScripting;

public class Manager : MonoBehaviour
{

    FunctionAgent<DroneAdaptive,
        DroneReaction> _agent;

    public Drone _drone;


    Wrapper _driver;

    public class Wrapper
    {
        public Drone _wrapped_entity;

        private List<double> _value;
        private object _previous_state;
        public object _current_state;

        private Mutex _slider_mutex;
        private Mutex _drop_mutex;
        private Mutex _reset_mutex;
        private Mutex _current_state_mutex;

        public class DroneInformation
        {
            public float _dl_s_s_f;
            public float _dr_s_s_f;
            public float _ul_s_s_f;
            public float _ur_s_s_f;
            public bool IsCollision;
            public bool IsTrigger;
            public Vector3 _local_position;
            public Quaternion _rotation;
            public Vector3 _velocity;
            public Vector3 _angular_velocity;
            public Vector3 _goal_position;
            public Vector3 _up;
            public Vector3 _position;
            public float _angle;
            public float _pr_angle;
            public Vector3 _forward;
            public DroneInformation(float dl_s_s_f, float dr_s_s_f, float ul_s_s_f, float ur_s_s_f, 
                bool IsCollisionVal, bool IsTriggerVal, Vector3 local_position, Quaternion rotation,
                Vector3 velocity, Vector3 angular_velocity, Vector3 goal_position,
                Vector3 up, Vector3 position, Vector3 forward)
            {
                _dl_s_s_f = dl_s_s_f;
                _dr_s_s_f = dr_s_s_f;
                _ul_s_s_f = ul_s_s_f;
                _ur_s_s_f = ur_s_s_f;
                IsCollision = IsCollisionVal;
                IsTrigger = IsTriggerVal;
                _local_position = local_position;
                _rotation = rotation;
                _velocity = velocity;
                _angular_velocity = angular_velocity;
                _goal_position = goal_position;
                _up = up;
                _position = position;
                _forward = forward;
            }
        }
        public bool IsChanged { get; private set; }
        public bool IsDropped { get; private set; }
        public bool IsResetted { get; private set; }
        public bool IsCurrentStateGetted { get; private set; }

        /*
         * 0 - Left Back
         * 1 - Left Front
         * 2 - Right Back
         * 3 - Right Front
         */
        public void CallSliderEvent()
        {
            _slider_mutex.WaitOne();
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
            _slider_mutex.ReleaseMutex();
        }

        public void CallDropEvent()
        {
            _drop_mutex.WaitOne();
            _wrapped_entity._interface.DroneReset();
            IsDropped = false;
            _drop_mutex.ReleaseMutex();
        }

        public void CallResetEvent()
        {
            _reset_mutex.WaitOne();
            _wrapped_entity.DroneUpdate(
                ((DroneInformation)_previous_state)._dl_s_s_f,
                ((DroneInformation)_previous_state)._dr_s_s_f,
                ((DroneInformation)_previous_state)._ul_s_s_f,
                ((DroneInformation)_previous_state)._ur_s_s_f,
                ((DroneInformation)_previous_state).IsCollision,
                ((DroneInformation)_previous_state).IsTrigger,
                ((DroneInformation)_previous_state)._local_position,
                ((DroneInformation)_previous_state)._rotation,
                ((DroneInformation)_previous_state)._velocity,
                ((DroneInformation)_previous_state)._angular_velocity);
            IsResetted = false;
            _reset_mutex.ReleaseMutex();
        }

        public void CallGetCurrentState()
        {
            _current_state_mutex.WaitOne();
            //
            float pr_angle = ((DroneInformation)_current_state)._angle;
            _current_state = new DroneInformation(
                _wrapped_entity._slider_controller._dl_slider.value,
                _wrapped_entity._slider_controller._dr_slider.value,
                _wrapped_entity._slider_controller._ul_slider.value,
                _wrapped_entity._slider_controller._ur_slider.value,
                _wrapped_entity.IsCollision,
                _wrapped_entity.IsTrigger,
                _wrapped_entity.transform.localPosition,
                _wrapped_entity.transform.rotation,
                _wrapped_entity._d_body.velocity,
                _wrapped_entity._d_body.angularVelocity,
                _wrapped_entity._target.transform.position,
                _wrapped_entity.transform.up,
                _wrapped_entity.transform.position,
                _wrapped_entity.transform.forward);
            ((DroneInformation)_current_state)._angle = pr_angle;
            IsCurrentStateGetted = false;
            _current_state_mutex.ReleaseMutex();
        }

        public Wrapper(Drone wrapped_entity)
        {
            _slider_mutex = new();
            _drop_mutex = new();
            _reset_mutex = new();
            _current_state_mutex = new();

            _wrapped_entity = wrapped_entity;

            _previous_state = new object();
            _value = new List<double>();

            IsChanged = false;
            IsDropped = false;
            IsResetted = false;
            IsCurrentStateGetted = false;

            _current_state = new DroneInformation(
                0.3f,
                0.3f,
                0.3f,
                0.3f,
                _wrapped_entity.IsCollision,
                _wrapped_entity.IsTrigger,
                _wrapped_entity.transform.localPosition,
                _wrapped_entity.transform.rotation,
                _wrapped_entity._d_body.velocity,
                _wrapped_entity._d_body.angularVelocity,
                _wrapped_entity._target.transform.position,
                _wrapped_entity.transform.up,
                _wrapped_entity.transform.position,
                _wrapped_entity.transform.forward);


            ((DroneInformation)_current_state)._angle = 181;
            ((DroneInformation)_current_state)._pr_angle = 181;
        }

        public IEnumerable<int> ChangeSliderValue(List<double> value)
        {
            _slider_mutex.WaitOne();
            _value = value;
            IsChanged = true;
            _slider_mutex.ReleaseMutex();
            while (IsChanged)
            {
                yield return 0;
            }
            yield return 0;
        }

        public IEnumerable<int> DroneDrop()
        {
            _drop_mutex.WaitOne();
            IsDropped = true;
            _drop_mutex.ReleaseMutex();
            while (IsDropped)
            {
                yield return 0;
            }
            yield return 0;
        }

        public IEnumerable<int> DroneReset(object previous_state)
        {
            _reset_mutex.WaitOne();
            _previous_state = previous_state;
            IsResetted = true;
            _reset_mutex.ReleaseMutex();
            while (IsResetted)
            {
                yield return 0;
            }
            yield return 0;
        }

        public IEnumerable<int> GetCurrentState()
        {
            _current_state_mutex.WaitOne();
            IsCurrentStateGetted = true;
            _current_state_mutex.ReleaseMutex();
            while (IsCurrentStateGetted)
            {
                yield return 0;
            }
            yield return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _driver = new Wrapper(_drone);
        Task adaptive_agent_task = Task.Factory.StartNew(Agent);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (_driver.IsChanged)
        {
            _driver.CallSliderEvent();
        }
        if (_driver.IsDropped)
        {
            _driver.CallDropEvent();
        }
        if (_driver.IsResetted)
        {
            _driver.CallResetEvent();
        }
        if (_driver.IsCurrentStateGetted)
        {
            _driver.CallGetCurrentState();
        }
    }

    void Update()
    {
        
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
        //_agent.UpdateAdaptive<NullAction>();

        _agent.Import();

        _agent.CreateAdaptiveBehavior();
        //_agent.Save();
        return;
    }
}
