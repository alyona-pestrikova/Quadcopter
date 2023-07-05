using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System;

public class MoveDrone : Drone
{

    private Vector3 _max;
    private Vector3 _min;


    public GameObject _target;
    public SliderController _slider_controller;
    public override void OnEpisodeBegin()
    {
        this._max = new Vector3(2, 8, 18);
        this._min = new Vector3(-17, -1.29f, -1.2f);
        this._interface.DroneReset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Func<Vector3, Vector3, Vector3, Vector3> norm_vec = (vector, max_vector, min_vector) => {
            Func<float, float, float, float> norm_coord = (value, max_value, min_value) => (value - min_value) / (max_value - min_value);
            Vector3 rez = new Vector3();
            rez.x = norm_coord(vector.x, max_vector.x, min_vector.x);
            rez.y = norm_coord(vector.y, max_vector.y, min_vector.y);
            rez.z = norm_coord(vector.z, max_vector.z, min_vector.z);
            return rez;
        };

        Vector3 local_position = norm_vec(transform.localPosition, this._max, this._min); // [0,1]
        Vector3 local_target_position = norm_vec(this._target.transform.localPosition, this._max, this._min); // [0,1]
        Vector3 velocity = norm_vec(this._d_body.velocity, this._max, this._min); // [0,1]
        Vector3 angular_velocity = norm_vec(this._d_body.angularVelocity, this._max, this._min); // [0,1]

        Quaternion rotation = transform.rotation; 
        Vector3 norm_rotation= rotation.eulerAngles / 360.0f;  // [0,1]

        sensor.AddObservation(local_position);
        sensor.AddObservation(norm_rotation);
        sensor.AddObservation(velocity);
        sensor.AddObservation(local_target_position);
        sensor.AddObservation(angular_velocity);


        sensor.AddObservation(this._slider_controller._dl_slider.value);
        sensor.AddObservation(this._slider_controller._ul_slider.value);
        sensor.AddObservation(this._slider_controller._dr_slider.value);
        sensor.AddObservation(this._slider_controller._ur_slider.value);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (this._interface._block_input)
        {
            return;
        }
        this._slider_controller._dl_slider.value += Time.deltaTime * actionBuffers.ContinuousActions[0];
        this._slider_controller._ul_slider.value += Time.deltaTime * actionBuffers.ContinuousActions[1];
        this._slider_controller._dr_slider.value += Time.deltaTime * actionBuffers.ContinuousActions[2];
        this._slider_controller._ur_slider.value += Time.deltaTime * actionBuffers.ContinuousActions[3];



        if ((this.transform.position - this._target.transform.position).magnitude < (this._pr_position - this._target.transform.position).magnitude)
        {
            SetReward(0.1f);
        }

        this._pr_position = transform.position;

        if (this.IsTrigger)
        {
            SetReward(1);
        }

        if (this.transform.localPosition.z >= this._max.z || this.transform.localPosition.z <= this._min.z ||
            this.transform.localPosition.x >= this._max.x || this.transform.localPosition.x <= this._min.x ||
            this.transform.localPosition.y <= this._min.y || this.transform.localPosition.y >= this._max.y ||
            this.IsCollision)
        {
            SetReward(-1);
        }

        if (this.transform.localPosition.z >= this._max.z || this.transform.localPosition.z <= this._min.z ||
            this.transform.localPosition.x >= this._max.x || this.transform.localPosition.x <= this._min.x ||
            this.transform.localPosition.y <= this._min.y || this.transform.localPosition.y >= this._max.y ||
            this.IsTrigger || this.IsCollision)
        {
            EndEpisode();
        }
        


        //SetReward(-0.01f);

    }

}
