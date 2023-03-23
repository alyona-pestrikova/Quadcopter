using UnityEngine;
using System;
using System.Collections;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    public float _speed; // Spinner speed
    public float _acceleration; // Spinner acceleration
    public float _next_speed; // This speed is changed by input

    private Vector3 _local_axis; // Make rotate axis
    
    // Default values
    void Start()
    {
        this._acceleration = 500;
        this._speed = 0;
        this._next_speed = 0;
        this._local_axis = new Vector3(0, 0, 1);
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

