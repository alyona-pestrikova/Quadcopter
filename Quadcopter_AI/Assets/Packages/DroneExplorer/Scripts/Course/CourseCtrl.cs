using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
    /// Date:   03 / 28 / 2022 
    /// 
    /// File:
    /// I am the Course Controll Class I maintain all the variables and states for the timed attack 
    /// course gameplay. I utailize Target Group Objects as gates to fly through or shoot. I keep track
    /// of previous times and game play variables through player prefs. I also interact with canvass 
    /// elements to inform the player of the current game status.
    /// </summary>
    public class CourseCtrl : MonoBehaviour
    {
        
        public int visitCur;            // flag of the current visited gate 0 means starting

        public Color colGateCur;        // Color of the current Gate's particles
        public Color colGateNext;       // Color of the next Gate's particles
        public Color colGateOther;      // Color of all the otherg ates that are not Current or Next

        public int gateCur;             // index of the current gate in myGates
        public TargetGroup[] myGates;   // list of groups, this is loaded from children
        public GameObject pointer;      // game object that is in the player's view port and will be
                                        // pointed at the active target group
        public Vector3 pointerOffset;   // offset from camera the poitner is bound to
        public DroneLook myLooker;      // the camera controller of the player's drone. used to aim at the next gate if 
                                        // the look correct is active.
        public float lookCorrect = 1;   // used to force the camera to look a the next gate. 0 means no camera correction, range should be between 0 and 2
        public float lookMax = 90;      // maximum offset lookCorrect will multiply by. leave as is

        public DroneRigidBody player;       // pointer to player for computing fly through

        // these paramites store the starting location of the player and looker
        Vector3 playerPosOld;           
        Quaternion playerRotOld;
        Vector3 myLookerOld;            // used to reset myLooker position on forced reset
        public Vector3 lastPos;         // used in gate pass through computation
        // Start is called before the first frame update

        public UnityEngine.UI.Text tbCurTime;   // UI text element that will show current time
        public UnityEngine.UI.Text tbBestTime;  // UI text element that will show best time
        public UnityEngine.UI.Text tbTopText;   // UI text element that will popup breefly to tell the player how they did 
        public GameObject uiPostGame;           // wrapper object for the post game UI elements.
        public GameObject[] uiStars;            // wrapper object for the 3 star sprites 
        // default times for 1, 2, and 3 star performances. This should be overwritten in the editor foe each
        // level based on how long it is
        TimeSpan[] uiStarTime = {  new TimeSpan(0, 0, 80), new TimeSpan(0, 0, 60), new TimeSpan(0, 0, 50) };
        // brief tutorial message that accopanies tbTopText
        public UnityEngine.UI.Text tbBottomText;

        DateTime startTime;     // start time of run, modifed by pause feature
        TimeSpan bestTime;      // time span of best run
        public int tbBottomTime;    // time out time of bottom text

        public GhostLine myGhostLine;   // reference to the ghost line object that remembers the path of the player's last best time

        // player settings
        public bool isGhostLine;        // local flag to enable/disable ghost line
        public bool isGamePad;          // local flag to enable/disable Wasd or gamepad input
        public bool isHardGate;         // local flag to enable or disable the colliders on the gates
        public int trackIndex;          // index of the track we are on (for main menu)

        public GameObject inpWasd;      // local Object that contains the mouse keyboard controler input script
        public GameObject inpGamePad;   // local Object that containse the X box controller input script
        float[] lookMul = { 0.0f, 10, 50, 200}; // modifes look Correct 

        // pause screen
        public GameObject uiPauseMenu;  // pause menu wrapper
        Vector3 pauseVelo;              // storage for velocity of player at time of pausing
        Vector3 pauseRot;               // storage for angular velocity of player at time of pausing
        public string uiQuitScene = "MainMenu"; // reference to the scene to quit back to on game exit.

        void Start()
        {
            // if gates were not pre populated, pull gates from components in children
            if (myGates.Length == 0)
            {
                myGates = GetComponentsInChildren<TargetGroup>();
            }
            // store current position of objects in to "old" values for future reset
            myLookerOld = myLooker.transform.position;
            playerPosOld = player.transform.position;
            playerRotOld = player.transform.rotation;
            // call reset function to reset everything
            Reset();
            // set default best time to a very big number
            bestTime = new TimeSpan(23, 59, 59);
            //move the pointer obect into the main camera
            pointer.transform.parent = Camera.main.transform;
            pointer.transform.localPosition = pointerOffset;

            // read prefs on startup
            LoadPrefs();

        }

        /// <summary>
        /// Load paramiters from PlayerPrefs. Some will be level specific.
        /// </summary>
        void LoadPrefs()
        {
            string scene = SceneManager.GetActiveScene().name;
            // get static values
            isGhostLine = (PlayerPrefs.GetString(PrefConst.isGhostLine, isGhostLine.ToString() ) == true.ToString()) ;
            isGamePad = (PlayerPrefs.GetString(PrefConst.isGamePad, isGamePad.ToString()) == true.ToString());
            isHardGate = (PlayerPrefs.GetString(PrefConst.isHardGate, isHardGate.ToString()) == true.ToString());
            trackIndex = PlayerPrefs.GetInt (PrefConst.trackIndex, trackIndex);
            // get level values
            string levname = SceneManager.GetActiveScene().name;
            // get best Time
            int milsec = PlayerPrefs.GetInt(PrefConst.bestTime + levname, 86399999);
            bestTime = new TimeSpan(0, 0, 0, 0, milsec);

            // get Star times
            uiStarTime[0] = new TimeSpan(0, 0, PlayerPrefs.GetInt(PrefConst.star1Time + levname, (int)(uiStarTime[0].TotalSeconds)));
            uiStarTime[1] = new TimeSpan(0, 0, PlayerPrefs.GetInt(PrefConst.star2Time + levname, (int)(uiStarTime[1].TotalSeconds)));
            uiStarTime[2] = new TimeSpan(0, 0, PlayerPrefs.GetInt(PrefConst.star3Time + levname, (int)(uiStarTime[2].TotalSeconds)));
            // set look Correct to the indexed paramiter
            if (trackIndex > lookMul.Length-1) trackIndex = lookMul.Length - 1;
             lookCorrect = lookMul[trackIndex];
            // set WASD or Xbox input
            inpWasd.SetActive(!isGamePad);
            inpGamePad.SetActive(isGamePad);

            // set all colliders based off isHardGate
            if (!isHardGate)
            {
                foreach (TargetGroup gate in myGates)
                {
                    Collider[] cols = gate.GetComponentsInChildren<Collider>();
                    foreach (Collider col in cols)
                    {
                        // ignore hit taker colliders
                        HitTaker taker = col.GetComponent<HitTaker>();
                        if(taker == null)
                        {
                            col.enabled = false;
                        }
                    }
                }
            }

            // populate myGhostLine 
            myGhostLine.ReadPath(levname);
        }


        /// <summary>
        /// Save the best time and the ghost line to player Prefs, so that future plays can see them.
        /// </summary>
        public void SavePrefs()
        {
            string levname = SceneManager.GetActiveScene().name;

            int time = (int)(bestTime.TotalMilliseconds);
            PlayerPrefs.SetInt(PrefConst.bestTime + levname, time);

            // Write best ghost line to storage
            myGhostLine.WritePath(levname);
        }

        /// <summary>
        /// This is called on startup and when ever the player resets the run.
        /// </summary>
        public void Reset()
        {
            startTime = DateTime.Now;
            gateCur = myGates.Length - 1;
            SetNextGroup();
            visitCur = 0;   // flags this as pre trial mode
            lastPos = myGates[gateCur].transform.InverseTransformPoint(player.transform.position);

            // copy old points to player and looker objects
            player.transform.position = playerPosOld;
            player.transform.rotation = playerRotOld;
            myLooker.transform.position = myLookerOld;
            myLooker.transform.rotation = playerRotOld;
            myLooker.Reset();
            // reset all pop target gates
            foreach(TargetGroup gate  in myGates)
            {
                gate.SetHide();
            }
            // reset the Ghost line
            myGhostLine.Restart(false);
            // unpause if Reset was triggered by dialog
            Unpause();
        }

        // Update is called once per frame
        void Update()
        {
            // reset time on R button
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reset();

            }

            // check for pause or unpause
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (uiPauseMenu.activeSelf)
                {
                    Unpause();
                }
                else
                {
                    Pause();
                }
            }
            // change start time if in pause
            if (uiPauseMenu.activeSelf)
            {
                startTime = startTime.AddSeconds(Time.deltaTime);
                myGhostLine.startTime = startTime;

                if (Input.GetKeyDown(KeyCode.Q)) SaveAndQuit();
                return;
            }

            // check if this a pass through gate 
            if (myGates[gateCur].tgts.Length == 0)
            {
                CheckFlyGate();
            }
            else if(myGates[gateCur].isDone)// this is a through gate
            { 
                SetNextGroup();
            }

            // pass player's position to ghost line
            if (visitCur != 0 && myGhostLine != null)
            {
                myGhostLine.UpdatePoint(player.transform.position, isGhostLine);
            }

            // update sub functions
            UpdateTimer();
            UpdatePointer();
            UpdateLooker();
        }

        /// <summary>
        /// If the current gate is a fligh through gate, check if the user passed through it.
        /// </summary>
        void CheckFlyGate()
        {

            Vector3 inversePlayer = myGates[gateCur].transform.InverseTransformPoint(player.transform.position);
            Vector3 toPlayer = inversePlayer - lastPos;
            // check if passing on Z axis
            if ((lastPos.z <= 0 && inversePlayer.z > 0) || (lastPos.z >= 0 && inversePlayer.z < 0))
            {
                Vector3 inPlain = lastPos + toPlayer * lastPos.z;
                if (lastPos.z < 0) inPlain = lastPos - toPlayer * lastPos.z;

                // check for box gate
                if (myGates[gateCur].gateDim.x > 0)
                {
                    if (Mathf.Abs(inPlain.x) < myGates[gateCur].gateDim.x &&
                        Mathf.Abs(inPlain.y) < myGates[gateCur].gateDim.y)
                    {
                        SetNextGroup();
                        // reset inversePlayer on new gate
                        inversePlayer = myGates[gateCur].transform.InverseTransformPoint(player.transform.position);
                    }
                }
                else
                {
                    // ring detection
                    if(inPlain.x*inPlain.x + inPlain.y * inPlain.y < myGates[gateCur].gateDim.y* myGates[gateCur].gateDim.y)
                    {
                        SetNextGroup();
                        // reset inversePlayer on new gate
                        inversePlayer = myGates[gateCur].transform.InverseTransformPoint(player.transform.position);
                    }
                }
            }

            // store into lastPos for new calculation
            lastPos = inversePlayer;

        }

        /// <summary>
        /// Incriment gate/target group.
        /// </summary>
        void SetNextGroup()
        {
            // if pass through gate 0 we are at the end of the run, Check for new best time, and show star ranking.
            if(gateCur == 0 )
            {
                TimeSpan span = DateTime.Now - startTime;
                startTime = DateTime.Now;
                // if not initial start pass 
                if(visitCur > 0)
                {
                    if(span < bestTime)
                    {   // new best time found
                        bestTime = span;
                        // congradulate player on new best time
                        uiPostGame.SetActive(true);
                        tbBottomText.text = "New Best Time";

                        // write new best time to long term storage
                        // comment out this line if you want to have high score saving be done on level exit.
                        SavePrefs();

                        //Copy path into ghost line for next time
                        myGhostLine.Restart(true);
                    }
                    else
                    {
                        // showbottom text for non high score
                        uiPostGame.SetActive(true);
                        tbBottomText.text = TimeOfsToStr(span);
                        myGhostLine.Restart(false);
                    }
                    // Show the high schore star counter
                    // clear each star
                    foreach(GameObject obj in uiStars){
                        obj.SetActive(false);
                    }
                    // show Stars
                    if(span > uiStarTime[0])
                    {
                        tbTopText.text = "Next Star: " + TimeOfsToStr(uiStarTime[0]);
                        uiStars[0].SetActive(true);
                    }
                    else if(span > uiStarTime[1])
                    {
                        tbTopText.text = "Next Star: " + TimeOfsToStr(uiStarTime[1]);
                        uiStars[1].SetActive(true);
                    }
                    else if (span > uiStarTime[2])
                    {
                        tbTopText.text = "Next Star: "+ TimeOfsToStr(uiStarTime[2]);
                        uiStars[2].SetActive(true);
                    }
                    else 
                    {
                        tbTopText.text = "Great Job";
                        uiStars[3].SetActive(true);
                    }

                }// end if not initial gate pass
                else
                {   // do ghost line reset on inital pass but none of the other stuff
                    myGhostLine.Restart(false);
                }

            }

            // index values to compute on default to linear progression
            int old = gateCur;
            int next = gateCur + 1;
            // loop if next overflows
            if (next >= myGates.Length)
            {
                next = 0;
            } 


            // call respective group hide and show
            myGates[old].SetHide();
            myGates[next].SetShow();
            gateCur = next;

            // set particle systems for each gate
            // figure out next gate index
            next = gateCur + 1;
            // loop if next overflows
            if (next >= myGates.Length)
            {
                next = 0;
            }
            // go through all the gates setting their color to Cur, Next, or Other
            for(int itor= 0; itor < myGates.Length; itor++)
            {
                if(itor == gateCur)
                {
                    myGates[itor].SetColor(colGateCur);
                }
                else if(itor == next)
                {
                    myGates[itor].SetColor(colGateNext);
                }
                else
                {
                    myGates[itor].SetColor(colGateOther);
                }
            }

            // incriment visit
            visitCur += 1;
        }

        /// <summary>
        /// Updates the timer text boxes: tbBestTime, tbBottomText, and uiPostGame
        /// </summary>
        void UpdateTimer()
        {
            TimeSpan span = DateTime.Now - startTime;
            if (tbCurTime != null)
            {
                if (visitCur == 0)
                {   // show start mode if visitCur indicates a pre run state
                    tbCurTime.text = "Start";
                }
                else
                {   // show time
                    tbCurTime.text = TimeOfsToStr(span);
                }
            }
            // always show best time if the UI element exists
            if(tbBestTime !=null)
            {
                tbBestTime.text = TimeOfsToStr(bestTime);
            }

            // show bottom text or hide it 
            if(tbBottomText != null)
            {
                if (visitCur == 0)
                {
                    uiPostGame.SetActive(true);
                    tbTopText.text = "R to Reset, P to Pause / Quit";
                    tbBottomText.text = "Fly Through First Gate";
                    foreach(GameObject obj in uiStars) { obj.SetActive( false); }
                }
                else
                {
                    // clear text if time expires
                    if (span.Seconds > tbBottomTime && tbBottomText.text != "")
                    {
                        uiPostGame.SetActive(false);
                    }
                }
            } 

        }

        /// <summary>
        /// Convert time span to a string format
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        string TimeOfsToStr(TimeSpan span)
        {
            return span.Hours.ToString("00") + ":" + 
                span.Minutes.ToString("00") + ":" + 
                span.Seconds.ToString("00") + "." + 
                span.Milliseconds.ToString("000");
        }

        /// <summary>
        /// Aim the pointer object at the next gate
        /// </summary>
        void UpdatePointer()
        {
            if (pointer == null) return;    // don't update if no pointer
            GameObject obj = myGates[gateCur].gameObject;
            float minDist = float.MaxValue;
            foreach (PopTarget tgt in myGates[gateCur].tgts)
            {
                float dist = Vector3.Distance(player.transform.position, tgt.transform.position);
                if (dist < minDist)
                {
                    dist = minDist;
                    obj = tgt.gameObject;
                }
            }
            // aim pointer act current group
            if (pointer != null) pointer.transform.LookAt(obj.transform.position);

        }

        /// <summary>
        /// Update the looker object to aim at the next target
        /// </summary>
        void UpdateLooker()
        {
            if (myLooker == null || pointer == null) return;
            float ang = Vector3.Angle(pointer.transform.forward, myLooker.transform.forward);
            if(ang > lookMax)
            {
                ang = lookMax;
            }
            // don't force look in early explore mode
            if (visitCur != 0)
            {
                myLooker.lastOffset -= pointer.transform.forward * lookCorrect * (ang / lookMax)*Time.deltaTime;
            }
        }


        /// <summary>
        /// Initate pause mode including UI elements
        /// </summary>
        public void Pause()
        {
            uiPauseMenu.SetActive(true);

            pauseVelo = player.myBody.velocity;
            pauseRot = player.myBody.angularVelocity;
            player.myBody.isKinematic = true;

            if(isGamePad == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        /// <summary>
        ///  Disable pause mode including UI elements
        /// </summary>
        public void Unpause()
        {
            uiPauseMenu.SetActive(false);

            player.myBody.velocity = pauseVelo;
            pauseRot = player.myBody.angularVelocity;
            player.myBody.isKinematic = false;

            if (isGamePad == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        /// <summary>
        /// Button callback for quit button. 
        /// </summary>
        public void SaveAndQuit()
        {
            SavePrefs();
            SceneManager.LoadScene(uiQuitScene);

        }
    }
}
