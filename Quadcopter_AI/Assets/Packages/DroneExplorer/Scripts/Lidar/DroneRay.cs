using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRay : MonoBehaviour
{
    public Rigidbody _drone_body; // links to "DroneRigidBody" obj
    private Ray ray; //Ray object
    private float _offset = 0.05f; // Ray offset relative to the drone center

    //Possible ray directions relative to the drone
    public enum Directions
    {
        Left, Right, Up, Down, Forward, Back
    }


    public List<string> Exception; // Objects that must be ignored by lidar
    public Directions _direction; // The direction of this ray


    // Some info about the nearest object
    public string _nearest_obj_name; 
    public float _nearest_distance;


    private void Start()
    {
        Exception.Add("WindArea");
        _nearest_obj_name = "";
        _nearest_distance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = _drone_body.transform.position + _offset * GetDirection(_direction);
        ray.direction = GetDirection(_direction);

        // Drawing of the ray on the Scene, not in the Game mode!
        Debug.DrawRay(ray.origin, ray.direction.normalized * 3, Color.magenta);

        RaycastHit hit; // Contains info about the object that the ray collided with

        // Search for ray collisions with colliders
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1))
        {
            bool is_exc = false;
            for (int i = 0; i < Exception.Count; i++)
            {
                is_exc = (Exception[i] == hit.collider.gameObject.name);
            }
            if (!is_exc)
            {
                _nearest_obj_name = hit.collider.gameObject.name;
                _nearest_distance = hit.distance;
            } else
            {
                _nearest_distance = 0;
                _nearest_obj_name = "";
            }
        } else
        {
            _nearest_distance = 0;
            _nearest_obj_name = "";
        }
    }


    //GetDirection determines the current direction of the ray by the current position of the drone
    Vector3 GetDirection(Directions dir)
    {
        if (dir == Directions.Left)
        {
            return _drone_body.transform.right * -1;
        } 
        else if (dir == Directions.Right)
        {
            return _drone_body.transform.right;
        } 
        else if (dir == Directions.Up)
        {
            return _drone_body.transform.up;
        } 
        else if (dir == Directions.Down)
        {
            return _drone_body.transform.up * -1;
        } 
        else if (dir == Directions.Forward)
        {
            return _drone_body.transform.forward;
        }
        else
        {
            return _drone_body.transform.forward * -1;
        }
    }
}
