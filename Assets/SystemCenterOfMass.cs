using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCenterOfMass : MonoBehaviour
{
    public Pilote pilote;
    public Glider glider;
    public Rigidbody SystemBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( pilote == null || glider == null )
        {
            return;
        }
        transform.position = ( pilote.CenterOfMass.position * pilote.ComputedMass + glider.CenterOfMass.transform.position* SystemBody.mass)/(pilote.ComputedMass + SystemBody.mass);
        SystemBody.centerOfMass = SystemBody.transform.InverseTransformPoint(transform.position);
    }
}
