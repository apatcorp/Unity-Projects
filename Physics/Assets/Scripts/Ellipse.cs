using System;
using UnityEngine;

[Serializable]
public class Ellipse
{
    [SerializeField]
    public float minRadius = 1f;
    [SerializeField]
    public float maxRadius = 5f;
    [SerializeField]
    public float semi_major_axis;
    [SerializeField]
    public float semi_minor_axis;

    [Range(0f, 1f)]
    [SerializeField]
    public float eccentricity;

    [HideInInspector]
    [SerializeField]
    public float semi_latus_rectum;

    public float CalculateRadiusAtAngle(float angleInRadians)
    {
        return semi_latus_rectum / (1 + eccentricity * Mathf.Cos(angleInRadians));
    }

    public Vector2 EvaluateAngle(float angleInRadians)
    {
        float currentRadius = CalculateRadiusAtAngle(angleInRadians);
        return new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)) * currentRadius;
    }
}

[System.Serializable]
public class EllipticOrbit : Ellipse
{
    [SerializeField, HideInInspector]
    public float orbitalPeriod;

    float gravitationalParamter;

    float angularVelocity = 0f;

    public float currentRadius { get; set; }


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
