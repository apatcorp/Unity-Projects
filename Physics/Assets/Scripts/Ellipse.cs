using UnityEngine;

[System.Serializable]
public class Ellipse
{
    [Header("Min Radius From Centre (Perihelion)")]
    public float minRadius = 1f;
    [Header("Max Radius From Centre (Aphelion)")]
    public float maxRadius = 5f;
    [Header("Gravitational Parameter (G * M)")]
    public float gravitationalParameter = 1f;

    float semi_major_axis;
    float semi_minor_axis;
    float semi_latus_rectum;
    float eccentricity;
    public float orbitalPeriod { get; private set; }

    Vector2 position = Vector2.zero;
    float angularVelocity = 0f;
    float radius = 0f;

    float angularVelocityConstant = 0f;

    public Ellipse(float minRadius, float maxRadius, float gravitationalParameter)
    {
        this.minRadius = minRadius;
        this.maxRadius = maxRadius;
        this.gravitationalParameter = gravitationalParameter;

        semi_major_axis = CalculateSemiMajorAxis();
        semi_minor_axis = CalculateSemiMinorAxis();
        eccentricity = CalculateEccentricity();
        semi_latus_rectum = CalculateSemiLatusRectum();
        radius = CalculateRadius(0f);
        orbitalPeriod = CalculateOrbitalPeriod();

        angularVelocityConstant = (2f * Mathf.PI * semi_major_axis * semi_minor_axis);
    }

    float CalculateSemiMajorAxis ()
    {
        return (minRadius + maxRadius) / 2f;
    }

    float CalculateSemiMinorAxis()
    {
        return Mathf.Sqrt((minRadius * maxRadius));
    }

    float CalculateEccentricity()
    {
        return Mathf.Sqrt(1 - ((semi_minor_axis * semi_minor_axis) / (semi_major_axis * semi_major_axis)));
    }

    float CalculateSemiLatusRectum()
    {
        return (semi_minor_axis * semi_minor_axis) / semi_major_axis;
    }

    float CalculateRadius(float angleInRadians)
    {
        return semi_latus_rectum / (1 + eccentricity * Mathf.Cos(angleInRadians));
    }

    float CalculateOrbitalPeriod ()
    {
        return 2f * Mathf.PI * Mathf.Sqrt(Mathf.Pow(semi_major_axis, 3) / gravitationalParameter);
    }

    float CalculateAngularVelocity ()
    {
        return angularVelocityConstant / (orbitalPeriod * (radius * radius));
    }

    public Vector2 Evaluate(float t)
    {
        radius = CalculateRadius(t);
        return new Vector3(Mathf.Cos(t), Mathf.Sin(t)) * radius;
    }

    public void CalculateOrbitalPositionXY(Transform orbitalObject)
    {
        // calculate velocity
        //angularVelocity += (2f * Mathf.PI * semi_major_axis * semi_minor_axis) / (orbitalPeriod * (radius * radius)) * Time.deltaTime;
        angularVelocity += CalculateAngularVelocity() * Time.deltaTime;
        position = Evaluate(angularVelocity);

        orbitalObject.localPosition = new Vector3(position.x, position.y, 0f);
    }

    public void CalculateOrbitalPositionXZ(Transform orbitalObject)
    {
        // calculate velocity
        //angularVelocity += (2f * Mathf.PI * semi_major_axis * semi_minor_axis) / (orbitalPeriod * (radius * radius)) * Time.deltaTime;
        angularVelocity += CalculateAngularVelocity() * Time.deltaTime;
        position = Evaluate(angularVelocity);

        orbitalObject.localPosition = new Vector3(position.x, 0f, position.y);
    }
}
