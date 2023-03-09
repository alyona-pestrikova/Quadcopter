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
    /// I am the drone rigid body function I apply all the thrust and torque values to the rigid body 
    /// to make it seem like it is flying.
    /// </summary>

    public class DroneRigidBody : MonoBehaviour
    {
        public Rigidbody myBody;            // reference to the Rigid Body

        public bool isCamLock;              // flag to use yawObj to turn the drone
        public CmnInterface myInterface;    // interface to pull input from
        public float thrustBase;            // Upward thrust while idle
        public float thrustOffset;          // Additional thrust when myIntefce.Thrust == 1;

        public float upCorrectInput;        // Modify desired up vector by myInterfce.Move
        public float upCorrectMax;          // Maximum offset angle for correction (prevents over correction)
        public float upCorrectMul;          // offset multipler for torque

        public GameObject yawObj;           // external object to dictate the forward vector of the drone
        public float yawOfsMax;             // Maximum offset (prevents over correction)
        public float yawOfsMul;             // offset multipler for torque

        Vector3 resetPt;                    // starting point and orientation for "R button
        Quaternion resetRot;

        public Spinner[] mySpinner;
        public float spinBase;
        public float spinFast;
        public float spinDelta;
        // Start is called before the first frame update
        void Start()
        {
            // set reset point
            resetPt = transform.position;
            resetRot = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            // reset on pressing R key
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = resetPt;
                transform.rotation = resetRot;
                myBody.velocity = Vector3.zero;
                myBody.angularVelocity = Vector3.zero;
            }
        }

        // Apply physics functions
        private void FixedUpdate()
        {

            UpCorrect();
            YawCorrect();
            UpdateThrust();
        }

        /// <summary>
        /// I apply the appropriate torque to keep the drone's up vector poitned the way we want.
        /// </summary>
        void UpCorrect()
        {
            // set Upvect to up plus input modification
            Vector3 upVect = Vector3.up;
            if (yawObj != null && isCamLock)
            {
                upVect += Flat(yawObj.transform.right).normalized * myInterface.move.x * upCorrectInput;
                upVect += Flat(yawObj.transform.forward).normalized * myInterface.move.y * upCorrectInput;
            }
            else
            {
                upVect += Vector3.right * myInterface.move.x;
                upVect += Vector3.forward * myInterface.move.y;
            }

            // get the cross producty and angle vector calculus stuff 
            Vector3 cross = Vector3.Cross(transform.up, upVect);
            float ang = Vector3.Angle(transform.up, upVect);

            // limit ang to bounds
            if (ang < -upCorrectMax) ang = -upCorrectMax;
            if (ang > upCorrectMax) ang = upCorrectMax;

            // apply force based on ang on the cross vector
            myBody.AddTorque(cross.normalized * ang * upCorrectMul, ForceMode.Acceleration);

        }

        /// <summary>
        /// I torqu the forward vector to keep it inline with the heading set by yawObj
        /// </summary>
        void YawCorrect()
        {
            if (yawObj == null) return;
            Vector3 objFw = Flat(yawObj.transform.forward);
            Vector3 selfFw = Flat(transform.forward);

            Vector3 cross = Vector3.Cross(selfFw, objFw);
            float ang = Vector3.Angle(selfFw, objFw);

            // limit ang to bounds
            if (ang < -yawOfsMax) ang = -yawOfsMax;
            if (ang > yawOfsMax) ang = yawOfsMax;

            // apply force basedd on ang
            myBody.AddTorque(cross.normalized * ang * yawOfsMul, ForceMode.Acceleration);
        }


        /// <summary>
        /// I add thrust allong the local up vector base on myInterfcace Thrust or the 2 & 3 interface button
        /// </summary>
        void UpdateThrust()
        {
            float upThrust = 0;

            // apply interface input
            if (myInterface.isPress[2]) upThrust -= 1;
            if (myInterface.isPress[3]) upThrust += 1;

            upThrust += myInterface.thrust;

            // limit bounds
            if (upThrust > 1) upThrust = 1;
            if (upThrust < -1) upThrust = -1;

            float spin = spinBase;
            if (upThrust < 0)
            {
                spin += upThrust * spinBase;
            }
            else
            {
                spin += upThrust * spinFast;
            }

            // compute spin delta
            float delta = spinDelta * Time.deltaTime;
            foreach (Spinner spinner in mySpinner)
            {
                // move spinner speed up or down by delta
                if (spinner.speed < spin - delta)
                {
                    spinner.speed += delta;
                }
                else if (spinner.speed > spin + delta)
                {
                    spinner.speed -= delta;
                }
                else
                {
                    spinner.speed = spin;
                }

            }


            // compute thrust from 0 to 1
            float thrust = thrustBase + upThrust * thrustOffset;

            // special case for thrust < 0 scale thrust from 0 to thrustBase
            if (upThrust < 0)
            {
                thrust = thrustBase * (1.0f + upThrust);
            }

            // add force to body
            myBody.AddForce(transform.up * thrust, ForceMode.Acceleration);
        }


        // used to reomve the Y value from a vector 3
        Vector3 Flat(Vector3 tgt)
        {
            return new Vector3(tgt.x, 0, tgt.z);
        }
    }
}