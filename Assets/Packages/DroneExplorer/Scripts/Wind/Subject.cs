using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Скрипт подключен к DroneRigidBody
//
//В этом скрипте проверяется, попадает ли наш объект rb в зону ветра и, если это так,
//к rb прикладывается сила, с которой действует ветер.

public class Subject : MonoBehaviour
{

    private bool inWindZone = false;
    public GameObject windZone;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "windArea")
        {
            windZone = coll.gameObject;
            inWindZone = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "windArea")
        {
            inWindZone = false;
        }
    }

    void FixedUpdate()
    {
        if (inWindZone)
        {
            rb.AddForce(windZone.GetComponent<WindArea>().direction * windZone.GetComponent<WindArea>().strength, ForceMode.Acceleration);
        }
    }
}