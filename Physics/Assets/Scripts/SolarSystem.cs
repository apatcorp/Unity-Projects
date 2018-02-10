using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public static float G = 0.01f;
    const int planetaryObjectsCount = 9;

    [Header("Relative To Earth")]
    [Range(1, 100)]
    public float massMultiplier = 1f;
    [Range(1, 100)]
    public float radiusMultiplier = 1f;

    PlanetaryObject[] planetaryObjects;
   
	
	void Start ()
    {
        SetupSolarSystem();
	}
	
    void SetupSolarSystem ()
    {
        double massOfEarth = PlanetaryObjectData.masses[(int)Name.EARTH];
        double radiusSunEarth = PlanetaryObjectData.radiiToCentre[(int)Name.EARTH];

        planetaryObjects = new PlanetaryObject[planetaryObjectsCount];

        // set the sum as first element
        planetaryObjects[0] = new PlanetaryObject((float)(PlanetaryObjectData.masses[0] / massOfEarth) * massMultiplier, (float)(PlanetaryObjectData.radiiToCentre[0] / radiusSunEarth) * radiusMultiplier);
        SetupPlanetaryObject(0, planetaryObjects[0]);

        // variable with stores the combined masses until the ith object
        float combinedMass = 0f;

        for (int i = 1; i < planetaryObjectsCount; i++)
        {
            combinedMass += planetaryObjects[i - 1].mass;

            PlanetaryObject planetaryobject = new PlanetaryObject((float)(PlanetaryObjectData.masses[i] / massOfEarth) * massMultiplier, (float)(PlanetaryObjectData.radiiToCentre[i] / radiusSunEarth) * radiusMultiplier);
            planetaryobject.CalculateOrbitalSpeed(combinedMass);
            planetaryObjects[i] = planetaryobject;

            // instantiate planetary object
            SetupPlanetaryObject(i, planetaryobject);
        }
    }

    void SetupPlanetaryObject(int index, PlanetaryObject planetaryObject)
    {
        OrbitalObject orbitalObject = transform.GetChild(index).GetComponent<OrbitalObject>();
        orbitalObject.SetupOrbitalObject(planetaryObject);
    }
}

public enum Name { SUN, MERCURY, VENUS, EARTH, MARS, JUPITER, SATURN, URANUS, NEPTUNE }
