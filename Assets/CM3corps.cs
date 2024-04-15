using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM3corps : MonoBehaviour
{
    public Rigidbody b1, b2, CM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //CM.position = ((b1.position * b1.mass + b2.position * b2.mass) / (b1.mass + b2.mass));
        b1.centerOfMass = b1.transform.InverseTransformPoint(((b1.position * b1.mass + b2.position * b2.mass) / (b1.mass + b2.mass)));
        Debug.DrawLine(b1.transform.position, b1.worldCenterOfMass);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            b2.AddForce(new Vector3(-10000, 0, 0));
        }
        b1.AddForceAtPosition((b1.mass + b2.mass) * -Physics.gravity.y * Vector3.up, b1.transform.position);
    }
}
