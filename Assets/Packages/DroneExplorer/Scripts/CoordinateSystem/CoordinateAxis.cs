using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateAxis : MonoBehaviour
{
    public Vector3 _axis; // shows direction of drone movement

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, _axis);
    }
}
