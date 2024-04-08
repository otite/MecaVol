using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilote : MonoBehaviour
{
    public float Weight = 100f;
    public Transform CenterOfMass;
    public Transform SystemCenterOfMass;
    public Vector3D v3dApparentWeight, v3dFcent;
    public float ComputedWeight => ApparentMassVectorKg.magnitude;

    [Range( -1f, 1f )]
    public float MassControl = 0f;
    private Vector3 ApparentMassVectorKg;
    public Rigidbody body;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.mass = Weight;
        GetComponent<FixedJoint>().massScale = Weight;
        GetComponent<FixedJoint>().connectedMassScale = 6f; //glider weight todo settings
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CenterOfMass.localPosition = new Vector3(MassControl, 0f, 0f);
        body.centerOfMass = CenterOfMass.position;
        //vitesse angulaire w = vitesse car distance fixe entre cg systeme et pilote.
        //fcentrigue = mass*w*w*R
        float forceCentrigugeKg = 0.1019716213f * body.mass * body.velocity.magnitude * body.velocity.magnitude * Vector3.Distance( body.centerOfMass, SystemCenterOfMass.position );
        ApparentMassVectorKg = new Vector3(0,-body.mass, 0) + (body.centerOfMass - SystemCenterOfMass.position).normalized * forceCentrigugeKg; 
        v3dApparentWeight.values = ApparentMassVectorKg;

        Vector3 FCent = (CenterOfMass.position - SystemCenterOfMass.position).normalized * forceCentrigugeKg;
        v3dFcent.transform.position = CenterOfMass.position;
        v3dFcent.values = FCent;
    }
}
