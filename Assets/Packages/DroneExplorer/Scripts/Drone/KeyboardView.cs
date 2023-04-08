using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardView : MonoBehaviour
{
    public SliderController _slider_controller;
    public Interface _interface;

    private float _input_amendment;

    // Start is called before the first frame update
    void Start()
    {
        this._input_amendment = 0.08f;
    }

    // Update is called once per frame
    void Update()
    {
        this.ButtonCheckout();
    }

    void ButtonCheckout()
    {
        float negative = 1;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            negative = -1;
        }
        if (Input.GetKey(KeyCode.R))
        {
            this._interface.DroneReset();
        }
        if(Input.GetButton("AllSpinUp"))
        { 
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._ul_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._ur_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        if(Input.GetButton("Horizontal"))
        {
            this._slider_controller._dl_slider.value -= -Time.deltaTime * this._input_amendment * Input.GetAxis("Horizontal");
            this._slider_controller._ul_slider.value -= -Time.deltaTime * this._input_amendment * Input.GetAxis("Horizontal");
            this._slider_controller._ur_slider.value += -Time.deltaTime * this._input_amendment * Input.GetAxis("Horizontal");
            this._slider_controller._dr_slider.value += -Time.deltaTime * this._input_amendment * Input.GetAxis("Horizontal");
        }
        if(Input.GetButton("Vertical"))
        {
            this._slider_controller._ul_slider.value -= Time.deltaTime * this._input_amendment * Input.GetAxis("Vertical");
            this._slider_controller._ur_slider.value -= Time.deltaTime * this._input_amendment * Input.GetAxis("Vertical");
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * Input.GetAxis("Vertical");
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * Input.GetAxis("Vertical");
        }
        if(Input.GetButton("RotateLeft"))
        {

            this._slider_controller._ul_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        if(Input.GetButton("RotateRight"))
        {
            this._slider_controller._ur_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
    }
}
