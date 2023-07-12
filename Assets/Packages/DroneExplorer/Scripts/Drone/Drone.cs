using Unity.MLAgents;
using UnityEngine;
using System;
using TMPro;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine.UIElements;

public class Drone : Agent
{
    public Rigidbody _d_body; // links to "DroneRigidBody" obj


    public SliderController _slider_controller;


    public Mutex _axis_mutex;
    
    public GameObject _target;


    // Telemetry from drone
    public TMP_Text _immutable_telemetry;
    public TMP_Text _changeable_telemetry;

    public Spinner[] _spinners; // spinner objs

    public Interface _interface; // links drone with input

    public Arrow _arrow; // shows direction of drone movement

    public CoordinateAxis _ox;
    public CoordinateAxis _oy;

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
    public bool IsCollision;
    public bool IsTrigger;

    public Vector3 _pr_position;

    public int _quarter;

    // Start is called before the first frame update
    void Start()
    {
        _axis_mutex = new Mutex();
        _max_spinner_speed = 5000;
        _full_force_amendment = 0.2f;
        _full_air_drag = 0.5f;

        _spinner_radius = 0.03f;
        _air_density = 1.2f;
        _torque_const = 0.0025f;
        _reference_area = 0.007f * 0.055f;
        _torque_amendment = 0.5f * _spinner_radius * _air_density * _torque_const * _reference_area;

        _start_spinner_factor = 0.3f;

        // Set spinner traction direction
        _spinners[0]._traction = Spinner.Traction.Direct;
        _spinners[1]._traction = Spinner.Traction.Inverse;
        _spinners[2]._traction = Spinner.Traction.Direct;
        _spinners[3]._traction = Spinner.Traction.Inverse;


        _interface.DroneReset();

        //StartAgent();
    }


    // Update is called once per frame
    void Update()
    {
        PrintTelemetry();
    }

    // With plane norm and point returns the projection of the desired vector onto that plane
    public static Vector3 GetProjection(Vector3 norm, Vector3 point, Vector3 source)
    {
        Vector3 point_3d = point + source;

        float A = norm.x;
        float B = norm.y;
        float C = norm.z;
        float D = -(A * point.x + B * point.y + C * point.z);

        float x = (B * B * point_3d.x) / A + (C * C * point_3d.x) / A - B * point_3d.y - C * point_3d.z - D;
        x = x / (A + (B * B / A) + (C * C / A));

        float y = ((B * (x - point_3d.x)) / A) + point_3d.y;
        float z = ((C * (x - point_3d.x)) / A) + point_3d.z;

        Vector3 oy = new Vector3(x, y, z) - point;

        return oy.normalized * 10;
    }

    public static int GetQuarter(Vector3 ox, Vector3 oy, Vector3 source)
    {
        float source_ox = Vector3.Angle(ox, source);  
        float source_oy = Vector3.Angle(oy, source);
        if (source_ox < 90) // 1 or 4
        {
            if (source_oy < 90) // 1 or 2
            {
                return 1;
            }
            if (source_oy > 90) // 3 or 4
            {
                return 4;
            }    
        }
        if (source_ox > 90) // 2 or 3
        {
            if (source_oy < 90) // 1 or 2
            {
                return 2;
            }
            if (source_oy > 90) // 3 or 4
            {
                return 3;
            }
        }


        if (source_oy == 0) // -1
        {
            return 1;
        }
        if (source_ox == 0) // -4
        {
            return 4;
        }
        if (source_oy == 90) // -2 or -4
        {
            return 2;
        }
        return 3; // -1 or -3
    }

    // Update is called once per sometime
    void FixedUpdate()
    {
        _axis_mutex.WaitOne();
        _arrow._axis = _d_body.velocity; // updates drone arrow direction

        Vector3 norm = transform.up;

        Vector3 oy = GetProjection(norm, transform.position, _target.transform.position - transform.position);

        _oy._axis = oy;

        Vector3 ox = Vector3.Cross(norm, oy);

        if (Vector3.Dot(ox, Vector3.Cross(norm, oy)) < 0 )
        {
            ox = -ox;
        }
        ox = ox.normalized * 10;

        _ox._axis = ox;
        _quarter = GetQuarter(ox, oy, GetProjection(norm, transform.position, _d_body.velocity));

        _axis_mutex.ReleaseMutex();



        // Calculates hight amendment
        float hight_amendment = (float)transform.position.y / 10; // height amendment correction
        _force_amendment = _full_force_amendment / (hight_amendment < 1 ? 1 : hight_amendment); // force correction about height
        _d_body.drag = _full_air_drag / (hight_amendment < 1 ? 1 : hight_amendment); // air drag correction about height

        // Adds force
        foreach (var spinner in _spinners)
        {
            _d_body.AddForceAtPosition(_force_amendment * transform.up * Math.Abs(spinner._speed) * 
            Time.fixedDeltaTime, spinner.transform.position); // adds force from the side of every spinner

            _d_body.AddTorque(transform.up * _torque_amendment * spinner._speed * spinner._speed * Math.Sign(spinner._speed)); // adds torque from every spinner
        }
    }

