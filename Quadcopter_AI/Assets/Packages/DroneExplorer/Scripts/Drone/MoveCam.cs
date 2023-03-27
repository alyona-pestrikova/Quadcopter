using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Transform _target; // drone transform field

    public Vector3 _dist; // dist beetwen drone and camera

    // Start is called before the first frame update
    void Start()
    {
        this._dist = transform.position - this._target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = this._target.position + this._dist;
        transform.rotation = this._target.rotation;
        transform.LookAt(this._target.position);
    }
}
