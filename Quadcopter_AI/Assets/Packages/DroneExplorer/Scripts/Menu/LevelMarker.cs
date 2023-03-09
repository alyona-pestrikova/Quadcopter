using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelMarker : MonoBehaviour
{
    public string levelName;
    public string title;
    public string description;

    public Text uiTitle;
    public Text uiBestTime;

    public GameObject[] stars;
    public int[] starTimes;
    TimeSpan bestTime;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        //if(String.IsNullOrWhiteSpace(uiTitle.text))
        //{
            uiTitle.text = title;
        //}
    }

    public void Reset()
    { 
        int milsec = PlayerPrefs.GetInt(PrefConst.bestTime + levelName, 86399999);
        bestTime = new TimeSpan(0, 0, 0, 0, milsec);
        
        // hide all stars
        foreach(GameObject star in stars) { star.SetActive(false); }
        
        // show valid Star
        if (bestTime.TotalSeconds > starTimes[0])
        {
            stars[0].SetActive(true);
        }
        else if (bestTime.TotalSeconds > starTimes[1])
        {
            stars[1].SetActive(true);
        }
        else if (bestTime.TotalSeconds > starTimes[2])
        {
            stars[2].SetActive(true);
        }
        else
        {
            stars[3].SetActive(true);
        }

        uiBestTime.text = TimeOfsToStr(bestTime);
    }

    string TimeOfsToStr(TimeSpan span)
    {
        return span.Hours.ToString("00") + ":" +
            span.Minutes.ToString("00") + ":" +
            span.Seconds.ToString("00") ;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
