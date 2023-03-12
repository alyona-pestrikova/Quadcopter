using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneExplorer {
    public class PlayerTouchScreen : MonoBehaviour
    {
        public CmnInterface myCmnInterface; // Common interface to write to
        public DynamicJoystick joystick_left;
        public DynamicJoystick joystick_right;

        // Axis references
        //public string moveX;
        //public string moveY;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            myCmnInterface.move = Vector3.right * joystick_right.Horizontal + Vector3.up * joystick_right.Vertical;
            myCmnInterface.isPress[1] = (joystick_left.Vertical > 0);
        }
    }
}