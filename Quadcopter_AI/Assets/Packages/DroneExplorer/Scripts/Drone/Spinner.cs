using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneExplorer
{
    /// <summary>
    /// Project: 
    /// This project enables the exploration of 3D space by way of a quad-copter drone. The intent 
    /// is to provide a quick drop in package to allow a user to explore a scene in the Unity Play 
    /// engine. The code is written to be modular and can incorporate other gameplay elements with 
    /// the addition of more scripts.
    /// 
    /// Author: Wilson Sauders 
    /// Email: HamsterUnity@gmail.com
    /// Date:   03 / 20 / 2022 
    /// 
    /// File:
    /// I am the Simple spinner class. I rotate the object I am attached to by a public variable on a public axis.
    /// </summary>
    public class Spinner : MonoBehaviour
    {
        public float speed; // rotation speed
        public Vector3 localAxis;   // local axis
                                    // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(localAxis * speed * Time.deltaTime);
        }
    }
}