using Unity.MLAgents;
using System.Collections;
using UnityEngine;
using System;
using TMPro;
using FSAgent.Agent;
using FSAgent.Agent.Component;

public class Drone : Agent
{
    public Rigidbody _d_body; // links to "DroneRigidBody" obj

    public SliderController _slider_controller;
    public GameObject _target;

    // Telemetry from drone
    public TMP_Text _immutable_telemetry;
    public TMP_Text _changeable_telemetry;

    public Spinner[] _spinners; // spinner objs

    public Interface _interface; // links drone with input

    public Arrow _arrow; // shows direction of drone movement

    public float _max_spinner_speed; // max spinner speed

    // These amendments need to correct drone physics
    private float _full_air_drag; // full air drag without height amendment
    private float _full_force_amendment; // full force without height amendment
    private float _force_amendment; // force with height amendment

    // Parameters for torque counting
    private float _spinner_radius;
    private float _air_density;
    private float _torque_const; // constant fron torque formula
    private float _reference_area;
    private float _torque_amendment; // torque from formula without speed

    public float _start_spinner_factor; 

    // Collision and trigger events
    public bool IsCollision { get; set; }
    public bool IsTrigger { get; set; }

    public Vector3 _pr_position;
    // Drone position 


    // Start is called before the first frame update
    void Start()
    {
        this._max_spinner_speed = 5000;
        this._full_force_amendment = 0.2f;
        this._full_air_drag = 0.5f;

        this._spinner_radius = 0.03f;
        this._air_density = 1.2f;
        this._torque_const = 0.0025f;
        this._reference_area = 0.007f * 0.055f;
        this._torque_amendment = 0.5f * this._spinner_radius * this._air_density * this._torque_const * this._reference_area;

        this._start_spinner_factor = 0.3f;

        // Set spinner traction direction
        this._spinners[0]._traction = Spinner.Traction.Direct;
        this._spinners[1]._traction = Spinner.Traction.Inverse;
        this._spinners[2]._traction = Spinner.Traction.Direct;
        this._spinners[3]._traction = Spinner.Traction.Inverse;


        this._interface.DroneReset();

        //StartAgent();
    }


    // Update is called once per frame
    void Update()
    {
        this.PrintTelemetry();
    }

    // Update is called once per sometime
    void FixedUpdate()
    {
        this._arrow._axis = this._d_body.velocity; // updates drone arrow direction

        // Calculates hight amendment
        float hight_amendment = (float)transform.position.y / 10; // height amendment correction
        this._force_amendment = this._full_force_amendment / (hight_amendment < 1 ? 1 : hight_amendment); // force correction about height
        this._d_body.drag = this._full_air_drag / (hight_amendment < 1 ? 1 : hight_amendment); // air drag correction about height

        // Adds force
        foreach (var spinner in this._spinners)
        {
            this._d_body.AddForceAtPosition(this._force_amendment * transform.up * Math.Abs(spinner._speed) *
            Time.fixedDeltaTime, spinner.transform.position); // adds force from the side of every spinner

            this._d_body.AddTorque(transform.up * this._torque_amendment * spinner._speed * spinner._speed * Math.Sign(spinner._speed)); // adds torque from every spinner
        }
    }

    private void PrintTelemetry()
    {
        // Immutable telemetry
        this._immutable_telemetry.text = "Force amendment: " + this._force_amendment.ToString() + "\n";
        this._immutable_telemetry.text += "Air drag: " + this._d_body.drag.ToString() + "\n";
        this._immutable_telemetry.text += "X: " + this._d_body.transform.position.x.ToString() + "\n";
        this._immutable_telemetry.text += "Y: " + this._d_body.transform.position.y.ToString() + "\n";
        this._immutable_telemetry.text += "Z: " + this._d_body.transform.position.z.ToString() + "\n";
        this._immutable_telemetry.text += "Velocity: " + this._d_body.velocity.magnitude.ToString() + " m/s" + "\n";

        // Changeable telemetry
        this._changeable_telemetry.text = "Left Up Spinner speed: " + (this._spinners[0]._speed * (float)this._spinners[0]._traction).ToString() + "\n";
        this._changeable_telemetry.text += "Left Down Spinner speed: " + (this._spinners[1]._speed * (float)this._spinners[1]._traction).ToString() + "\n";
        this._changeable_telemetry.text += "Right Down Spinner speed: " + (this._spinners[2]._speed * (float)this._spinners[2]._traction).ToString() + "\n";
        this._changeable_telemetry.text += "Right Up Spinner speed: " + (this._spinners[3]._speed * (float)this._spinners[3]._traction).ToString() + "\n";
    }


