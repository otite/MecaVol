using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SystemCenterOfMass : MonoBehaviour
{
    public Rigidbody piloteBody;
    public Glider glider;
    public Rigidbody SystemBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( piloteBody == null || glider == null )
        {
            return;
        }
        transform.position = ( piloteBody.centerOfMass* piloteBody.mass+glider.CenterOfMass.transform.position* SystemBody.mass)/(piloteBody.mass+ SystemBody.mass);
        SystemBody.centerOfMass = transform.position;
    }
}
