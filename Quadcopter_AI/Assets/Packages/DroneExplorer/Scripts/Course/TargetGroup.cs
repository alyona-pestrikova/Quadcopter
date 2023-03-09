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
    /// I am the target group object. I contain a group of PopTargets and activate a random set at
    /// the behest of TargetCtrl. 
    /// Update: I now contain a particle system for fly through gates. If I have no PopTargets I will 
    /// requiret the player to fly through me to complete.
    /// </summary>

    public class TargetGroup : MonoBehaviour
    {
        public PopTarget[] tgts;    // all PopTarget objects inside me

        public bool isDone;         // flag for TargetCtrl to indicate all PopTargets are hit
        public float timeOut = 5;   // time after the last target is hit before isDone will flag

        public float popPercent;    // random percent of using a single pop target
                                    // Start is called before the first frame update

        // variables for fly through gate
        public Vector3 gateDim = new Vector3(1,1,1);
        public ParticleSystem[] gateParticle;
        void Start()
        {
            // get targets in group if no other function has done so
            if (tgts.Length == 0) tgts = GetComponentsInChildren<PopTarget>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isDone) return; // do nothing if done

            // check each pop terget
            bool done = true;
            foreach (PopTarget tgt in tgts)
            {
                // condition is if the frag time is pre destruction or < timeout.
                // Also that the tgt is in showing or shown. Since some tgts will be
                // in hidden mode we sould not count them
                if (tgt.fragTime < timeOut && (tgt.mode == 2 || tgt.mode == 1)) done = false;
            }
            // if no tgt is still valid we can set isDone to true
            if (done) isDone = true;
        }

        /// <summary>
        /// Called externaly to set some targets to the showns State
        /// </summary>
        public void SetShow()
        {
            // get targets in group if no other function has done so
            if (tgts.Length == 0) tgts = GetComponentsInChildren<PopTarget>();

            // set isDone flag to false so targetCtrl will not immediatly dismiss this group
            isDone = false;

            // randomly activate some of the targets
            int count = 0;
            foreach (PopTarget tgt in tgts)
            {
                if (Random.Range(0, 100) < popPercent)
                {
                    tgt.SetShow();
                    count++;
                }
            }

            // check if none were activated and activate 0th one
            if (count == 0 && tgts.Length > 0) tgts[0].SetShow();
        }

        // Called to move active targets to hidden state
        public void SetHide()
        {
            // get targets in group if no other function has done so
            if (tgts.Length == 0) tgts = GetComponentsInChildren<PopTarget>();
            isDone = true;
            foreach (PopTarget tgt in tgts)
            {
                if (tgt.mode == 2)
                    tgt.SetHide();
            }

        }

        /// <summary>
        /// Set the color of all the particle emmiters for the gate
        /// </summary>
        /// <param name="col"></param>
        public void SetColor(Color col)
        {
            foreach(ParticleSystem prt in gateParticle)
            {
                ParticleSystem.MainModule settings = prt.main;
                settings.startColor = col;
                if (col.a == 0)
                {
                    prt.Stop();
                }
                else
                {
                    prt.Play();
                }
            }
        }
    }
}