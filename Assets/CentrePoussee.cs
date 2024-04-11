using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentrePoussee : MonoBehaviour
{
    public AnimationCurve ZfI;
    public AnimationCurve XfI;

    public float _roulis;

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
    public void UpdatePosition(float incidence, float roulis ) {
        _roulis = roulis;
        transform.localPosition = initialLocalPosition + new Vector3( XfI.Evaluate( roulis ), ZfI.Evaluate(incidence),0);
    }

    
}
