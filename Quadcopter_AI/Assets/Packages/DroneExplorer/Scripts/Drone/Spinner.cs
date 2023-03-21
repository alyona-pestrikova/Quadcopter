using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    public float _speed; // Spinner speed

    private Vector3 _local_axis; // Make rotate axis
    
    // Default values
    public void Start()
    {
        this._speed = 0;
        this._local_axis = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(this._local_axis * this._speed * Time.deltaTime);
    }
    
    public void SetSpeed(float speed) => this._speed = speed;

    public float GetSpeed() => this._speed;
}

