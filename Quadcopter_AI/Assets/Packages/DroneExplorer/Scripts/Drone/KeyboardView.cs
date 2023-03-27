using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardView : MonoBehaviour
{
    public SliderController _slider_controller;

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
        if(Input.GetKey(KeyCode.LeftControl))
        {
            negative = -1;
        }
        if(Input.GetButton("AllSpinUp"))
        { 
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._ul_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._ur_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.UpArrow))
        {
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._ul_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._ur_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            this._slider_controller._ul_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._ur_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetButton("RotateLeft"))
        {

            this._slider_controller._ul_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetButton("RotateRight"))
        {
            this._slider_controller._ur_slider.value += Time.deltaTime * this._input_amendment * negative;
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            this._slider_controller._ul_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.W))
        {
            this._slider_controller._ur_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.E))
        {
            this._slider_controller._dl_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
        else if(Input.GetKey(KeyCode.R))
        {
            this._slider_controller._dr_slider.value += Time.deltaTime * this._input_amendment * negative;
        }
    }
}
