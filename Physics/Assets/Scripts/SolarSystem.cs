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
    [Range(1, 100)]
    public float orbitalPeriodMultiplier = 1f;

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

        for (int i = 0; i < planetaryObjectsCount; i++)
        {
            PlanetaryObject planetaryobject = new PlanetaryObject(i, massMultiplier, radiusMultiplier, orbitalPeriodMultiplier);
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
