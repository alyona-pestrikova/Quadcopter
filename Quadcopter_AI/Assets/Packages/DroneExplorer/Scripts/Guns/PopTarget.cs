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
    /// I am the popup target object. I control a target that can move between two points when called 
    /// to show and hide. When the hit takers of the target exceed damage, I will also throw parts off 
    /// to show destruction.
    /// </summary>

    public class PopTarget : MonoBehaviour
    {
        public int mode = 0; // 0 -  Hide, 1 - move to show, 2 - show, 3 - move to hide, 4 - hide
        public GameObject baseObj;          // base object that will be moved
        public GameObject hidePos;          // position for base object in hiding state
        public GameObject showPos;          // position for base boject in shown state

        public float lerpTime = 1;          // time to swap between hide and show
        float lerpCur = 0;                  // timer
        public GameObject[] fragObj;        // Objects to fragment off base (must not be child of base)
        public GameObject[] fragStart;      // non destructed positions of frag(childed to base)
        Vector3[] fragVelo;                 // internal storage for velocity of fragment
        Vector3[] fragSpin;                 // internal storage for rotation of fragment
        public float fragVeloPush;          // initial velocity based off fragment's offset from base
        public float fragVeloRand;          // random alteration to initial velocity
        public float fragSpinRand;          // random spin given to fragmenr
        public float fragTime = -1;         // state of fragment  < 0 means stuck to base, > 0 is flying
        public float fragTimeMax;           // how long fragment falls (does not collide with ground)

        public HitTaker[] takers;           // hit takers
        public float maxDammage;            // maximum dammage before fragmenting.

        // Start is called before the first frame update
        void Start()
        {
            // initilize velo and spin
            fragVelo = new Vector3[fragObj.Length];
            fragSpin = new Vector3[fragObj.Length];

            for (int itor = 0; itor < fragStart.Length && itor < fragObj.Length; itor++)
            {
                fragVelo[itor] = fragSpin[itor] = Vector3.zero;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // calculare frac based on lerpCur Value
            float frac = lerpCur / lerpTime;
            // do different thing based on mode
            switch (mode)
            {
                case 0: // 0 -  Hide,
                        // force base to Hide position
                    baseObj.transform.position = hidePos.transform.position;
                    baseObj.transform.rotation = hidePos.transform.rotation;
                    break;
                case 1: // 1 - move to show,
                    lerpCur -= Time.deltaTime;
                    // check for finish of transition ans swap to mode 2 shownig
                    if (lerpCur < 0)
                    {
                        lerpCur = 0;
                        mode = 2;
                    }
                    // lerp objects
                    baseObj.transform.position = Vector3.Lerp(showPos.transform.position, hidePos.transform.position, frac);
                    baseObj.transform.rotation = Quaternion.Lerp(showPos.transform.rotation, hidePos.transform.rotation, frac);
                    break;
                case 2: // 2 - show,
                        // Refactor to UpdateShow 
                    UpdateShow();
                    break;
                case 3: // 3 - move to hide,
                    lerpCur -= Time.deltaTime;
                    // check for finish of transition
                    if (lerpCur < 0)
                    {
                        lerpCur = 0;
                        mode = 0;
                    }
                    // lerp objects
                    baseObj.transform.position = Vector3.Lerp(hidePos.transform.position, showPos.transform.position, frac);
                    baseObj.transform.rotation = Quaternion.Lerp(hidePos.transform.rotation, showPos.transform.rotation, frac);
                    break;
            }

            // always update fragments
            UpdateFrag();

        }

        /// <summary>
        /// While in show state check dammage and trigger frag
        /// </summary>
        void UpdateShow()
        {
            // force base to shown position
            baseObj.transform.position = showPos.transform.position;
            baseObj.transform.rotation = showPos.transform.rotation;

            // Check if object is not destroyed by useing fragTime
            if (fragTime < 0)
            {
                float dammage = 0; // dammage counter
                foreach (HitTaker taker in takers)
                {
                    dammage += taker.dammage;
                }

                // check damage counter for destruction state
                if (dammage >= maxDammage)
                {
                    // start fragment counter
                    fragTime = 0;
                    // set initial velocity
                    for (int itor = 0; itor < fragStart.Length && itor < fragObj.Length; itor++)
                    {   // set intitial put
                        fragVelo[itor] = fragStart[itor].transform.position - gameObject.transform.position;
                        fragVelo[itor] = fragVelo[itor].normalized * fragVeloPush;// set initial push
                        fragVelo[itor] += new Vector3(Random.Range(-fragVeloRand, fragVeloRand),
                            Random.Range(-fragVeloRand, fragVeloRand), Random.Range(-fragVeloRand, fragVeloRand));
                        // set initial rotation
                        fragSpin[itor] += new Vector3(Random.Range(-fragSpinRand, fragSpinRand),
                            Random.Range(-fragSpinRand, fragSpinRand), Random.Range(-fragSpinRand, fragSpinRand));
                    }
                    // remove taker colliders from scene
                    foreach (HitTaker taker in takers)
                    {
                        Collider col = taker.GetComponent<Collider>();
                        if (col != null) col.enabled = false;
                    }
                }
            }


        }

        /// <summary>
        /// Place framents where they are supposed to be
        /// </summary>
        void UpdateFrag()
        {
            // check if frag time is pre frag and lock frament to the FragStart position
            if (fragTime < 0)
            {
                for (int itor = 0; itor < fragStart.Length && itor < fragObj.Length; itor++)
                {   // place objects at startobject
                    fragObj[itor].transform.position = fragStart[itor].transform.position;
                    fragObj[itor].transform.rotation = fragStart[itor].transform.rotation;
                }
                return;
            }

            // stop moving after fragTimeMax
            if (fragTime > fragTimeMax) return;

            // update frag time
            fragTime += Time.deltaTime;

            // place frag objects with velocity and rotation by time
            for (int itor = 0; itor < fragStart.Length && itor < fragObj.Length; itor++)
            {   // place objects at startobject
                fragObj[itor].transform.position = fragStart[itor].transform.position;
                fragObj[itor].transform.rotation = fragStart[itor].transform.rotation;

                // modify position by initial velocity and rotation
                fragObj[itor].transform.position += fragVelo[itor] * fragTime;
                fragObj[itor].transform.Rotate(fragSpin[itor] * fragTime);
                // update with gravity
                fragObj[itor].transform.position += fragTime * fragTime * Physics.gravity * 0.5f;
            }

        }

        /// <summary>
        /// Called by external functions to start animation from hide to Show. Also reactivate collider.
        /// </summary>
        public void SetShow()
        {
            fragTime = -1;
            foreach (HitTaker taker in takers)
            {
                Collider col = taker.GetComponent<Collider>();
                if (col != null) col.enabled = true;
                taker.dammage = 0;
            }
            lerpCur = lerpTime;
            mode = 1;
        }

        /// <summary>
        /// Called by external functions to start animation from show to hide
        /// </summary>
        public void SetHide()
        {
            lerpCur = lerpTime;
            mode = 3;
        }
    }
}