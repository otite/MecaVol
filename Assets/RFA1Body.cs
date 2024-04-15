using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFA1Body : MonoBehaviour
{
    public float GliderWeight = 6f;
    public float PiloteWeight = 100f;
    public Rigidbody rb;
    public Transform CordeFuite, CordeAttaque, CP, PilotCenterOfMass;
    Vector3 ComputedCorde, Speed, Portance, Trainee, ComputedRFA;
    float incidence=6f, roulis;
    public Vector3D v3dDrag;
    public Vector3D v3dSpeed;
    public Vector3D v3dPortance;
    public Vector3D v3dRFA;

    private void Awake()
    {
        rb.velocity = Quaternion.AngleAxis(incidence, transform.right) * transform.forward * 8f;
        incidence = Vector3.SignedAngle(ComputedCorde, Speed, transform.right);

    }
    // Start is called before the first frame update
    void Start()
    {
        rb.mass = GliderWeight + PiloteWeight;
        rb.centerOfMass = rb.transform.InverseTransformPoint(((CP.position * GliderWeight + PilotCenterOfMass.position * PiloteWeight) / (PiloteWeight + GliderWeight)));
        //rb.velocity = Quaternion.AngleAxis(incidence, transform.right) * transform.forward * 8f;
    }

    private void Update()
    {
        
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        ComputedCorde = CordeAttaque.position - CordeFuite.position;

        Speed = rb.GetPointVelocity(CP.transform.position);
        incidence =  Vector3.SignedAngle(ComputedCorde, Speed, transform.right);
        //CP.UpdatePosition(incidence, 0f);
        float Cz = AppManager.Instance.settings.GliderCzI.Evaluate(incidence);
        float Cx = AppManager.Instance.settings.GliderCxI.Evaluate(incidence);

        float PortanceMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * Speed.magnitude * Speed.magnitude * Cz;
        //if (PortanceMag <= 0f) PortanceMag = 0f;
        float TraineeMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * Speed.magnitude * Speed.magnitude * Cx;
        //rb.drag = TraineeMag/9.81f;
        //rb.angularDrag = TraineeMag / 9.81f;
        //float RFAMag = Mathf.Sqrt(PortanceMag * PortanceMag + TraineeMag * TraineeMag);


         Trainee = -Speed.normalized * TraineeMag;
         Portance = Vector3.Cross(Speed, transform.right).normalized * PortanceMag;
         ComputedRFA = Portance + Trainee;// -pilote.ApparentMassVector.normalized * RFAMag;
        rb.AddForceAtPosition(ComputedRFA, CP.transform.position, ForceMode.Force);
        //rb.AddForceAtPosition(Physics.gravity * 100f, PilotCenterOfMass.position, ForceMode.Force);


        Debug.DrawLine(CP.transform.position, rb.worldCenterOfMass, Color.red);
        v3dDrag.values = Trainee;
        v3dSpeed.values = Speed;
        v3dPortance.values = Portance;
        v3dRFA.values = ComputedRFA;


    }
}
