using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeplerOrbitalMotion : MonoBehaviour
{
    const int planetCount = 9;

    [SerializeField]
    public List<EllipseDrawer.EllipseConfiguration> ellipseConfigurations;

    [SerializeField]
    public int massMultiplier;
    [SerializeField]
    public int radiusMultiplier;

    void Start ()
    {
        CreateSolarSystem();
	}

    void CreateSolarSystem ()
    {
        for (int i = 0; i < planetCount; i++)
        {
            CreatePlanet(i);
        }
    }
	
    void CreatePlanet (int index)
    {
        GameObject planet = new GameObject(System.Enum.GetName(typeof(Name), index));
        planet.transform.SetParent(transform);
        planet.transform.position = Vector3.zero;
        CreateOrbit(index, planet);
    }

    void CreateOrbit(int index, GameObject planet)
    {    
        // setup orbiting object
        EllipticalMotion ellipseMotion = planet.AddComponent<EllipticalMotion>();
        ellipseMotion.ellipse = new EllipticOrbit();
        ellipseMotion.ellipse.semi_major_axis = (float)(PlanetaryObjectData.semi_major_axes[index] / PlanetaryObjectData.semi_major_axes[(int)Name.EARTH]) * radiusMultiplier;
        ellipseMotion.ellipse.eccentricity = PlanetaryObjectData.eccentricities[index];
        ellipseMotion.ellipse.semi_minor_axis = EllipseCalculation.Semi_Minor_Axis_2(ellipseMotion.ellipse.semi_major_axis, ellipseMotion.ellipse.eccentricity);
        ellipseMotion.ellipse.maxRadius = EllipseCalculation.Max_Radius_1(ellipseMotion.ellipse.semi_major_axis, ellipseMotion.ellipse.eccentricity);
        ellipseMotion.ellipse.minRadius = EllipseCalculation.Min_Radius_1(ellipseMotion.ellipse.semi_major_axis, ellipseMotion.ellipse.eccentricity);
        ellipseMotion.ellipse.semi_latus_rectum = ellipseMotion.ellipse.semi_major_axis > 0f ? EllipseCalculation.Semi_Latus_Rectum(ellipseMotion.ellipse.semi_major_axis, ellipseMotion.ellipse.semi_minor_axis) : 0f;
        float m = (float)(PlanetaryObjectData.masses[index] / PlanetaryObjectData.masses[(int)Name.EARTH]) * massMultiplier;
        float M = 0f;

        for (int i = 1; i <= index; i++)
        {
            M += (float)(PlanetaryObjectData.masses[i - 1] / PlanetaryObjectData.masses[(int)Name.EARTH]) * massMultiplier;
        }

        ellipseMotion.ellipse.orbitalPeriod = EllipseCalculation.Orbiting.CalculateOrbitalPeriod(ellipseMotion.ellipse.semi_major_axis, SolarSystem.G, m, M);
        ellipseMotion.ellipse.currentRadius = EllipseCalculation.Orbiting.CalculateRadiusAtAngle(0f, ellipseMotion.ellipse.semi_latus_rectum, ellipseMotion.ellipse.eccentricity);

        // draw orbit
        LineRenderer lineRenderer = planet.AddComponent<LineRenderer>();
        lineRenderer.material = ellipseConfigurations[index].lineMaterial;
        lineRenderer.material.color = ellipseConfigurations[index].lineColour;
        lineRenderer.widthCurve = ellipseConfigurations[index].widthCurve;
        lineRenderer.widthMultiplier = ellipseConfigurations[index].widthMultiplier;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;

        if (ellipseMotion.ellipse.semi_major_axis > 0f)
        {
            int segments = ellipseConfigurations[index].segments;

            Vector3[] points = new Vector3[segments];

            for (int i = 0; i < segments; i++)
            {
                float angleInRadians = ((float)i / (float)segments) * 360f * Mathf.Deg2Rad;
                Vector2 position = ellipseMotion.ellipse.EvaluateAngle(angleInRadians);
                points[i] = new Vector3(position.x, 0f, position.y);
            }

            lineRenderer.positionCount = segments;
            lineRenderer.SetPositions(points);

            ellipseMotion.xy = false;
            ellipseMotion.move = true;
        }
    }
}
