using UnityEngine;

public class ColorPropertySetter : MonoBehaviour
{
    //The color of the object
    public Color MaterialColor;

    //The material property block we pass to the GPU
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        OnValidate();
    }
    // OnValidate is called in the editor after the component is edited
    void OnValidate() {
        //create propertyblock only if none exists
        if( propertyBlock == null )
            propertyBlock = new MaterialPropertyBlock();
        //Get a renderer component either of the own gameobject or of a child
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        //set the color property
        propertyBlock.SetColor( "_Color", MaterialColor );
        //apply propertyBlock to renderer
        foreach( Renderer renderer in renderers )
        renderer.SetPropertyBlock( propertyBlock );
    }
}