using UnityEngine;
using System.Collections.Generic;

public class EllipseDrawer : MonoBehaviour
{
    public List<EllipseConfiguration> ellipses;

    void Start ()
    {
        DrawEllipses();
	}
	
    public void DrawEllipses ()
    {
        foreach (EllipseConfiguration ellipse in ellipses)
        {
            GameObject ellipseObject = new GameObject("Ellipse");
            ellipseObject.transform.SetParent(transform);
            LineRenderer ellipseRenderer = ellipseObject.AddComponent<LineRenderer>();
            ellipse.DrawEllipse(ellipseRenderer, ellipseObject.transform);
        }
    }

    [System.Serializable]
    public class EllipseConfiguration
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

        public void DrawEllipse (LineRenderer ellipseRenderer, Transform parent)
        {
            ellipseRenderer.material = lineMaterial;
            ellipseRenderer.material.color = lineColour;
            ellipseRenderer.widthCurve = widthCurve;
            ellipseRenderer.widthMultiplier = widthMultiplier;
            ellipseRenderer.loop = true;

            print(ellipse.minRadius);
            print(ellipse.maxRadius);
            print(ellipse.semi_major_axis);
            print(ellipse.semi_minor_axis);
            print(ellipse.eccentricity);
            print(ellipse.semi_latus_rectum);

            Vector3[] points = new Vector3[segments];

            for (int i = 0; i < segments; i++)
            {
                float angleInRadians = ((float)i / (float)segments) * 360f * Mathf.Deg2Rad;
                Vector2 position = ellipse.EvaluateAngle(angleInRadians);
                points[i] = new Vector3(position.x, position.y, 0f);
            }

            GameObject focalPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            focalPoint.transform.SetParent(parent);
            focalPoint.transform.localScale = Vector3.one * 0.8f;
            focalPoint.transform.position = Vector3.zero;

            ellipseRenderer.positionCount = segments;
            ellipseRenderer.SetPositions(points);
        }
    }
}
