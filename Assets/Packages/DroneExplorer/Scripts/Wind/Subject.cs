using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������ ��������� � DroneRigidBody
//
//� ���� ������� �����������, �������� �� ��� ������ rb � ���� ����� �, ���� ��� ���,
//� rb �������������� ����, � ������� ��������� �����.

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