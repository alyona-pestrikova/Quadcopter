using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LidarInterface : MonoBehaviour
{

    public TMP_Text _canvas_text; //Text on the screen
    public string _name;          //Name is direction of lidar

    // Start is called before the first frame update
    void Start()
    {
        _name = _canvas_text.text;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<DroneRay>()._nearest_obj_name != "")
        {
            _canvas_text.text = _name + " " + GetComponent<DroneRay>()._nearest_obj_name +
                            " " + GetComponent<DroneRay>()._nearest_distance;
        } else
        {
            _canvas_text.text = _name;
        }
    }
}
