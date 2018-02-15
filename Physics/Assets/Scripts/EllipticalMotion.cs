using UnityEngine;

public class EllipticalMotion : MonoBehaviour
{
    [Header("Orbital Object Prefab")]
    public Transform orbitObjectPrefab;

    [Header("Ellipse")]
    public EllipticOrbit ellipse;

    [HideInInspector, SerializeField]
    public ElliptcalMotionData ellipseMotionData = new ElliptcalMotionData();

    Transform orbitalObject;


    void Start ()
    {
        orbitalObject = orbitObjectPrefab != null ? Instantiate(orbitObjectPrefab) : GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        orbitalObject.SetParent(transform);

        print(ellipse.maxRadius);
        print(ellipse.minRadius);
        print(ellipse.semi_major_axis);
        print(ellipse.semi_minor_axis);
        print(ellipse.semi_latus_rectum);
        print(ellipse.eccentricity);
        print(ellipse.orbitalPeriod);

        ellipse.currentRadius = ellipse.CalculateRadiusAtAngle(0f);
	}

    void Update()
    {
        Vector2 position = ellipse.CalculateOrbitalPosition();
        orbitalObject.localPosition = new Vector3(position.x, position.y, 0f);
    }
}
