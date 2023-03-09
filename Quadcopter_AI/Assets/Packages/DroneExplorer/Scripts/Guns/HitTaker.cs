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
    /// I am a container class. Bullets will look for me attached to a collider so add damage and verify
    /// team for friendly fire mechanics.
    /// </summary>
    public class HitTaker : MonoBehaviour
    {
        public int team;
        public float dammage;

    }
}
