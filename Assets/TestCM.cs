using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCM : MonoBehaviour
{
    public Rigidbody b1, b2;
    public Transform CP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = (b1.position * b1.mass + b2.position * b2.mass) / (b1.mass + b2.mass);
        b1.centerOfMass = b1.transform.InverseTransformPoint(transform.position);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            b2.AddForce(new Vector3(-10000, 0, 0));
        }
        b1.AddForceAtPosition((b1.mass+b2.mass)*-Physics.gravity.y*Vector3.up, CP.position);
    }
}
