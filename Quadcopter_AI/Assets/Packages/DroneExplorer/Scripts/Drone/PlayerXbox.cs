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
    /// Date:   03 / 14 / 2022 (Happy Pie day)
    /// 
    /// File:
    /// I am the Player XBox input class. I take in strings that define joystick axes and buttons. 
    /// Every Update I query these strings and update the myCmnInterface. To get maximum utility of 
    /// this class the user must set the appropriate axes in the Project Setting -> Input Manager 
    /// Axes. Please note that Couch coop can be accomplished by setting the specific axis and button 
    /// labels in the Input Manager, then binding the appropriate strings to the local avatars.
    ///
    /// </summary>
    public class PlayerXbox : MonoBehaviour
    {
        public CmnInterface myCmnInterface; // Common interface to write to

        // Axis references
        public string moveX;
        public string moveY;
        public string lookX;
        public string lookY;
        public string thrust;
        // button referenes
        public string[] button;
        // Start is called before the first frame update


        public bool rampLook;   // for thumbstick control use rampLook
        public float rampMul;   // speed at which the looking is ramped

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Set move if the axis strings are not empty
            if (moveX != "" && moveY != "")
            {
                myCmnInterface.move = Vector3.right * Input.GetAxis(moveX) + Vector3.up * Input.GetAxis(moveY);
            }
            // Set look if the axis strings are not empty
            if (lookX != "" && lookY != "")
            {
                if (rampLook)
                {
                    Vector3 next = Vector3.right * Input.GetAxis(lookX) + Vector3.up * Input.GetAxis(lookY);

                    myCmnInterface.look = Vector3.Lerp(myCmnInterface.look, next, Time.deltaTime * rampMul);
                }
                else
                {
                    myCmnInterface.look = Vector3.right * Input.GetAxis(lookX) + Vector3.up * Input.GetAxis(lookY);
                }
            }
            // Set thrust if the axis string is not empty
            if (thrust != "")
            {
                myCmnInterface.thrust = Input.GetAxis(thrust);
            }

            // set internal button states
            for (int itor = 0; itor < button.Length && itor < myCmnInterface.maxKey; itor++)
            {
                myCmnInterface.isPress[itor] = Input.GetButton(button[itor]);
                myCmnInterface.isDown[itor] = Input.GetButtonDown(button[itor]);
            }
        }
    }
}