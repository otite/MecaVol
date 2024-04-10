using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentrePoussee : MonoBehaviour
{
    public AnimationCurve ZfI;

    private Vector3 initialLocalPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePosition(float incidence ) {
        transform.localPosition = initialLocalPosition + new Vector3(0,ZfI.Evaluate(incidence),0);
    }
}
