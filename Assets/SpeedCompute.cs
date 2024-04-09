using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCompute : MonoBehaviour
{
    public Vector3D v3d;
    Vector3 previousPosition;
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        v3d.values = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
    }
}
