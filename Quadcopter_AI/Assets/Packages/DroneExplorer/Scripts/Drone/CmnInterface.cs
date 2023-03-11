using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneExplorer
{
    /// File:
    /// I am the common interface class. I am the middle ground between an input 
    /// class like Player Input or an AI controller and physics objects. I contain 
    /// objects for movement and orientation. I also have button objects and can 
    /// detect if a button has been pressed or held down.
    /// 
    /// For Couch Coop games each avatar will have a CmnInterface and the different 
    /// controls will be bound to the player input scripts.
    /// </summary>
    public class CmnInterface : MonoBehaviour
    {
        public Vector3 move;    // change position
        public Vector3 look;    // change orientation
        public float thrust;    // accelerate / decelerate
        public int maxKey;      // maximum buttons that will be proccessed
        public bool[] isPress;  // output flag true when button is initially pressed
        public bool[] isDown;   // output flag true when button is held down

        //public Texture2D[] iconButtons;

        // Start is called before the first frame update
        void Start()
        {
            isPress = new bool[maxKey];
            isDown = new bool[maxKey];
            for (int itor = 0; itor < maxKey; itor++)
            {
                isPress[itor] = isDown[itor] = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Gets value of isPress array with bounds checking 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true when button is initially pressed</returns>
        public bool GetPress(int index)
        {
            if (index < 0 || index >= maxKey) return false;
            return isPress[index];
        }

        /// <summary>
        /// Gets value of isDown array with bounds checking 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true when button is held down</returns>
        public bool GetDown(int index)
        {
            if (index < 0 || index >= maxKey) return false;
            return isDown[index];
        }
    }
}