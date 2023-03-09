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
    /// I am the PlayerMouseLook class. I lock the mouse and use its MouseX and MouseY movement axes 
    /// to alter the Common Interface Look value.
    /// </summary>
    public class DroneLook : MonoBehaviour
    {
        public CmnInterface myInterface;    // interface to pull input from
        public bool isInvertY;              // flag for inverting Y input for pitch
        public GameObject centerObj;        // link to the drone object that we will center on
        public GameObject camObj;           // link to the Camera object
        public float camPush;               // multiplier lateral and vertical camera pushing
        public float forceDist;             // the distance this object will be placed from the center obj
        public float levelMul;              // multipier to move the camera back to level
        public float levelMax;              // the maximum value that will be multiplied
        public float levelLock;             // maximum offset on the Y axis
        public Vector3 camOfs;              // offset from this object that the camera will be palced. Used for over the sholder view.

        public Vector3 lastOffset;                 // remember last offset 
        public float dragFrac;              // between 0-1 to determain how much drag should be applied to the final position

        public GameObject pitchAimer;
        //public Gun[] myGuns;

        // Start is called before the first frame update
        void Start()
        {
            Reset();
        }

        public void Reset()
        {
            // set initial value fo last offset
            lastOffset = transform.position - centerObj.transform.position;

        }

        // Update is called once per frame
        void Update()
        {
            // compute the new position if we let the camera drag behind
            Vector3 dragPos = CalcPointWithInput(transform.position);
            // compute the new position if we were bound to the object with no camera drag
            Vector3 basePos = CalcPointWithInput(centerObj.transform.position + lastOffset);

            // set new position to a lerp between dragPos and basePos
            transform.position = Vector3.Lerp(basePos, dragPos, dragFrac);

            // sent new orientation by looking at centerObj
            transform.LookAt(centerObj.transform.position);

            // copy orientation to camara
            camObj.transform.rotation = transform.rotation;

            // Offset camera by camOfs using local coordinates
            camObj.transform.position = transform.position +
                transform.right * camOfs.x +
                transform.up * camOfs.y +
                transform.forward * camOfs.z;


            // store position to offset base for next iteration
            lastOffset = transform.position - centerObj.transform.position;

            if (pitchAimer != null)
            {
                // revert to parent's rotation
                pitchAimer.transform.rotation = pitchAimer.transform.parent.rotation;
                Vector3 localForward = pitchAimer.transform.InverseTransformVector(transform.forward);
                // remove X component
                localForward = new Vector3(0, localForward.y, localForward.z);
                Vector3 lookPt = pitchAimer.transform.position +
                    pitchAimer.transform.forward * localForward.z +
                    pitchAimer.transform.up * localForward.y;

                pitchAimer.transform.LookAt(lookPt, pitchAimer.transform.up);
            }
        }

        /// <summary>
        /// Update helper function used apply input values to a position.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        Vector3 CalcPointWithInput(Vector3 pos)
        {
            // get pushed by input
            pos -= transform.right * myInterface.look.x * camPush * Time.deltaTime;
            if (isInvertY)
            {
                pos += transform.up * myInterface.look.y * camPush * Time.deltaTime;
            }
            else
            {
                pos -= transform.up * myInterface.look.y * camPush * Time.deltaTime;
            }


            // apply autoleveling;
            float offset = centerObj.transform.position.y - pos.y;

            if (offset > levelLock)
            {
                pos = Flat(pos) + Vector3.up * (centerObj.transform.position.y - levelLock);
            }
            else if (offset < -levelLock)
            {
                pos = Flat(pos) + Vector3.up * (centerObj.transform.position.y + levelLock);
            }
            else
            {
                if (offset < -levelMax) offset = -levelMax;
                if (offset > levelMax) offset = levelMax;
                pos += Vector3.up * offset * levelMul * Time.deltaTime;
            }

            // force distance from centerObj
            Vector3 toSelf = pos - centerObj.transform.position;
            pos = centerObj.transform.position + toSelf.normalized * forceDist;

            // return modified point
            return pos;
        }

        // used to reomve the Y value from a vector 3
        Vector3 Flat(Vector3 tgt)
        {
            return new Vector3(tgt.x, 0, tgt.z);
        }
    }
}