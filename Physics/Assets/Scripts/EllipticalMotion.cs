using UnityEngine;

public class EllipticalMotion : MonoBehaviour
{
    [Header("Orbital Object Prefab")]
    public Transform orbitObjectPrefab;

    [Header("Ellipse")]
    public Ellipse ellipse;

    Transform orbitalObject;

    /*void OnValidate()
    {
        if (ellipse != null)
        {
            ellipse.minRadius = ellipse.minRadius < 0 ? 0 : ellipse.minRadius;
            ellipse.maxRadius = ellipse.maxRadius < 0 ? 0 : ellipse.maxRadius;

            ellipse.minRadius = ellipse.minRadius > ellipse.maxRadius ? ellipse.maxRadius : ellipse.minRadius;
            ellipse.maxRadius = ellipse.maxRadius < ellipse.minRadius ? ellipse.minRadius : ellipse.maxRadius;

            ellipse.gravitationalParameter = ellipse.gravitationalParameter < 0 ? Mathf.Epsilon : ellipse.gravitationalParameter;
        }
    }

    void Start ()
    {
        orbitalObject = orbitObjectPrefab != null ? Instantiate(orbitObjectPrefab) : GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        ellipse = new Ellipse(ellipse.minRadius, ellipse.maxRadius, ellipse.gravitationalParameter);
        print("Orbital Period: " + ellipse.orbitalPeriod + " sec");
	}

    void Update()
    {
        ellipse.CalculateOrbitalPositionXY(orbitalObject);
    }*/
}
