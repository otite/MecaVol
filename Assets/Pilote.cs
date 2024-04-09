using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilote : MonoBehaviour
{
    public Transform CenterOfMass;
    public Transform SystemCenterOfMass;
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
        //GetComponent<FixedJoint>().massScale = Weight;
        //GetComponent<FixedJoint>().connectedMassScale = 6f; //glider weight todo settings
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CenterOfMass.localPosition = new Vector3(MassControl, 0f, 0f);

        Vector3 velocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;

        body.centerOfMass = CenterOfMass.position;
        //vitesse angulaire w = vitesse car distance fixe entre cg systeme et pilote.
        //fcentrigue = mass*w*w*R
        float forceCentriguge = body.mass * velocity.magnitude * velocity.magnitude * Vector3.Distance(CenterOfMass.position, SystemCenterOfMass.position) ;
        ApparentMassVector = body.mass * -Vector3.up;// + (body.centerOfMass - SystemCenterOfMass.position).normalized * forceCentriguge / 9.81f;
        ComputedMass = ApparentMassVector.magnitude;
        body.mass = ComputedMass;

        v3dApparentWeight.values = ApparentMassVector;

        Vector3 FCent = (CenterOfMass.position - SystemCenterOfMass.position).normalized * forceCentriguge / 9.81f;
        v3dFcent.transform.position = CenterOfMass.position;
        v3dFcent.values = FCent;

        
        v3dSpeed.values = velocity;
    }
}
