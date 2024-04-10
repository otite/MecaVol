using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCenterOfMass : MonoBehaviour
{
    public Pilote pilote;
    public Glider glider;
    public Rigidbody SystemBody;
    public Vector3 speed;

    private Vector3 oldPosition;

    private void Start() {
        oldPosition = transform.position;
    }

    // Start is called before the first frame update
    void Update()
    {
        if( pilote == null || glider == null ) {
            return;
        }
        transform.position = pilote.CenterOfMass.position + ( glider.body.mass / ( glider.body.mass + pilote.body.mass ) ) * ( glider.CenterOfMass.position - pilote.CenterOfMass.position );// ( pilote.CenterOfMass.position * pilote.ComputedMass + glider.CenterOfMass.transform.position * SystemBody.mass)/(pilote.ComputedMass + SystemBody.mass);
        SystemBody.centerOfMass = SystemBody.transform.InverseTransformDirection( transform.position - SystemBody.position);
        Debug.DrawLine( SystemBody.position, SystemBody.worldCenterOfMass, Color.green );

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( pilote == null || glider == null )
        {
            return;
        }
        speed = (transform.position - oldPosition)/Time.deltaTime;
        oldPosition = transform.position;
    }
}
