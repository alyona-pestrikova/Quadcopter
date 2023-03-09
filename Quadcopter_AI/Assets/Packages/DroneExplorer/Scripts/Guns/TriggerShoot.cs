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
    /// I am the gun trigger class. I act as the intermediary between an interface and a gun object.
    /// </summary>
    public class TriggerShoot : MonoBehaviour
    {
        public CmnInterface myInterface;    // interface where the input comes from
        public Gun[] guns;                  // list of guns to shoot
        public int shootButton;             // the index of the button that shoots 
        public bool isFullAuto;             // if the shoot will occure on button down or button hold

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            foreach (Gun gun in guns)
            {
                if (isFullAuto)
                {
                    gun.shoot = myInterface.GetPress(shootButton);
                }
                else
                {
                    gun.shoot = myInterface.GetDown(shootButton);
                }
            }
        }
    }
}