using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneExplorer
{
    
    /// File:
    /// I am the Player XBox input class. I take in strings that define joystick axes and buttons. 
    /// Every Update I query these strings and update the myCmnInterface. To get maximum utility of 
    /// this class the user must set the appropriate axes in the Project Setting -> Input Manager 
    /// Axes. Please note that Couch coop can be accomplished by setting the specific axis and button 
    /// labels in the Input Manager, then binding the appropriate strings to the local avatars.

    public class PlayerXbox : MonoBehaviour
    {
        public CmnInterface myCmnInterface; // Common interface to write to

        // Axis references
        public string moveX;
        public string moveY;
        public string lookX;
        public string lookY;
        // button referenes
        public string[] button;
        // Start is called before the first frame update
        

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
                myCmnInterface.look = Vector3.right * Input.GetAxis(lookX) + Vector3.up * Input.GetAxis(lookY);
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