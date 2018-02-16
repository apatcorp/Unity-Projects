using UnityEngine;

public class EllipticalMotion : MonoBehaviour
{
    [Header("Orbital Object Prefab")]
    public Transform orbitObjectPrefab;

    [Header("Ellipse")]
    public EllipticOrbit ellipse;

    [HideInInspector, SerializeField]
    public ElliptcalMotionData ellipseMotionData = new ElliptcalMotionData();

    public bool xy = false;
    public bool move = false;

    Transform orbitalObject;

    void Start ()
    {
        orbitalObject = orbitObjectPrefab != null ? Instantiate(orbitObjectPrefab) : GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        orbitalObject.SetParent(transform);
        
        ellipse.currentRadius = ellipse.CalculateRadiusAtAngle(0f);
	}


    void Update()
    {
        if (move)
        {
            Vector2 position = ellipse.CalculateOrbitalPosition();
            orbitalObject.localPosition = xy ? new Vector3(position.x, position.y, 0f) : new Vector3(position.x, 0f, position.y);
        }
    }
}
