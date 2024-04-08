using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFAUpdate : MonoBehaviour
{
    public Pilote pilote;
    public Transform CP;
    private Rigidbody rb;
    public Transform CordeFuite, CordeAttaque;
    public float InitialSpeed = 8;

    private Vector3 ComputedRFA;
    private Vector3 ComputedCorde;
    private float assiette;
    private float incidence = 7f;
    private float startFixedDeltaTime;

    [Header( "Vectors 3d" )]
    public Vector3D v3dDrag;
    public Vector3D v3dSpeed;
    public Vector3D v3dPortance;
    public Vector3D v3dRFA;
    //tmp

    private void Awake()
    {

        startFixedDeltaTime = Time.fixedDeltaTime;

        rb = GetComponent<Rigidbody>();
        rb.mass = AppManager.Instance.settings.GliderWeight;
        rb.velocity = Quaternion.AngleAxis(incidence, transform.right) * transform.forward * InitialSpeed;
        pilote.body.velocity = rb.velocity;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        Time.timeScale = AppManager.Instance.settings.slowMotionTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime * AppManager.Instance.settings.slowMotionTimescale;
    }

    private void FixedUpdate() {
        ComputedCorde = CordeAttaque.position - CordeFuite.position;

        Vector3 speed = rb.GetPointVelocity(CP.position);
        assiette = Vector3.SignedAngle(Vector3.ProjectOnPlane(speed, Vector3.up), speed, -transform.right);
        incidence = Vector3.SignedAngle(ComputedCorde, speed, transform.right);

        float Cz = AppManager.Instance.settings.GliderCzI.Evaluate(incidence);
        float Cx = AppManager.Instance.settings.GliderCxCz.Evaluate(Cz);

        float PortanceMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * speed.magnitude * speed.magnitude * Cz;
        float TraineeMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * speed.magnitude * speed.magnitude * Cx;
        float RFAMag = Mathf.Sqrt(PortanceMag * PortanceMag + TraineeMag * TraineeMag);


        Vector3 trainee = -speed.normalized * TraineeMag;
        Vector3 portance = Vector3.Cross( speed, transform.right ).normalized * PortanceMag;
        ComputedRFA = -pilote.ApparentMassVector.normalized * RFAMag;

        v3dDrag.values = trainee;
        v3dSpeed.values = speed;
        v3dPortance.values = portance;
        v3dRFA.values = ComputedRFA;

        //rb.AddForceAtPosition(ComputedRFA, CP.position);
        rb.AddForce(ComputedRFA);
    }
    
}
