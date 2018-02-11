using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseDrawer : MonoBehaviour
{
    [Header("Line Renderer Configs")]
    public Material lineMaterial;
    public Color lineColour = Color.white;
    public AnimationCurve widthCurve;
    public float widthMultiplier = 1f;

    [Header("Line Segments")]
    [Range(10, 500)]
    public int segments = 100;

    [Header("Ellipse")]
    public Ellipse ellipse;

    LineRenderer lineRenderer;


    void OnValidate()
    {
        if (ellipse != null)
        {
            ellipse.minRadius = ellipse.minRadius < 0 ? 0 : ellipse.minRadius;
            ellipse.maxRadius = ellipse.maxRadius < 0 ? 0 : ellipse.maxRadius;
            widthMultiplier = widthMultiplier < 0 ? Mathf.Epsilon : widthMultiplier;

            ellipse.minRadius = ellipse.minRadius > ellipse.maxRadius ? ellipse.maxRadius : ellipse.minRadius;
            ellipse.maxRadius = ellipse.maxRadius < ellipse.minRadius ? ellipse.minRadius : ellipse.maxRadius;

            ellipse.gravitationalParameter = ellipse.gravitationalParameter < 0 ? Mathf.Epsilon : ellipse.gravitationalParameter;
        }
    }

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Standard"));
        lineRenderer.material.color = lineColour;
        lineRenderer.widthCurve = widthCurve;
        lineRenderer.widthMultiplier = widthMultiplier;
        lineRenderer.loop = true;
    }


    void Start ()
    {
        ellipse = new Ellipse(ellipse.minRadius, ellipse.maxRadius, ellipse.gravitationalParameter);
        DrawEllipse();
	}
	
    void DrawEllipse ()
    {
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angleInRadians = ((float)i / (float)segments) * 360f * Mathf.Deg2Rad;
            points[i] = ellipse.Evaluate(angleInRadians);
        }

        GameObject focalPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        focalPoint.transform.SetParent(transform);
        focalPoint.transform.localScale = Vector3.one * 0.8f;
        focalPoint.transform.position = Vector3.zero;

        lineRenderer.positionCount = segments;
        lineRenderer.SetPositions(points);
    }
}
