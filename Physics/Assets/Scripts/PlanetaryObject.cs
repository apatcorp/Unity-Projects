using UnityEngine;

[System.Serializable]
public class PlanetaryObject
{
    public float m { get; private set; }
    public float M { get; private set; }
    public float orbitalSpeed_approximate { get; private set; }
    public float orbitalSpeed_precise { get; private set; }
    public float radiusToCentre { get; private set; }
    float semi_minor_axis;
    float semi_latus_rectum;
    float eccentricity;

    float gravitationalParamter;

    public PlanetaryObject(int index, float massMulitplier, float radiusMuliplier, float orbitalPeriodMultiplier)
    {
        m = (float)(PlanetaryObjectData.masses[index] / PlanetaryObjectData.masses[(int)Name.EARTH]) * massMulitplier;
        M = CalculateCombinedMass(index, massMulitplier);

        eccentricity = PlanetaryObjectData.eccentricities[index];
        radiusToCentre = (float)(PlanetaryObjectData.radiiToCentre[index] / PlanetaryObjectData.radiiToCentre[(int)Name.EARTH]) * radiusMuliplier;
        semi_minor_axis = radiusToCentre * Mathf.Sqrt((1 - (eccentricity * eccentricity)));
        semi_latus_rectum = radiusToCentre > 0 ? (semi_minor_axis * semi_minor_axis) / radiusToCentre : 0f; 

        orbitalSpeed_approximate = radiusToCentre > 0 ? Mathf.Sqrt((SolarSystem.G * M / radiusToCentre)) : 0f;

        gravitationalParamter = SolarSystem.G * M;
    }

    float CalculateCombinedMass (int index, float massMultiplier)
    {
        float combinedMass = 0f;

        for (int i = index; i > 0; i--)
        {
            combinedMass += (float)PlanetaryObjectData.masses[i - 1];
        }

        return combinedMass / (float)PlanetaryObjectData.masses[(int)Name.EARTH] * massMultiplier;
    }

    public float CalculateOrbitalSpeed_Precise(Vector3 position)
    {
        float angleInRadians = Vector3.Angle(Vector3.right, position.normalized) * Mathf.Deg2Rad;
        float radius = semi_latus_rectum / (1 + eccentricity * Mathf.Cos(angleInRadians));

        orbitalSpeed_precise = (radius > 0 && radiusToCentre > 0) ? Mathf.Sqrt(gravitationalParamter * ((2f / radius) - (1f / radiusToCentre))) : 0f;

        return orbitalSpeed_precise;
    }
}

public static class PlanetaryObjectData
{
    const float conversionDaysToSeconds = 24 * 60 * 60;

    public static readonly double[] masses = {
        1.989e+30,
        3.30104e+23,
        4.867e+24,
        5.972e+24,
        6.39e+23,
        1.89813e+27,
        5.68319e+26,
        8.68103e+25,
        1.0241e+26
    };

    public static readonly double[] radiiToCentre = {
        0,
        5.791e+10,
        1.082e+11,
        1.496e+11,
        2.279e+11,
        7.785e+11,
        1.429e+12,
        2.871e+12,
        4.498e+12
    };

    public static readonly float[] eccentricities = {
        0,
        0.205630f,
        0.006772f,
        0.0167086f,
        0.0934f,
        0.0489f,
        0.0565f,
        0.046381f,
        0.009456f
    };

    public static readonly double[] orbitalPeriods = {
        0,
        87.9691 * conversionDaysToSeconds,
        224.701 * conversionDaysToSeconds,
        365.256363004 * conversionDaysToSeconds,
        686.971 * conversionDaysToSeconds,
        4332.59 * conversionDaysToSeconds,
        10759.22 * conversionDaysToSeconds,
        30688.5 * conversionDaysToSeconds,
        60182 * conversionDaysToSeconds
     };
}
