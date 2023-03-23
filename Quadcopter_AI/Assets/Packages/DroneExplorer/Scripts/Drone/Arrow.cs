using UnityEngine;

public class Arrow : MonoBehaviour
{

    public Vector3 _axis; // show direction of drone movement

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_axis);
    }
}
