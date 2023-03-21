using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDrone : MonoBehaviour
{
    private Rigidbody _d_body; // link to "DroneRigidBody" obj

    public Spinner[] _spinners; // spinner objs

    public Interface _interface; // link drone with input

    public Arrow _arrow; // show direction of drone movement

    public float _max_spinner_speed; // max spinner speed

    // Start is called before the first frame update
    void Start()
    {
        this._max_spinner_speed = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
        this.SpinnerSpeedUpdate();

    }

    //Checks speed change
    void SpinnerSpeedUpdate()
    {
        this._spinners[0]._speed = this._interface._ul_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[1]._speed = this._interface._dr_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[2]._speed = this._interface._dl_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[3]._speed = this._interface._ur_spinner_speed_factor * this._max_spinner_speed;
    }

    //Calc result power direction
    Vector3 CaclResultDirection()
    {
        return new Vector3(0,0,0);
    }


}
