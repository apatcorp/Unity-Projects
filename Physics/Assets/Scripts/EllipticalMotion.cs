using UnityEngine;

public class EllipticalMotion : MonoBehaviour
{
    [Header("Orbital Object Prefab")]
    public Transform orbitObjectPrefab;

    [Header("Ellipse")]
    public EllipticOrbit ellipse;

    Transform orbitalObject;


    void Start ()
    {
        orbitalObject = orbitObjectPrefab != null ? Instantiate(orbitObjectPrefab) : GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        orbitalObject.SetParent(transform);

        ellipse.orbitalPeriod = EllipseCalculation.Orbiting.CalculateOrbitalPeriod(ellipse.semi_major_axis, SolarSystem.G, 5, 10);
        ellipse.semi_minor_axis = EllipseCalculation.Semi_Minor_Axis_2(ellipse.semi_major_axis, ellipse.eccentricity);
        ellipse.semi_latus_rectum = EllipseCalculation.Semi_Latus_Rectum(ellipse.semi_major_axis, ellipse.semi_minor_axis);
        ellipse.currentRadius = ellipse.CalculateRadiusAtAngle(0f);
	}

    void Update()
    {
        Vector2 position = ellipse.CalculateOrbitalPosition();
        orbitalObject.localPosition = new Vector3(position.x, position.y, 0f);
    }
}
