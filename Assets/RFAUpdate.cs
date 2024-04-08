using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFAUpdate : MonoBehaviour
{
    public Rigidbody piloteBody;
    public Transform CP;
    private Rigidbody rb;
    public AnimationCurve CzI;
    public float speedNorm;
    public Transform CordeFuite, CordeAttaque;

    private Vector3 ComputedRFA;
    private Vector3 ComputedCorde;
    [Header( "Vectors 3d" )]
    public Vector3D v3dDrag;
    public Vector3D v3dSpeed;
    public Vector3D v3dPortance;
    public Vector3D v3dRFA;
    //tmp
    public float incidence = 6f;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        ComputedCorde = CordeAttaque.position - CordeFuite.position;
    }

    private void Update() {
        Vector3 speed = (Quaternion.AngleAxis( incidence, transform.right ) * ComputedCorde).normalized * speedNorm;
        Vector3 trainee = -speed * CzI.Evaluate( incidence );
        Vector3 portance = Vector3.Cross(speed, transform.right);
        ComputedRFA = (piloteBody.transform.up).normalized * -1f;//portance + trainee;

        v3dDrag.values = trainee;
        v3dSpeed.values = speed;
        v3dPortance.values = portance;
        v3dRFA.values = ComputedRFA;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CP.position = rb.centerOfMass + new Vector3 (0, 6f, 0);
        //rb.AddForceAtPosition( ComputedRFA, CP.position );
        
    }
}
