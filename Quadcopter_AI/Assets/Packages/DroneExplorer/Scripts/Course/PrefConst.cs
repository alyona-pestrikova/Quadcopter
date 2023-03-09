using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Project: 
/// This project enables the exploration of 3D space by way of a quad-copter drone. The intent 
/// is to provide a quick drop in package to allow a user to explore a scene in the Unity Play 
/// engine. The code is written to be modular and can incorporate other gameplay elements with 
/// the addition of more scripts.
/// 
/// Author: Wilson Sauders 
/// Email: HamsterUnity@gmail.com
/// Date:   04 / 05 / 2022
/// 
/// File:
/// I am the PlayerPrefs Const file. To prevent silly one letter off errors. Any get or set to PlayerPrefs will
/// use a const string from this file.
/// </summary>
public class PrefConst 
{
    // general options
    public const string isGhostLine = "isGhostLine";
    public const string isGamePad   = "isGamePad";
    public const string isHardGate  = "isHardGate";
    public const string trackIndex  = "trackIndex";
    public const string lastPicked  = "lastPicked";

    // level specific options appended prior to level name
    public const string bestTime    = "bestTime";
    public const string star1Time   = "star1Time";
    public const string star2Time   = "star2Time";
    public const string star3Time   = "star3Time";
    public const string ghostLine   = "ghostLine";


}
