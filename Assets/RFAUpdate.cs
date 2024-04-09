using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RFAUpdate : MonoBehaviour
{
    public Transform SystemCenterOfMass;
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

    [Header( "UI" )]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI speedVectorTxt;
    public TextMeshProUGUI RFAMagText;
    public TextMeshProUGUI RFAVectText;
    public TextMeshProUGUI incidenceTxt;
    public TextMeshProUGUI portanceMagTxt;
    public TextMeshProUGUI portanceVectTxt;

    [Header("Simulate")]
    public bool Simulate;
    public Vector3 SimulatedSpeed;
    public float SimulatedIncidence = 6f;

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
        previousCPPos = CP.position;
    }
    private void Update()
    {
        Time.timeScale = AppManager.Instance.settings.slowMotionTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime * AppManager.Instance.settings.slowMotionTimescale;
    }

    private void FixedUpdate() {
        ComputedCorde = CordeAttaque.position - CordeFuite.position;

        Vector3 speed;//, gliderSpeed ;
        if (Simulate)
        {
            speed = SimulatedSpeed;
        }
        else
        {
            //speed =  rb.GetPointVelocity(CP.position);
            

            speed = (CP.position - previousCPPos) / Time.deltaTime;
            previousCPPos = CP.position;
        }

        //gliderSpeed = (CP.position - previousCPPos) / Time.deltaTime;
        //previousCPPos = CP.position;

        incidence = Vector3.SignedAngle(ComputedCorde, speed, transform.right);
        //assiette = Vector3.SignedAngle(Vector3.ProjectOnPlane(speed, Vector3.up), speed, -transform.right);

        float Cz = AppManager.Instance.settings.GliderCzI.Evaluate(incidence);
        float Cx = AppManager.Instance.settings.GliderCxI.Evaluate(incidence);

        float PortanceMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * speed.magnitude * speed.magnitude * Cz;
        //if (PortanceMag <= 0f) PortanceMag = 0f;
        float TraineeMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * speed.magnitude * speed.magnitude * Cx;
        rb.drag = TraineeMag/9.81f;
        //rb.angularDrag = TraineeMag / 9.81f;
        float RFAMag = Mathf.Sqrt(PortanceMag * PortanceMag + TraineeMag * TraineeMag);


        Vector3 trainee = -speed.normalized * TraineeMag;
        Vector3 portance = Vector3.Cross( speed, transform.right ).normalized * PortanceMag;
        ComputedRFA = portance + trainee;// -pilote.ApparentMassVector.normalized * RFAMag;

        v3dDrag.values = trainee;
        v3dSpeed.values = speed;
        v3dPortance.values = portance;
        v3dRFA.values = ComputedRFA;

        speedText.text = speed.magnitude.ToString();
        speedVectorTxt.text = speed.ToString();
        RFAMagText.text = ComputedRFA.magnitude.ToString();
        RFAVectText.text = ComputedRFA.ToString();
        incidenceTxt.text = incidence.ToString();
        portanceMagTxt.text = PortanceMag.ToString();
        portanceVectTxt.text = portance.ToString();

        if (Simulate)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            //rb.AddForce(ComputedRFA);
            rb.AddForceAtPosition(ComputedRFA, CP.position, ForceMode.Force);
            //rb.AddForceAtPosition(pilote.ApparentMassVector, pilote.CenterOfMass.position, ForceMode.Acceleration);
        }
        //rb.AddForce(ComputedRFA);
    }
    
}
