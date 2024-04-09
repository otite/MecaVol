using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MecaVol/AppSettings", order = 1)]
public class AppSettings : ScriptableObject
{
    [Header("Glider")]
    public float GliderWeight = 6f;
    public Vector2 GliderWeightRange = new Vector2(90, 110);
    public float GliderSurface = 28f;
    public float GliderFinesse = 8f;
    public AnimationCurve GliderCzI;
    public AnimationCurve GliderCxCz;
    //public AnimationCurve GliderSpeedI;

    [Header("Pilote")]
    public float PiloteWeight = 100f;

    [Header("Environment")]
    public float AirTemperature;
    public AnimationCurve AirDensity;

    [Header("Various")]
    public float Vectors3DScale = 0.05f;
    public float slowMotionTimescale = 0.05f;
}