using UnityEngine;

public class Arrow : MonoBehaviour
{

    public Vector3 _axis; // shows direction of drone movement
    public GameObject _cylinder; // cylinder object for scaling length of the arrow
    public GameObject _drone_rb; // drone rigid body object for changing the arrow's position

    void Start()
    {
        transform.position = _drone_rb.transform.position;
    }
    
    void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, _axis);
        _cylinder.transform.localScale = new Vector3(0.007f, _axis.magnitude * 1 / 500 /*0.01f + _axis.magnitude / 20*/, 0.006f);
        transform.position = _drone_rb.transform.position /*+ new Vector3(0, _cylinder.transform.localScale.y / 2, 0)*/;
    }
}