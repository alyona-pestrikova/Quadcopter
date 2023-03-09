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
    /// I am the target control class. I contain all the target groups and cycle 
    /// between them to give the player something to do.
    /// </summary>
    public class TargetCtrl : MonoBehaviour
    {
        public int index;               // current index
        public bool isRandom;           // flag for linear swap between target groups or random
        public TargetGroup[] myGroups;  // list of groups, this is loaded from children
        public GameObject pointer;      // game object that is in the player's view port and will be
                                        // pointed at the active target group
        public GameObject player;       // pointer to player for computing fly through


        // Start is called before the first frame update
        void Start()
        {
            // get tharget groups in children
            myGroups = GetComponentsInChildren<TargetGroup>();

            // pick random target group
            int next = 0;
            if (isRandom) next = Random.Range(0, myGroups.Length);
            foreach (TargetGroup group in myGroups)
            {
                group.SetHide();
            }
            // set starting
            index = next;
            myGroups[next].SetShow();
        }

        // Update is called once per frame
        void Update()
        {
            // safty check 
            if (myGroups.Length <= 0) return;

            // check if current groups is done and swap groups
            if (myGroups[index].isDone)
            {
                SetNextGroup();
            }
            UpdatePointer();

        }

        // move to next group
        void SetNextGroup()
        {
            // index values to compute on default to linear progression
            int old = index;
            int next = index + 1;
            // loop if next overflows
            if (next >= myGroups.Length)
            {
                next = 0;
            }
            // random logic
            if (isRandom)
            {
                next = Random.Range(0, myGroups.Length);
                // Disallow next and olde being the same
                if (next == old)
                {
                    if (next > myGroups.Length / 2)
                    {
                        next -= 1;
                    }
                    else
                    {
                        next += 1;
                    }
                }
            }

            // call respective group hide and show
            myGroups[old].SetHide();
            myGroups[next].SetShow();
            index = next;
        }

        void UpdatePointer()
        {
            if (pointer == null) return;    // don't update if no pointer
            GameObject obj = myGroups[index].gameObject;
            float minDist = float.MaxValue;
            foreach(PopTarget tgt in myGroups[index].tgts)
            {
                float dist = Vector3.Distance(player.transform.position, tgt.transform.position);
                if(dist < minDist)
                {
                    dist = minDist;
                    obj = tgt.gameObject;
                }
            }
            // aim pointer act current group
            if (pointer != null) pointer.transform.LookAt(obj.transform.position);

        }

    }
}