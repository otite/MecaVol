using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilote : MonoBehaviour
{
    public float Weight = 100f;
    public Transform CenterOfMass;
    [Range( -1f, 1f )]
    public float MassControl = 0f;
    private float ApparentWeight = 100f;
    private Rigidbody rigidBody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = Weight;
        GetComponent<FixedJoint>().massScale = Weight;
    }

    // Update is called once per frame
    void Update()
    {
        CenterOfMass.localPosition = new Vector3(MassControl, 0f, 0f);
        rigidBody.centerOfMass = CenterOfMass.position;
        ApparentWeight = Weight; //todo force centirfuge/pete
    }
}
