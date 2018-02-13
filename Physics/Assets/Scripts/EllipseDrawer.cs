using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EllipseDrawer : MonoBehaviour
{
    [SerializeField]
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
        [SerializeField]
        public Material lineMaterial;
        [SerializeField]
        public Color lineColour = Color.white;
        [SerializeField]
        public AnimationCurve widthCurve;
        [SerializeField]
        public float widthMultiplier = 1f;

        [Header("Line Segments")]
        [Range(10, 500)]
        [SerializeField]
        public int segments = 100;

        [Header("Ellipse")]
        [SerializeField]
        public Ellipse ellipse;

        public void DrawEllipse (LineRenderer ellipseRenderer, Transform parent)
        {
            ellipseRenderer.material = lineMaterial;
            ellipseRenderer.material.color = lineColour;
            ellipseRenderer.widthCurve = widthCurve;
            ellipseRenderer.widthMultiplier = widthMultiplier;
            ellipseRenderer.loop = true;
            ellipseRenderer.useWorldSpace = false;

            Vector3[] points = new Vector3[segments];

            for (int i = 0; i < segments; i++)
            {
                float angleInRadians = ((float)i / (float)segments) * 360f * Mathf.Deg2Rad;
                Vector2 position = ellipse.EvaluateAngle(angleInRadians);
                points[i] = new Vector3(position.x, position.y, 0f);
            }

            GameObject focalPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            focalPoint.transform.SetParent(parent);
            focalPoint.transform.localScale = Vector3.one * 0.1f * ellipse.semi_minor_axis;
            focalPoint.transform.position = Vector3.zero;

            ellipseRenderer.positionCount = segments;
            ellipseRenderer.SetPositions(points);
        }
    }
}
