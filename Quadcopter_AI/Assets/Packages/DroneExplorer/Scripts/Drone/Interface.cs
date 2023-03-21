using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interface : MonoBehaviour
{

    public float _ul_spinner_speed_factor; // upper left spinner speed factor [0;1]
    public float _ur_spinner_speed_factor; // upper right spinner speed factor [0;1]
    public float _dl_spinner_speed_factor; // down left spinner speed factor [0;1]
    public float _dr_spinner_speed_factor; // down right spinner speed factor [0;1]

    public Action _onSpeedChanged; //action react when speed is changing

    // Call by input interface
    public void SpeedUpdate()
    {
        this._onSpeedChanged?.Invoke();
    }

}
