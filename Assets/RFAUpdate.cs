using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RFAUpdate : MonoBehaviour
{
    //public SystemCenterOfMass systemCenterOfMass;
    //public Pilote pilote;
    //public CentrePoussee CP;
    //private Rigidbody body;
    public Rigidbody gliderBody, piloteBody, centerOfMassBody;
    public ConfigurableJoint Pilote2SCOMJoint, SCOM2GliderJoint;
    public CentrePoussee CP;
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
    float initialPilote2GliderDistance;

    [Header( "Vectors 3d" )]
    public Vector3D v3dDrag;
    public Vector3D v3dSpeed;
    public Vector3D v3dPortance;
    public Vector3D v3dRFA;

    [Header( "UI" )]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI incidenceTxt;
    public TextMeshProUGUI roulisTxt;

    [Header("Simulate")]
    public bool Simulate;
    public Vector3 SimulatedSpeed;
    public float SimulatedIncidence = 8f;


    private void Awake()
    {
        startFixedDeltaTime = Time.fixedDeltaTime;

        //body = GetComponent<Rigidbody>();
        gliderBody.mass = AppManager.Instance.settings.GliderWeight;
        piloteBody.mass = AppManager.Instance.settings.PiloteWeight;
        initialPilote2GliderDistance = Vector3.Distance(piloteBody.position, gliderBody.position);
        //if (!Simulate)
        //{
        //    body.velocity = Quaternion.AngleAxis(incidence, transform.right) * transform.forward * InitialSpeed;
        //    pilote.body.velocity = body.velocity;
        //}
    }
    
    

    private void FixedUpdate() {
        Time.timeScale = AppManager.Instance.settings.slowMotionTimescale;
        Time.fixedDeltaTime = startFixedDeltaTime * AppManager.Instance.settings.slowMotionTimescale;

        if (Simulate)
        {
            Speed = SimulatedSpeed;
        }
        else
        {
            Speed = gliderBody.velocity;// GetPointVelocity(CP.transform.position );
        }
        //forces centrifuges
        //fcentrigue = mass*w*w*R
        //vitesse relative tangantielle par rapport au com
        //pilote
        Vector3 relativeSpeed = Vector3.ProjectOnPlane( piloteBody.velocity - centerOfMassBody.velocity, ( piloteBody.worldCenterOfMass - centerOfMassBody.position ).normalized );
        float forceCentrifuge = piloteBody.mass * relativeSpeed.magnitude * relativeSpeed.magnitude / Vector3.Distance( piloteBody.worldCenterOfMass, centerOfMassBody.position );
        Vector3 FCent = ( piloteBody.worldCenterOfMass - centerOfMassBody.position ).normalized * forceCentrifuge;
        //glider
        Vector3 relativeSpeedGlider = Vector3.ProjectOnPlane( gliderBody.velocity - centerOfMassBody.velocity, ( gliderBody.worldCenterOfMass - centerOfMassBody.position ).normalized );
        float forceCentrifugeG = gliderBody.mass * relativeSpeedGlider.magnitude * relativeSpeedGlider.magnitude / Vector3.Distance( gliderBody.worldCenterOfMass, centerOfMassBody.position );
        Vector3 FCentGlider = ( piloteBody.worldCenterOfMass - centerOfMassBody.position ).normalized * forceCentrifuge;


        //placement SCOM
        Vector3 piloteMassVector = piloteBody.mass * -Vector3.up;
        float piloteApparentMass =  Vector3.Project(piloteMassVector, gliderBody.position - piloteBody.position).magnitude + forceCentrifuge;

        Vector3 gliderMassVector = gliderBody.mass * -Vector3.up;
        float gliderApparentMass =  Vector3.Project( gliderMassVector, gliderBody.position - piloteBody.position ).magnitude + forceCentrifugeG;

        SCOM2GliderJoint.connectedAnchor = new Vector3(0, -initialPilote2GliderDistance * ( piloteApparentMass / ( gliderApparentMass + piloteApparentMass ) ), 0);
        Pilote2SCOMJoint.connectedAnchor = new Vector3(0, -initialPilote2GliderDistance * ( gliderApparentMass / ( gliderApparentMass + piloteApparentMass ) ), 0);


        //centerOfMassBody.MovePosition(( piloteBody.worldCenterOfMass + ( gliderApparentMass / ( gliderApparentMass + piloteApparentMass ) ) * ( gliderBody.position - piloteBody.worldCenterOfMass)));
        //body.centerOfMass = body.transform.InverseTransformDirection(body.position - systemCenterOfMass.body.position);


        ComputedCorde = CordeAttaque.position - CordeFuite.position;

        

        roulis = Vector3.SignedAngle(gliderBody.transform.up, Vector3.ProjectOnPlane(Vector3.up, gliderBody.transform.forward), gliderBody.transform.forward);
        incidence = Vector3.SignedAngle(ComputedCorde, Vector3.ProjectOnPlane(Speed, gliderBody.transform.right), gliderBody.transform.right);
        CP.UpdatePosition( incidence, roulis );
        float Cz = AppManager.Instance.settings.GliderCzI.Evaluate(incidence);
        float Cx = AppManager.Instance.settings.GliderCxI.Evaluate(incidence);

        float PortanceMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * Speed.magnitude * Speed.magnitude * Cz;
        float TraineeMag = 0.5f * AppManager.Instance.settings.AirDensity.Evaluate(AppManager.Instance.settings.AirTemperature) * AppManager.Instance.settings.GliderSurface * Speed.magnitude * Speed.magnitude * Cx;
        //float RFAMag = Mathf.Sqrt(PortanceMag * PortanceMag + TraineeMag * TraineeMag);


        Trainee = -Speed.normalized * TraineeMag;
        Portance = Vector3.Cross( Speed, gliderBody.transform.right ).normalized * PortanceMag;
        ComputedRFA = Portance + Trainee;// -pilote.ApparentMassVector.normalized * RFAMag;

       

        if (Simulate)
        {
            gliderBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            gliderBody.AddForceAtPosition(ComputedRFA, CP.transform.position, ForceMode.Force);

            //gliderBody.AddForceAtPosition(Vector3.Project(-ComputedCorde.normalized * RightBrake * Speed.magnitude, Trainee), RightBrakePoint.position, ForceMode.Force);
            //gliderBody.AddTorque(gliderBody.transform.up * RightBrake );

            //gliderBody.AddForceAtPosition(Vector3.Project(-ComputedCorde.normalized * LeftBrake * Speed.magnitude, Trainee), LeftBrakePoint.position, ForceMode.Force);
            //gliderBody.AddTorque(gliderBody.transform.up * -LeftBrake);


            //gliderBody.AddTorque(gliderBody.transform.up * roulis * 50f);

        }


        v3dDrag.values = Trainee;
        v3dSpeed.values = Speed;
        v3dPortance.values = Portance;
        v3dRFA.values = ComputedRFA;

        speedText.text = (Speed.magnitude * 3.6f).ToString() + " km/h";
        incidenceTxt.text = "Incidence : " + incidence.ToString();
        roulisTxt.text = "Roulis : " + roulis.ToString();
    }

}
