using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MoveDrone : MonoBehaviour
{
    public Rigidbody _d_body; // links to "DroneRigidBody" obj

    public Spinner[] _spinners; // spinner objs

    public Interface _interface; // links drone with input

    public Arrow _arrow; // shows direction of drone movement

    public float _max_spinner_speed;

    // These amendments need to correct drone physics
    private float _force_amendment;

    // Parameters for torque counting
    private float _spinner_radius = 0.03f; 
    private float _air_density = 1.2f;
    private float _torque_const = 0.0025f;   //Constant fron torque formula
    private float _reference_area = 0.007f * 0.055f;
    private float _torque_value;  //Torque from formula without speed

    // Start is called before the first frame update
    void Start()
    {
        this._max_spinner_speed = 5000;
        this._force_amendment = 0.8f;
        _torque_value = 0.5f * _spinner_radius * _air_density * _torque_const * _reference_area;

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

        // Adds force from the side of every spinner
        foreach (var spinner in this._spinners)
        {
            this._d_body.AddForceAtPosition(this._force_amendment * transform.up * spinner._speed * 
            Time.fixedDeltaTime * (float)spinner._traction, spinner.transform.position); // adds force from the side of every spinner
            
            this._d_body.AddTorque(transform.up * _torque_value * spinner._speed * spinner._speed);
        }


    }


    private void OnEnable()
    {
        this._interface._onSpeedChanged += SpinnerSpeedUpdate; //Adds subscribe to change speed event
    }

    
    private void OnDisable()
    {
        this._interface._onSpeedChanged -= SpinnerSpeedUpdate; //Deletes subscribe to change speed event
    }


    //Change spinner speed
    private void SpinnerSpeedUpdate()
    {
        this._spinners[0]._next_speed = this._interface._ul_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[1]._next_speed = this._interface._dr_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[2]._next_speed = -this._interface._dl_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[3]._next_speed = -this._interface._ur_spinner_speed_factor * this._max_spinner_speed;
    }
}
