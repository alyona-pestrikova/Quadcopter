using UnityEngine;

public class MoveDrone : MonoBehaviour
{
    public Rigidbody _d_body; // link to "DroneRigidBody" obj

    public Spinner[] _spinners; // spinner objs

    public Interface _interface; // link drone with input

    public Arrow _arrow; // show direction of drone movement

    public float _max_spinner_speed; // max spinner speed

    private Vector3 _result_vector_;

    // Start is called before the first frame update
    void Start()
    {
        this._max_spinner_speed = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called once per sometime
    void FixedUpdate()
    {
       

    }





    //Calc result power direction
    private Vector3 CaclResultDirection()
    {
        return new Vector3(0,0,0);
    }



    
    private void OnEnable()
    {
        this._interface._onSpeedChanged += SpinnerSpeedUpdate; //Add subscribe to event
    }

    
    private void OnDisable()
    {
        this._interface._onSpeedChanged -= SpinnerSpeedUpdate; //Delete subscribe to event
    }


    //Change spinner speed
    private void SpinnerSpeedUpdate()
    {
        this._spinners[0]._next_speed = this._interface._ul_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[1]._next_speed = this._interface._dr_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[2]._next_speed = this._interface._dl_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[3]._next_speed = this._interface._ur_spinner_speed_factor * this._max_spinner_speed;
    }
}
