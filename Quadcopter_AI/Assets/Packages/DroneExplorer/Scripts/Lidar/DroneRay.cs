using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRay : MonoBehaviour
{
    public Rigidbody _drone_body; // links to "DroneRigidBody" obj
    private Ray ray; //Ray object
    private float _offset = 0.1f; // Ray offset relative to the drone center

    //Possible ray directions relative to the drone
    public enum Directions
    {
        Left, Right, Up, Down, Forward, Back
    }


    public Directions _direction; // The direction of this ray


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
            Debug.Log("Hit " + hit.collider.gameObject.name + " " + _direction.ToString().ToLower());
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
