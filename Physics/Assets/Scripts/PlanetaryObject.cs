using UnityEngine;

[System.Serializable]
public class PlanetaryObject
{
    public float mass { get; private set; }
    public float orbitalSpeed { get; private set; }
    public float radiusToCentre { get; private set; }

    public PlanetaryObject(float mass, float radiusToCentre)
    {
        this.mass = mass;
        this.radiusToCentre = radiusToCentre;
    }

    public void CalculateOrbitalSpeed(float M)
    {
        if (radiusToCentre > 0)
            orbitalSpeed = Mathf.Sqrt((SolarSystem.G * M / radiusToCentre));
        else
            orbitalSpeed = 0f;
    }

    public void CalculateRadiusToCentre(float M, float velocity)
    {
        if (velocity > 0)
            radiusToCentre = SolarSystem.G * M / (velocity * velocity);
        else
            radiusToCentre = 0f;
    }
}

public static class PlanetaryObjectData
{
    public static readonly double[] masses = { 1.989e+30, 3.30104e+23, 4.867e+24, 5.972e+24, 6.39e+23, 1.89813e+27, 5.68319e+26, 8.68103e+25, 1.0241e+26 };
    public static readonly double[] orbitalSpeeds = { 0, 47362, 35020, 29780, 24007, 13070, 9690, 6810, 5430 };
    public static readonly double[] radiiToCentre = { 0, 5.791e+10, 1.082e+11, 1.496e+11, 2.279e+11, 7.785e+11, 1.429e+12, 2.871e+12, 4.498e+12 };
}
