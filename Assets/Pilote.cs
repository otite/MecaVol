using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilote : MonoBehaviour
{
    public Transform CenterOfMass;
    public Transform SystemCenterOfMass;
    public Vector3D v3dApparentWeight, v3dFcent;
    public float ComputedWeight => ApparentMassVector.magnitude;

    [Range( -1f, 1f )]
    public float MassControl = 0f;
    public Vector3 ApparentMassVector;
    public Rigidbody body;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.mass = AppManager.Instance.settings.PiloteWeight;
        //GetComponent<FixedJoint>().massScale = Weight;
        //GetComponent<FixedJoint>().connectedMassScale = 6f; //glider weight todo settings
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CenterOfMass.localPosition = new Vector3(MassControl, 0f, 0f);
        body.centerOfMass = CenterOfMass.position;
        //vitesse angulaire w = vitesse car distance fixe entre cg systeme et pilote.
        //fcentrigue = mass*w*w*R
        float forceCentriguge = body.mass * AppManager.Instance.glider.body.angularVelocity.magnitude * AppManager.Instance.glider.body.angularVelocity.magnitude * Vector3.Distance(CenterOfMass.position, SystemCenterOfMass.position) ;
        ApparentMassVector = body.mass * Physics.gravity.y * Vector3.up + (body.centerOfMass - SystemCenterOfMass.position).normalized * forceCentriguge; 
        v3dApparentWeight.values = ApparentMassVector;

        Vector3 FCent = (CenterOfMass.position - SystemCenterOfMass.position).normalized * forceCentriguge;
        v3dFcent.transform.position = CenterOfMass.position;
        v3dFcent.values = FCent;
    }
}
