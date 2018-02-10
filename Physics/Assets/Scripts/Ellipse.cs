using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Ellipse : MonoBehaviour
{
    [Range(0, 100f)]
    public float minRadius;
    [Range(0, 100f)]
    public float maxRadius;
    [Range(1, 100f)]
    public float constant;

    public bool toggle = true;

    public Transform orbitObjectPrefab;
    Transform orbitalObject;

    float semi_major_axis;
    float semi_minor_axis;
    float semi_latus_rectum;

    float eccentricity;
    float distanceFocalPointsToCentre;

    int segments = 100;

    float anglularVelocity = 0f;
    float radius = 0f;

    LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start ()
    {
        semi_minor_axis = GetSemiMinorAxis(minRadius, maxRadius);
        semi_major_axis = GetSemiMajorAxis(minRadius, maxRadius);
        eccentricity = GetEccentricity(semi_major_axis, semi_minor_axis);
        semi_latus_rectum = GetSemiLatusRectum(semi_major_axis, semi_minor_axis);
        distanceFocalPointsToCentre = GetDistanceFocalPointsToCentre(semi_major_axis, eccentricity);
        radius = GetRadius(0);

        print("a: " + semi_major_axis);
        print("b: " + semi_minor_axis);
        print("e: " + eccentricity);
        print("c: " + distanceFocalPointsToCentre);

        DrawEllipse();
	}

    void OnValidate()
    {
        if (maxRadius < minRadius)
        {
            minRadius = maxRadius;
        }

        if (minRadius > maxRadius)
        {
            maxRadius = minRadius;
        }

        if (constant < 1)
            constant = 1;
    }

    void DrawEllipse ()
    {
        Vector3[] points = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angleInRadians = ((float)i / (float)segments) * 360f * Mathf.Deg2Rad;

            points[i] = Evaluate_O(angleInRadians);
        }

        lineRenderer.positionCount = segments;
        lineRenderer.SetPositions(points);

        GameObject f1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject f2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        f1.transform.localScale = Vector3.one * .5f;
        f2.transform.localScale = Vector3.one * .5f;

        f1.transform.position = Vector3.right * distanceFocalPointsToCentre;
        f2.transform.position = Vector3.left * distanceFocalPointsToCentre;

        orbitalObject = Instantiate(orbitObjectPrefab);

        //SetOrbitObjectPosition();
    }

    Vector2 Evaluate_N (float t)
    {
        return new Vector3(semi_major_axis * Mathf.Cos(t), semi_minor_axis * Mathf.Sin(t));
    }

    Vector2 Evaluate_O(float t)
    {
        radius = GetRadius(t);
        return new Vector3(Mathf.Cos(t), Mathf.Sin(t)) * radius;
    }

    static float GetSemiMajorAxis (float minRadius, float maxRadius)
    {
        return (minRadius + maxRadius) / 2f;
    }

    static float GetSemiMinorAxis(float minRadius, float maxRadius)
    {
        return Mathf.Sqrt((minRadius * maxRadius));
    }

    static float GetSemiLatusRectum(float semi_major_axis, float semi_minor_axis)
    {
        return (semi_minor_axis * semi_minor_axis) / semi_major_axis;
    }

    static float GetEccentricity(float semi_major_axis, float semi_minor_axis)
    {
        return Mathf.Sqrt(1 - ((semi_minor_axis * semi_minor_axis) / (semi_major_axis * semi_major_axis)));
    }

    static float GetDistanceFocalPointsToCentre (float semi_major_axis, float eccentricity)
    {
        return semi_major_axis * eccentricity;
    }

    float GetRadius(float angleInRadians)
    {
        return semi_latus_rectum / (1 + eccentricity * Mathf.Cos(angleInRadians));
    }

    void SetEllipsePosition ()
    {
        float radius = orbitalObject.position.magnitude;
        anglularVelocity += Mathf.Sqrt(constant * ((2f / radius) - (1f / semi_major_axis))) * Time.deltaTime;
        Vector2 position = Evaluate_N(anglularVelocity);

        orbitalObject.localPosition = new Vector3(position.x, position.y, 0f);
    }

    void SetOrbit_1 ()
    {
        // calculate new position
        float angle = constant * Time.time * Mathf.Deg2Rad;
        Vector2 position = Evaluate_O(angle);

        orbitalObject.localPosition = new Vector3(position.x, position.y, 0f);
    }

    void SetOrbit_2()
    {
        // calculate velocity
        radius = GetRadius(Time.time * Mathf.Deg2Rad);
        anglularVelocity += Mathf.Sqrt(constant * ((2f / radius) - (1f / semi_major_axis))) * Time.deltaTime;
        Vector2 position = Evaluate_O(anglularVelocity);

        orbitalObject.localPosition = new Vector3(position.x, position.y, 0f);
    }

    void Update()
    {
        if (toggle)
            SetOrbit_1();
        else
            SetOrbit_2();
    }
}
