using UnityEngine;
using System;
using System.Collections;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    public float _speed; // spinner speed
    public float _acceleration; // spinner acceleration
    public float _next_speed; // this speed is changed by input

    public enum Traction
    {
        Direct = 1,
        Inverse = -1
    };

    public Traction _traction; // spinner traction direction

    private Vector3 _local_axis; // makes rotate axis
    public Vector3 _start_position;

    // Default values
    void Start()
    {
        this._acceleration = 500;
        this._local_axis = new Vector3(0, 0, 1);
        this._start_position = transform.position;
    }

    // Update is called once per frame
    void Update() => transform.Rotate(this._local_axis * this._speed * Time.deltaTime); // rotate spinner


    void FixedUpdate()
    {
        // _speed changes until it becomes equal to this
        if(Math.Abs(this._next_speed - this._speed) >= this._acceleration)
        {
            if(this._next_speed < this._speed)
            {
                this._speed -= _acceleration * Time.fixedDeltaTime;
            }
            else
            {
                this._speed += _acceleration * Time.fixedDeltaTime;
            }
            
        } else if(this._speed != this._next_speed)
        {
            this._speed = this._next_speed;
        }
    }

}

