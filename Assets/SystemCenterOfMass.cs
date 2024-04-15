using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCenterOfMass : MonoBehaviour
{
    public Pilote pilote;
    public Glider glider;
    public Rigidbody SystemBody;
    public CentrePoussee CP;
    public Vector3 speed;
    public Vector3D v3dSpeed;

    public Transform DebugTrs;
    private Vector3 oldPosition;

    public Rigidbody body;
    private void Start() {
        oldPosition = transform.position;
    }


    private void Update()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        SystemBody.centerOfMass = SystemBody.transform.InverseTransformDirection(body.position - SystemBody.position);
        body.MovePosition((pilote.body.worldCenterOfMass + (glider.body.mass / (glider.body.mass + pilote.body.mass)) * (glider.CenterOfMass.position - pilote.body.worldCenterOfMass)));
        Debug.DrawLine(SystemBody.position, SystemBody.worldCenterOfMass, Color.green);
        Debug.DrawLine(pilote.body.worldCenterOfMass, SystemBody.worldCenterOfMass, Color.green);
        Debug.DrawLine(glider.CenterOfMass.position, pilote.body.position, Color.cyan);

        //ProcessraycastPositioning();
    }

    private float ProcessraycastPositioning() {
        RaycastHit hit;

        Vector3 p2Com = transform.position - pilote.CenterOfMass.position;
        Vector3 com2gDir = Vector3.Reflect( p2Com, glider.transform.right );
        // Does the ray intersect any objects excluding the player layer
        if( Physics.Raycast( transform.position, /*transform.TransformDirection( com2gDir )*/ Vector3.ProjectOnPlane(pilote.CenterOfMass.up, Vector3.right), out hit, Mathf.Infinity ) ) {
            Debug.DrawRay( transform.position, Vector3.ProjectOnPlane( pilote.CenterOfMass.up, Vector3.right ) * hit.distance, Color.yellow );
            //Debug.Log( "Did Hit" );
        } else {
            Debug.DrawRay( transform.position, Vector3.ProjectOnPlane( pilote.CenterOfMass.up, Vector3.right ) * 1000, Color.white );
            //Debug.Log( "Did not Hit" );
        }
        DebugTrs.position = hit.point;
        CP.transform.position = hit.point;
        return 0f;
    }
}
