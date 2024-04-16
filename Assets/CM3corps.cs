using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM3corps : MonoBehaviour
{
    public Rigidbody gliderBody, piloteBody, centerOfMassBody;
    public ConfigurableJoint Pilote2SCOMJoint, SCOM2GliderJoint;
    float DistPG;
    // Start is called before the first frame update
    void Start()
    {
        DistPG = Vector3.Distance(gliderBody.position, piloteBody.position);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //CM.position = ((b1.position * b1.mass + b2.position * b2.mass) / (b1.mass + b2.mass));
        //b1.centerOfMass = b1.transform.InverseTransformPoint(((b1.position * b1.mass + b2.position * b2.mass) / (b1.mass + b2.mass)));
        //Debug.DrawLine(b1.transform.position, b1.worldCenterOfMass);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            piloteBody.AddForce(new Vector3(-10000, 0, 0));
        }
        SCOM2GliderJoint.connectedAnchor = new Vector3(0, -DistPG * (piloteBody.mass/(gliderBody.mass+piloteBody.mass)), 0);
        Pilote2SCOMJoint.connectedAnchor = new Vector3(0, -DistPG * (gliderBody.mass/(gliderBody.mass+piloteBody.mass)), 0);

        gliderBody.AddForceAtPosition((gliderBody.mass + piloteBody.mass + centerOfMassBody.mass) * -Physics.gravity.y * Vector3.up, gliderBody.transform.position);
    }
}
