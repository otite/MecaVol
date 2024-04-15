using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RFAUpdate : MonoBehaviour
{
    public SystemCenterOfMass systemCenterOfMass;
    public Pilote pilote;
    public CentrePoussee CP;
    private Rigidbody rb;
    public Transform CordeFuite, CordeAttaque, RightBrakePoint, LeftBrakePoint;
    public float InitialSpeed = 8;
    [Range(0,500)]
    public float RightBrake;
    [Range(0, 500)]
    public float LeftBrake;
    public float roulis;

    private Vector3 ComputedRFA;
    private Vector3 ComputedCorde;
    private Vector3 Trainee, Portance;
    private float assiette;
    public float incidence = 7f;
    private float startFixedDeltaTime;
    Vector3 Speed;

    [Header( "Vectors 3d" )]
    public Vector3D v3dDrag;
    public Vector3D v3dSpeed;
    public Vector3D v3dPortance;
    public Vector3D v3dRFA;

    [Header( "UI" )]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI speedVectorTxt;
    public TextMeshProUGUI RFAMagText;
    public TextMeshProUGUI RFAVectText;
    public TextMeshProUGUI incidenceTxt;
    public TextMeshProUGUI roulisTxt;
    public TextMeshProUGUI portanceMagTxt;
    public TextMeshProUGUI portanceVectTxt;

    [Header("Simulate")]
    public bool Simulate;
    public Vector3 SimulatedSpeed;
    public float SimulatedIncidence = 8f;

    private Vector3 previousCPPos;

    private void Awake()
    {

        startFixedDeltaTime = Time.fixedDeltaTime;

        rb = GetComponent<Rigidbody>();
        rb.mass = AppManager.Instance.settings.GliderWeight;
        //if (!Simulate)
        //{
            rb.velocity = Quaternion.AngleAxis(incidence, transform.right) * transform.forward * InitialSpeed;
            pilote.body.velocity = rb.velocity;
        //}
    }
    // Start is called before the first frame update
    void Start()
    {
        previousCPPos = CP.transform.position;
    }
    private void Update()
    {
        Time.timeScale = AppManager.Instance.settings.slowMotionTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime * AppManager.Instance.settings.slowMotionTimescale;


        v3dDrag.values = Trainee;
        v3dSpeed.values = Speed;
        v3dPortance.values = Portance;
        v3dRFA.values = ComputedRFA;

        speedText.text = (Speed.magnitude*3.6f).ToString() + " km/h";
        speedVectorTxt.text = Speed.ToString();
        RFAMagText.text = ComputedRFA.magnitude.ToString();
        RFAVectText.text = ComputedRFA.ToString();
        incidenceTxt.text = "Incidence : "+incidence.ToString();
        portanceMagTxt.text = Portance.magnitude.ToString();
        portanceVectTxt.text = Portance.ToString();
        roulisTxt.text = "Roulis : "+roulis.ToString();
    }

    private void FixedUpdate() {
        

        //, gliderSpeed ;
        if (Simulate)
        {
            Speed = SimulatedSpeed;
        }
        else
        {
            Speed =  rb.GetPointVelocity(CP.transform.position );
            //Speed = (CP.transform.position - previousCPPos) / Time.deltaTime;
            previousCPPos = CP.transform.position;
        }

        ComputedCorde = CordeAttaque.position - CordeFuite.position;
        roulis = Vector3.SignedAngle(transform.up, Vector3.ProjectOnPlane(Vector3.up, transform.forward), transform.forward);
        incidence = Vector3.SignedAngle(ComputedCorde, Vector3.ProjectOnPlane(Speed,transform.right), transform.right);
        CP.UpdatePosition( incidence, roulis );
        float Cz = AppManager.Instance.settings.GliderCzI.Evaluate(incidence);
        float Cx = AppManager.Instance.settings.GliderCxI.Evaluate(incidence);

        float PortanceMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * Speed.magnitude * Speed.magnitude * Cz;
        //if (PortanceMag <= 0f) PortanceMag = 0f;
        float TraineeMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * Speed.magnitude * Speed.magnitude * Cx;
        //rb.drag = TraineeMag/9.81f;
        //rb.angularDrag = TraineeMag / 9.81f;
        //float RFAMag = Mathf.Sqrt(PortanceMag * PortanceMag + TraineeMag * TraineeMag);


        Trainee = -Speed.normalized * TraineeMag;
        Portance = Vector3.Cross( Speed, transform.right ).normalized * PortanceMag;
        ComputedRFA = Portance + Trainee;// -pilote.ApparentMassVector.normalized * RFAMag;

       

        if (Simulate)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            //rb.AddForce(ComputedRFA);
            rb.AddForceAtPosition(ComputedRFA, CP.transform.position, ForceMode.Force);

            rb.AddForceAtPosition(Vector3.Project(-ComputedCorde.normalized * RightBrake * Speed.magnitude, Trainee), RightBrakePoint.position, ForceMode.Force);
            rb.AddTorque(transform.up * RightBrake );

            rb.AddForceAtPosition(Vector3.Project(-ComputedCorde.normalized * LeftBrake * Speed.magnitude, Trainee), LeftBrakePoint.position, ForceMode.Force);
            rb.AddTorque(transform.up * -LeftBrake);


            rb.AddTorque(transform.up * roulis * 50f);

            //rb.AddForceAtPosition(pilote.ApparentMassVector, pilote.CenterOfMass.position, ForceMode.Acceleration);
        }
        //rb.AddForce(ComputedRFA);
    }
    
}
