using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Project: 
/// This project enables the exploration of 3D space by way of a quad-copter drone. The intent 
/// is to provide a quick drop in package to allow a user to explore a scene in the Unity Play 
/// engine. The code is written to be modular and can incorporate other gameplay elements with 
/// the addition of more scripts.
/// 
/// Author: Wilson Sauders 
/// Email: HamsterUnity@gmail.com
/// Date:   04 / 08 / 2022
/// 
/// File:
/// I am the Menu Class. I hold the callback functions for the options menu and track user level pick
/// </summary>
public class MenuCtrl : MonoBehaviour
{
    public GameObject rootMain;
    public GameObject rootLevSelect;
    public GameObject rootOptions;
    public GameObject rootCredits;

    public GameObject btBack;
    public GameObject btStart;

    public Toggle tgGhostLine;
    public Toggle tgGamePad;
    public Toggle tgHardGate;
    public Scrollbar sbCamTrack;

    public LevelMarker[] myLevelMarkers;
    public RectTransform highlgiht;
    public int picked;

    public Text description;
    // Start is called before the first frame update
    void Start()
    {
        if(myLevelMarkers.Length == 0)
            myLevelMarkers = GetComponentsInChildren<LevelMarker>();
        for(int itor = 0; itor <myLevelMarkers.Length; itor++)
        {
            Button btCur = myLevelMarkers[itor].GetComponent<Button>();
            if(btCur != null)
            {
                int tmp = itor; // local copy
                btCur.onClick.AddListener(delegate { SetLevelMarker(tmp); });
            }
        }

        SetScreen(0);
        LoadPrefs();
    }

    void LoadPrefs()
    {
        picked = PlayerPrefs.GetInt(PrefConst.lastPicked, 0);
        if (picked >= myLevelMarkers.Length) picked  = 0; // safety check for marker length
        SetLevelMarker(picked);
        tgGhostLine.isOn = (PlayerPrefs.GetString(PrefConst.isGhostLine, true.ToString()) == true.ToString());
        tgGamePad.isOn = (PlayerPrefs.GetString(PrefConst.isGamePad, false.ToString()) == true.ToString());
        tgHardGate.isOn = (PlayerPrefs.GetString(PrefConst.isHardGate, true.ToString()) == true.ToString());
        int cam = PlayerPrefs.GetInt(PrefConst.trackIndex,2);
        sbCamTrack.value = 0.01f + (float)cam / 4.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScreen(int screen)
    {
        rootMain.SetActive(screen == 0);
        rootLevSelect.SetActive(screen == 1);
        rootOptions.SetActive(screen == 2);
        rootCredits.SetActive(screen == 3);
        if (screen == 4) Application.Quit();

        // set back button if not in Main Menu screen
        btBack.SetActive(screen != 0);
        // create start button 
        btStart.SetActive(screen == 1);
    }

    public void CopyRef()
    {
        GUIUtility.systemCopyBuffer = "https://assetstore.unity.com/packages/templates/packs/drone-explorer-pack-216095";
    }

    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SetLevelMarker(int _picked)
    {
        picked = _picked;
        RectTransform tgt = myLevelMarkers[picked].GetComponent<RectTransform>();
        highlgiht.transform.position = tgt.transform.position;
        description.text = myLevelMarkers[picked].description.Replace("|", System.Environment.NewLine);

    }


    public void StartButton()
    {
        // write player prefs options
        PlayerPrefs.SetString(PrefConst.isGhostLine, tgGhostLine.isOn.ToString());
        PlayerPrefs.SetString(PrefConst.isGamePad, tgGamePad.isOn.ToString());
        PlayerPrefs.SetString(PrefConst.isHardGate, tgHardGate.isOn.ToString()) ;
        PlayerPrefs.SetInt(PrefConst.trackIndex,(int)(0.01f + sbCamTrack.value*4));


        PlayerPrefs.SetInt(PrefConst.lastPicked, picked);

        // set star times for level
        PlayerPrefs.SetInt(PrefConst.star1Time+ myLevelMarkers[picked].levelName, myLevelMarkers[picked].starTimes[0]);
        PlayerPrefs.SetInt(PrefConst.star2Time+ myLevelMarkers[picked].levelName, myLevelMarkers[picked].starTimes[1]);
        PlayerPrefs.SetInt(PrefConst.star3Time+ myLevelMarkers[picked].levelName, myLevelMarkers[picked].starTimes[2]);

        SceneManager.LoadScene(myLevelMarkers[picked].levelName);
    }

}
