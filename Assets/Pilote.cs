using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilote : MonoBehaviour
{
    public Transform CenterOfMass;
    public Vector3 speed, angularSpeed;
    public SystemCenterOfMass SystemCenterOfMass;
    public Vector3D v3dApparentWeight, v3dFcent, v3dSpeed;
    public float ComputedMass;

    [Range( -1f, 1f )]
    public float MassControl = 0f;
    public Vector3 ApparentMassVector;
    public Rigidbody body;


    private Vector3 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        body = GetComponent<Rigidbody>();
        body.mass = AppManager.Instance.settings.PiloteWeight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CenterOfMass.localPosition = new Vector3(MassControl, 0f, 0f);
        body.centerOfMass = body.transform.InverseTransformDirection( CenterOfMass.position - body.transform.position);
        Debug.DrawLine( body.position, body.worldCenterOfMass, Color.red );

        speed = body.velocity;
        //speed = (transform.position - previousPosition) / Time.deltaTime;
        //previousPosition = transform.position;


        //vitesse relative tangantielle par rapport au com
        Vector3 relativeSpeed = Vector3.ProjectOnPlane(speed - SystemCenterOfMass.speed, ( body.centerOfMass - SystemCenterOfMass.transform.position ).normalized );
   
        //vitesse angulaire w = vitesse car distance fixe entre cg systeme et pilote.
        //fcentrigue = mass*w*w*R
        //float forceCentrifuge = body.mass * relativeSpeed.magnitude * relativeSpeed.magnitude / Vector3.Distance(CenterOfMass.position, SystemCenterOfMass.transform.position) ;
        //Vector3 FCent = (CenterOfMass.position - SystemCenterOfMass.transform.position).normalized * forceCentrifuge;

        ApparentMassVector = body.mass * Physics.gravity.y * Vector3.up;// + FCent;

        ComputedMass = ApparentMassVector.magnitude / -Physics.gravity.y;

        v3dApparentWeight.values = ApparentMassVector;
        //v3dFcent.transform.position = CenterOfMass.position;
        //v3dFcent.values = FCent*200f;
        //v3dSpeed.values = speed;


    }
}
