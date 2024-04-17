using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilote : MonoBehaviour
{
  
    [Range( -1f, 1f )]
    public float MassControl = 0f;
    
    private Rigidbody body;
    


    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        //body.mass = AppManager.Instance.settings.PiloteWeight;
    }

    private void Update()
    {
        //CenterOfMass.localPosition = new Vector3(MassControl, 0f, 0f);

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        body.centerOfMass = body.transform.InverseTransformDirection(body.position + new Vector3(MassControl, 0f, 0f) - body.transform.position);
        //body.centerOfMass = new Vector3(MassControl, 0f, 0f);
        Debug.DrawLine( body.position, body.worldCenterOfMass, Color.red );

        //speed = body.velocity;

        ////fcent pilote
        ////vitesse relative tangantielle par rapport au com
        //Vector3 relativeSpeed = Vector3.ProjectOnPlane(speed - SCOM.velocity, ( body.worldCenterOfMass - SCOM.position ).normalized );
   
        ////vitesse angulaire w = vitesse car distance fixe entre cg systeme et pilote.
        ////fcentrigue = mass*w*w*R
        //float forceCentrifuge = body.mass * relativeSpeed.magnitude * relativeSpeed.magnitude / Vector3.Distance(body.worldCenterOfMass, SCOM.position) ;
        //Vector3 FCent = (body.worldCenterOfMass - SCOM.position).normalized * forceCentrifuge;

        //ApparentMassVector = body.mass * Physics.gravity.y * Vector3.up;// + FCent;

        //ComputedMass = body.mass * (1 + (FCent.magnitude / -Physics.gravity.y) / body.mass);// / -Physics.gravity.y;

        //v3dApparentWeight.values = ApparentMassVector;
        //v3dFcent.transform.position = body.position;
        //v3dFcent.values = FCent;
        //v3dSpeed.values = speed;


    }
}
