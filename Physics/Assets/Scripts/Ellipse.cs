using System;
using UnityEngine;

[Serializable]
public class Ellipse
{
    public float minRadius = 1f;
    public float maxRadius = 5f;

    public float semi_major_axis;
    public float semi_minor_axis;

    [Range(0f, 1f)]
    public float eccentricity;

    [HideInInspector]
    public float semi_latus_rectum;

    protected float CalculateRadiusAtAngle(float angleInRadians)
    {
        return semi_latus_rectum / (1 + eccentricity * Mathf.Cos(angleInRadians));
    }

    public Vector2 EvaluateAngle(float angleInRadians)
    {
        float currentRadius = CalculateRadiusAtAngle(angleInRadians);
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)) * currentRadius;
    }
}

public class EllipticOrbit : Ellipse
{
    float orbitalPeriod;
    float gravitationalParamter;

    float angularVelocity = 0f;
    float currentRadius = 0f;
  

    float CalculateAngularVelocity()
    {
        return (2f * Mathf.PI * semi_major_axis * semi_minor_axis) / (orbitalPeriod * (currentRadius * currentRadius));
    }

    Vector2 EvaluatePositionAtAngle(float angleInRadians)
    {
        currentRadius = CalculateRadiusAtAngle(angleInRadians);
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)) * currentRadius;
    }

    public Vector2 CalculateOrbitalPosition()
    {
        // calculate velocity
        angularVelocity += CalculateAngularVelocity() * Time.deltaTime;
        Vector2 position = EvaluatePositionAtAngle(angularVelocity);

        return position;
    }


    /*public void CalculateOrbitalPositionXZ(Transform orbitalObject)
    {
        // calculate velocity
        //angularVelocity += (2f * Mathf.PI * semi_major_axis * semi_minor_axis) / (orbitalPeriod * (radius * radius)) * Time.deltaTime;
        angularVelocity += CalculateAngularVelocity() * Time.deltaTime;
        position = EvaluatePositionAtAngle(angularVelocity);

        orbitalObject.localPosition = new Vector3(position.x, 0f, position.y);
    }*/
}
