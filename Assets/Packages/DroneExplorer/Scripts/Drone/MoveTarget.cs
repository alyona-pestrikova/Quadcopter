using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(Random.value * 7, Random.value * 7, Random.value * 7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
