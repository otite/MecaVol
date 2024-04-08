using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Vector3D : MonoBehaviour
{
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
        _lr.SetPosition( 1, transform.position + values );
        EndModel.transform.position = transform.position+values;
        EndModel.transform.up = values;
    }
}
