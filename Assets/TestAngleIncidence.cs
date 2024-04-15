using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngleIncidence : MonoBehaviour
{
    public Transform speed, corde;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log( Vector3.SignedAngle(corde.forward, speed.forward, transform.right));

    }
}
