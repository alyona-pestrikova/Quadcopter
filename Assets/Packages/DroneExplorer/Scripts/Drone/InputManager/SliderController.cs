using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    public Slider _ul_slider; // upper left spinner speed
    public Slider _ur_slider; // upper right spinner speed
    public Slider _dl_slider; // down left spinner speed
    public Slider _dr_slider; // down right spinner speed


    public Interface _interface; // input interface

    //Adds listeners on Value field at slider
    //Speed factor changes when value change
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


    private void OnEnable()
    {
        this._interface._onResetCall += SliderReset;
    }
    private void OnDisable()
    {
        this._interface._onResetCall -= SliderReset;
    }

    void SliderReset()
    {
        _ul_slider.value = _interface._ul_spinner_speed_factor;
        _ur_slider.value = _interface._ur_spinner_speed_factor;
        _dr_slider.value = _interface._dr_spinner_speed_factor;
        _dl_slider.value = _interface._dl_spinner_speed_factor;
    }

}
