using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDrone : MonoBehaviour
{
    private Rigidbody _d_body; // link to "DroneRigidBody" obj

    public Spinner[] _spinners; // spinner objs

    //public Interface _interface; // link drone with input

    // Default values
    public MoveDrone()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    Vector3 CaclResultDirection()
    {
        return new Vector3(0,0,0);
    }
}
