using UnityEngine;

public class MoveDrone : MonoBehaviour
{
    public Rigidbody _d_body; // link to "DroneRigidBody" obj

    public Spinner[] _spinners; // spinner objs

    public Interface _interface; // link drone with input

    public Arrow _arrow; // show direction of drone movement

    public float _max_spinner_speed; // max spinner speed



    // Start is called before the first frame update
    void Start()
    {
        this._max_spinner_speed = 5000;

        // Set spinner traction direction
        this._spinners[0]._traction = Spinner.Traction.Direct;
        this._spinners[1]._traction = Spinner.Traction.Direct;
        this._spinners[2]._traction = Spinner.Traction.Inverse;
        this._spinners[3]._traction = Spinner.Traction.Inverse;
    }




    // Update is called once per sometime
    void FixedUpdate()
    {
        // Adds force from the side of every spinner
        foreach (var spinner in this._spinners)
        {
            this._d_body.AddForceAtPosition(0.5f * transform.up * spinner._speed * Time.fixedDeltaTime * (float)spinner._traction, spinner.transform.position);
        }




    }




    
    private void OnEnable()
    {
        this._interface._onSpeedChanged += SpinnerSpeedUpdate; //Add subscribe to change speed event
    }

    
    private void OnDisable()
    {
        this._interface._onSpeedChanged -= SpinnerSpeedUpdate; //Delete subscribe to change speed event
    }


    //Change spinner speed
    private void SpinnerSpeedUpdate()
    {
        this._spinners[0]._next_speed = this._interface._ul_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[1]._next_speed = this._interface._dr_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[2]._next_speed = -this._interface._dl_spinner_speed_factor * this._max_spinner_speed;
        this._spinners[3]._next_speed = -this._interface._ur_spinner_speed_factor * this._max_spinner_speed;
    }
}
