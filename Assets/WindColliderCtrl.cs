using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class WindColliderCtrl : MonoBehaviour
{
    public VisualEffect visualEffect;
    VFXEventAttribute eventAttribute;

    //static readonly ExposedProperty TLR = "TLR";

    // Start is called before the first frame update
    void Start()
    {
        // Caches an Event Attribute matching the
        //eventAttribute = visualEffect.CreateVFXEventAttribute();
    }

    // Update is called once per frame
    void Update()
    {
        //eventAttribute.SetVector3( TLR, transform.localEulerAngles );
    }
}
