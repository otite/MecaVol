using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCM1Corp : MonoBehaviour
{
    public Rigidbody b1;
    public Transform piloteCom, CP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //CM.position = ((b1.position * b1.mass + b2.position * b2.mass) / (b1.mass + b2.mass));
        b1.centerOfMass = b1.transform.InverseTransformPoint(((CP.position * b1.mass + piloteCom.position * 100f) / (b1.mass + 100f)));
        Debug.DrawLine(b1.transform.position, b1.worldCenterOfMass);
        
        //b1.AddForceAtPosition((b1.mass ) * -Physics.gravity.y * Vector3.up, CP.position);
    }
}
