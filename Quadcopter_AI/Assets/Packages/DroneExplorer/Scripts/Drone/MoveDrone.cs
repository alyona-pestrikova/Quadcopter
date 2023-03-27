using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;

public class MoveDrone : MonoBehaviour
{
    public Rigidbody _d_body; // links to "DroneRigidBody" obj

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

    // Start is called before the first frame update
    void Start()
    {
        this._max_spinner_speed = 5000;
        this._full_force_amendment = 0.3f;
        this._full_air_drag = 0.5f;

        this._spinner_radius = 0.03f; 
        this._air_density = 1.2f;
        this._torque_const = 0.0025f;
        this._reference_area = 0.007f * 0.055f;
        this._torque_amendment = 0.5f * this._spinner_radius * this._air_density * this._torque_const * this._reference_area;
        

        // Set spinner traction direction
        this._spinners[0]._traction = Spinner.Traction.Direct;
        this._spinners[1]._traction = Spinner.Traction.Direct;
        this._spinners[2]._traction = Spinner.Traction.Inverse;
        this._spinners[3]._traction = Spinner.Traction.Inverse;
    }




    // Update is called once per sometime
    void FixedUpdate()
    {
        this._arrow._axis = this._d_body.velocity; // updates drone arrow direction

        // Calculates hight amendment
        float hight_amendment = (float)transform.position.y / 10; // height amendment correction
        this._force_amendment = this._full_force_amendment / (hight_amendment < 1? 1: hight_amendment); // force correction about height
        this._d_body.drag= this._full_air_drag / (hight_amendment < 1? 1: hight_amendment); // air drag correction about height

        // Adds force
        foreach (var spinner in this._spinners)
        {
            this._d_body.AddForceAtPosition(this._force_amendment * transform.up * Math.Abs(spinner._speed) * 
            Time.fixedDeltaTime, spinner.transform.position); // adds force from the side of every spinner
            
            this._d_body.AddTorque(transform.up * this._torque_amendment * spinner._speed * spinner._speed * Math.Sign(spinner._speed)); // adds torque from every spinner
        }

    }


    private void OnEnable()
    {
        this._interface._onSpeedChanged += SpinnerSpeedUpdate; // adds subscribe to change speed event
    }

    
    private void OnDisable()
    {
        this._interface._onSpeedChanged -= SpinnerSpeedUpdate; // deletes subscribe to change speed event
    }


    // Change spinner speed
    private void SpinnerSpeedUpdate()
    {
        this._spinners[0]._next_speed = this._interface._ul_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[1]._next_speed = this._interface._dr_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[2]._next_speed = -this._interface._dl_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[3]._next_speed = -this._interface._ur_spinner_speed_factor * this._max_spinner_speed;
    }
}
