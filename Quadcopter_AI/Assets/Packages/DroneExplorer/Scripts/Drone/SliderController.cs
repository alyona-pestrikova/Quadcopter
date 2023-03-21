using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    public Slider _ul_slider; // upper left spinner speed
    public Slider _ur_slider; // upper right spinner speed
    public Slider _dl_slider; // down left spinner speed
    public Slider _dr_slider; // down right spinner speed


    public Interface _interface; // input interface

    //Add listeners on Value field at slider
    //Speed factor change when value change
    void Start()
    {
        this._ul_slider.onValueChanged.AddListener((value) => {
            this._interface._ul_spinner_speed_factor = value;
            this._interface.SpeedUpdate();
        });
        this._ur_slider.onValueChanged.AddListener((value) => {
            this._interface._ur_spinner_speed_factor = value;
            this._interface.SpeedUpdate();
        });
        this._dl_slider.onValueChanged.AddListener((value) => {
            this._interface._dl_spinner_speed_factor = value;
            this._interface.SpeedUpdate();
        });
        this._dr_slider.onValueChanged.AddListener((value) => {
            this._interface._dr_spinner_speed_factor = value;
            this._interface.SpeedUpdate();
        });
    }

}
