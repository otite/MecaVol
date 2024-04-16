using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glider : MonoBehaviour
{
    public Rigidbody body, SCOM;
    public Vector3D FCentrifuge;
    private void FixedUpdate()
    {
        Vector3 speed = body.velocity;
        //fcent aile
        //vitesse relative tangantielle par rapport au com
        Vector3 relativeSpeed = Vector3.ProjectOnPlane(speed - SCOM.velocity, (body.worldCenterOfMass - SCOM.position).normalized);

        //vitesse angulaire w = vitesse car distance fixe entre cg systeme et pilote.
        //fcentrigue = mass*w*w*R
        float forceCentrifuge = body.mass * relativeSpeed.magnitude * relativeSpeed.magnitude / Vector3.Distance(body.worldCenterOfMass, SCOM.position);
        Vector3 FCent = (body.worldCenterOfMass - SCOM.position).normalized * forceCentrifuge;
        FCentrifuge.values = FCent;
    }
}