    public void DroneUpdate(float dl_s_s_f, float dr_s_s_f, float ul_s_s_f, float ur_s_s_f, 
        bool IsCollision, bool IsTrigger, Vector3 local_position, Quaternion rotation,
        Vector3 velocity, Vector3 angular_velocity)
    {
        transform.localPosition = local_position;
        transform.rotation = rotation;
        this._d_body.velocity = velocity;
        this._d_body.angularVelocity = angular_velocity;
        //StartCoroutine(ResetRigidbody(local_position, rotation, velocity, angular_velocity));
        this._interface._dl_spinner_speed_factor = dl_s_s_f;
        this._interface._dr_spinner_speed_factor = dr_s_s_f;
        this._interface._ul_spinner_speed_factor = ul_s_s_f;
        this._interface._ur_spinner_speed_factor = ur_s_s_f;
        this._interface.SpeedUpdate();

        this._spinners[0]._speed = this._interface._ul_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[0]._traction;
        this._spinners[1]._speed = this._interface._dl_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[1]._traction;
        this._spinners[2]._speed = this._interface._dr_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[2]._traction;
        this._spinners[3]._speed = this._interface._ur_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[3]._traction;

        this._pr_position = transform.position;

        this.IsCollision = IsCollision;
        this.IsTrigger = IsTrigger;


    }

    // Reset Drone
    public void DroneReset()
    {
        DroneUpdate(this._start_spinner_factor,
            this._start_spinner_factor,
            this._start_spinner_factor,
            this._start_spinner_factor,
            false, false,
            Vector3.zero,
            Quaternion.Euler(Vector3.zero),
            Vector3.zero,
            Vector3.zero);
    }
    IEnumerator ResetRigidbody(Vector3 local_position, Quaternion rotation,
        Vector3 velocity, Vector3 angular_velocity)
    {
        this._interface._block_input = true;
        while (this._d_body.angularVelocity != angular_velocity)
        {
            transform.localPosition = local_position;
            transform.rotation = rotation;
            this._d_body.velocity = velocity;
            this._d_body.angularVelocity = angular_velocity;
            yield return new WaitForFixedUpdate();
        }
        this._interface._block_input = false;
    }


    // Trigget event
    private void OnTriggerEnter(Collider other)
    {
        this.IsTrigger = true;
    }

    // Collision event
    private void OnCollisionEnter(Collision collision)
    {
        this.IsCollision = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this._interface._onSpeedChanged += SpinnerSpeedUpdate; // adds subscribe to change speed event
        this._interface._onResetCall += DroneReset; // adds subscribe to reset call event
    }



    protected override void OnDisable()
    {
        base.OnDisable();
        this._interface._onSpeedChanged -= SpinnerSpeedUpdate; // deletes subscribe to change speed event
        this._interface._onResetCall -= DroneReset; // deletes subscribe to reset call event
    }


    // Change spinner speed
    private void SpinnerSpeedUpdate()
    {
        this._spinners[0]._next_speed = this._interface._ul_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[0]._traction;
        this._spinners[1]._next_speed = this._interface._dl_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[1]._traction;
        this._spinners[2]._next_speed = this._interface._dr_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[2]._traction;
        this._spinners[3]._next_speed = this._interface._ur_spinner_speed_factor * this._max_spinner_speed * (float)this._spinners[3]._traction;
    }
}
