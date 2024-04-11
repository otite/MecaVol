using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3D : MonoBehaviour
{
    public float magnitude;
    //public Color color = Color.white;
    public Vector3 values;
    public GameObject EndModel;
    public Material material;
    LineRenderer _lr;


    // Start is called before the first frame update
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.material = material;
    }

    // Update is called once per frame
    void Update()
    {
        if( _lr == null ) return;
        //if( material != null ) {
        //    material.SetColor( "_Color", color ) ;
        //}
        
            _lr.SetPosition( 0, transform.position );
            _lr.SetPosition( 1, transform.position + values*AppManager.Instance.settings.Vectors3DScale );
            EndModel.transform.position = transform.position + values * AppManager.Instance.settings.Vectors3DScale;
            EndModel.transform.up = values * AppManager.Instance.settings.Vectors3DScale;
            magnitude = values.magnitude;


    }
}