    private void PrintTelemetry()
    {
        // Immutable telemetry
        _immutable_telemetry.text = "Force amendment: " + _force_amendment.ToString() + "\n";
        _immutable_telemetry.text += "Air drag: " + _d_body.drag.ToString() + "\n";
        _immutable_telemetry.text += "X: " + _d_body.transform.position.x.ToString() + "\n";
        _immutable_telemetry.text += "Y: " + _d_body.transform.position.y.ToString() + "\n";
        _immutable_telemetry.text += "Z: " + _d_body.transform.position.z.ToString() + "\n";
        _immutable_telemetry.text += "Velocity: " + _d_body.velocity.magnitude.ToString() + " m/s" + "\n";

        // Changeable telemetry
        _changeable_telemetry.text = "Left Up Spinner speed: " + (_spinners[0]._speed * (float)_spinners[0]._traction).ToString() + "\n";
        _changeable_telemetry.text += "Left Down Spinner speed: " + (_spinners[1]._speed * (float)_spinners[1]._traction).ToString() + "\n";
        _changeable_telemetry.text += "Right Down Spinner speed: " + (_spinners[2]._speed * (float)_spinners[2]._traction).ToString() + "\n";
        _changeable_telemetry.text += "Right Up Spinner speed: " + (_spinners[3]._speed * (float)_spinners[3]._traction).ToString() + "\n";
    }


    public void DroneUpdate(float dl_s_s_f, float dr_s_s_f, float ul_s_s_f, float ur_s_s_f, 
        bool IsCollision, bool IsTrigger, Vector3 local_position, Quaternion rotation,
        Vector3 velocity, Vector3 angular_velocity)
    {
        transform.localPosition = local_position;
        transform.rotation = rotation;
        _d_body.velocity = velocity;
        _d_body.angularVelocity = angular_velocity;
        _d_body.Sleep();
        _interface._dl_spinner_speed_factor = dl_s_s_f;
        _interface._dr_spinner_speed_factor = dr_s_s_f;
        _interface._ul_spinner_speed_factor = ul_s_s_f;
        _interface._ur_spinner_speed_factor = ur_s_s_f;

        _slider_controller._dl_slider.value = dl_s_s_f;
        _slider_controller._dr_slider.value = dr_s_s_f;
        _slider_controller._ul_slider.value = ul_s_s_f;
        _slider_controller._ur_slider.value = ur_s_s_f;

        _interface.SpeedUpdate();

        _spinners[0]._speed = _interface._ul_spinner_speed_factor * _max_spinner_speed * (float)_spinners[0]._traction;
        _spinners[1]._speed = _interface._dl_spinner_speed_factor * _max_spinner_speed * (float)_spinners[1]._traction;
        _spinners[2]._speed = _interface._dr_spinner_speed_factor * _max_spinner_speed * (float)_spinners[2]._traction;
        _spinners[3]._speed = _interface._ur_spinner_speed_factor * _max_spinner_speed * (float)_spinners[3]._traction;

        _pr_position = transform.position;

        this.IsCollision = IsCollision;
        this.IsTrigger = IsTrigger;

    }



    // Reset Drone
    public void DroneReset()
    {
        DroneUpdate(_start_spinner_factor,
            _start_spinner_factor,
            _start_spinner_factor,
            _start_spinner_factor,
            false, false,
            Vector3.zero,
            Quaternion.Euler(Vector3.zero),
            Vector3.zero,
            Vector3.zero);
    }

    // Trigget event
    private void OnTriggerEnter(Collider other) 
    {
        IsTrigger = true;
    }

    // Collision event
    private void OnCollisionEnter(Collision collision)
    {
        IsCollision = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _interface._onSpeedChanged += SpinnerSpeedUpdate; // adds subscribe to change speed event
        _interface._onResetCall += DroneReset; // adds subscribe to reset call event
    }



    protected override void OnDisable()
    {
        base.OnDisable();
        _interface._onSpeedChanged -= SpinnerSpeedUpdate; // deletes subscribe to change speed event
        _interface._onResetCall -= DroneReset; // deletes subscribe to reset call event
    }


    // Change spinner speed
    private void SpinnerSpeedUpdate()
    {
        _spinners[0]._next_speed = _interface._ul_spinner_speed_factor * _max_spinner_speed * (float)_spinners[0]._traction;
        _spinners[1]._next_speed = _interface._dl_spinner_speed_factor * _max_spinner_speed * (float)_spinners[1]._traction;
        _spinners[2]._next_speed = _interface._dr_spinner_speed_factor * _max_spinner_speed * (float)_spinners[2]._traction;
        _spinners[3]._next_speed = _interface._ur_spinner_speed_factor * _max_spinner_speed * (float)_spinners[3]._traction;
    }
}
