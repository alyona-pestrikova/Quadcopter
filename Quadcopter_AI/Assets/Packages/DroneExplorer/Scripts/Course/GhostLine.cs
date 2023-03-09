using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

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
/// I am the Ghost Line Class. I track an object over a period of time. I can then draw a tracing line for that object
/// that represented a previous object's movment. I have functions that can load and save a path to the player prefs. 
/// </summary>
public class GhostLine : MonoBehaviour
{
    public TrailRenderer render;    // the object that draws the trail.
    public long delta;              // time gap between
    public long listMax;            // the maximum size fo the array
    public List<Vector3> listCur;   // the list that new updates atre added
    public List<Vector3> listOld;   // the list that the trail rendere follows

    public DateTime startTime;  // used to calculate time offsets
    public float detail;        // used to multiply the XYZ values of the Vertex so that we can write the point to int
    // Start is called before the first frame update
    void Start()
    {
    }

    // Don't use this and instead use UpdatePoint from controller class.
    void Update()
    {
        
    }

    /// <summary>
    /// This should be called at the Start of a run to restart the tracking of the player. 
    /// If the copy argument is true, we copy the the listCur to the listOld container.
    /// </summary>
    /// <param name="copy"></param>
    public void Restart(bool copy)
    {
        // Copy listCur to listOld, by pionter shifting
        if (copy)
        {
            listOld = listCur;
            listCur = new List<Vector3>();
        }
        // ensure listCur empty
        listCur.Clear();
        startTime = DateTime.Now;   // restart timer
        render.gameObject.SetActive(false);
        // reset render position
        if (listOld.Count > 0)
        {
            render.transform.position = listOld[0];
        }
        else
        {
            render.transform.position = transform.position;
        }
        // clear path so that the line render does not jump
        render.Clear();
    }

    /// <summary>
    /// This should be called once per update where the point is on the move. Also if the
    /// showPath flag is up we update the trail render "render"
    /// </summary>
    /// <param name="point"></param>
    /// <param name="showPath"></param>
    public void UpdatePoint(Vector3 point, bool showPath)
    {
        // get time span since start
        TimeSpan span = DateTime.Now - startTime;
        // calculate which frame we are in based off timespan from start
        long wantFrame = span.Ticks / delta;
        // Add new point to list cur if we are in a want frame that has not already been added to the list.
        // Also prevent list from growing beyond list max here
        if(wantFrame > listCur.Count && wantFrame < listMax)
        {
            // note we mulyiply by detail and Round so that we don't store decimals in the stering.
            Vector3 pt = new Vector3(Mathf.Round(point.x* detail), Mathf.Round(point.y * detail), Mathf.Round(point.z * detail));
            listCur.Add(pt);
        }

        // Place render if the want frame is valid to listOld
        if(wantFrame > 0 &&  wantFrame < listOld.Count-1 && showPath)
        {
            long mod = span.Ticks - wantFrame * delta;  // delta time since last want fram
            Vector3 last = listOld[(int)(wantFrame) - 1];   // start and endpoints to lerp
            Vector3 next = listOld[(int)(wantFrame)];
            float frac = (float)(mod) / ((float)(delta));   // lerp fraction
            render.transform.position =  Vector3.Lerp(last, next, frac)/ detail;    // place at lerp calculoated point
           
            // Set render active if it wasn't already. This is mostly done for debugging so that
            // the develiper can turn the trail on and off in the editor
            if(render.gameObject.activeSelf == false)
                render.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// This is the store to memory function for this path. It writes each point as X,Y,Z with "|" between 
    /// points. We concatinate PrefConst.ghostLine and the level paramiter to create a new key for PlayerPrefs.
    /// This will allow us to save one line per level.
    /// </summary>
    /// <param name="level"></param>
    public void WritePath(string level)
    {
        StringBuilder builder = new StringBuilder();
        for(int itor = 0; itor < listOld.Count; itor++)
        {
            builder.Append((int)(listOld[itor].x));
            builder.Append(",");
            builder.Append((int)(listOld[itor].y));
            builder.Append(",");
            builder.Append((int)(listOld[itor].z));
            builder.Append("|");
        }
        PlayerPrefs.SetString(PrefConst.ghostLine + level, builder.ToString());
    }

    /// <summary>
    /// This reads the data from PlayerPrefs that WritePath inserted and compiles the OldList from it
    /// </summary>
    /// <param name="level"></param>
    public void ReadPath(string level)
    {

        string line = PlayerPrefs.GetString(PrefConst.ghostLine + level, "");
        listOld.Clear();
        char[] bar = { '|' };
        char[] comma = { ',' };
        string[] points = line.Split(bar);
        foreach(string point in points)
        {
            try
            {   // skip any bad data
                string[] vect = point.Split(comma);
                Vector3 pt = new Vector3(Convert.ToInt32(vect[0]), Convert.ToInt32(vect[1]), Convert.ToInt32(vect[2]));
                listOld.Add(pt);
            }
            catch { }
        }
    }
}
