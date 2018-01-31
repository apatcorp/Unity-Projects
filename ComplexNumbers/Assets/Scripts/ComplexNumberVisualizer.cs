using UnityEngine;
using System.Collections.Generic;

public class ComplexNumberVisualizer : MonoBehaviour
{
    public GameObject complexPlanePrefab;

    [Range(1, 5)]
    public int scaleFactor = 1;

    GameObject centreComplexPlane;
    ComplexPlane centreComplexPlaneComponent;
    ComplexPlane currentComplexPlane;

    Dictionary<Vector3, ComplexPlane> dictionary = new Dictionary<Vector3, ComplexPlane>();

#region Singleton
    private static ComplexNumberVisualizer singleton = null;
    public static ComplexNumberVisualizer Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<ComplexNumberVisualizer>();
            }
            if (singleton == null)
            {
                Debug.LogError("No instance of ComplexNumberVisualizier found in scene");
            }
            return singleton;
        }
        private set
        {
            if (singleton != null)
            {
                DestroyImmediate(singleton.gameObject);             
            }
            singleton = value;
        }
    }

#endregion

    private void Awake()
    {
        if (singleton == null)
            Singleton = this;

        DontDestroyOnLoad(this);
    }

    void Start ()
    {     
        CreateNewComplexPlanePart(Utility.PlaneType.CENTRE, Vector3.zero);
    }

    void CreateNewComplexPlanePart(Utility.PlaneType planeType, Vector3 position)
    {
        // create centre complex plane
        centreComplexPlane = Instantiate(complexPlanePrefab, transform);

        // setup centre complex plane
        ComplexPlane cp = centreComplexPlane.GetComponent<ComplexPlane>();
        cp.SetupComplexPlane(planeType, position, scaleFactor);

        if (centreComplexPlaneComponent == null)
            centreComplexPlaneComponent = cp;

        // add complex plane to dictionary
        dictionary.Add(position, cp);
    }

    public ComplexPlane GetOverCurrentComplexPlane(Camera orthoCamComponent)
    {
        RaycastHit hit;
        Ray ray = orthoCamComponent.ScreenPointToRay(Input.mousePosition);
        ComplexPlane newComplexPlane = null;

        if (Physics.Raycast(ray, out hit))
        {
            newComplexPlane = GetComplexPlaneAtPosition(hit.collider.transform.position);
            if (newComplexPlane != currentComplexPlane)
            {
                currentComplexPlane = newComplexPlane;
            }
        }

        return currentComplexPlane;
    }

    public ComplexPlane GetComplexPlaneAtPosition(Vector3 position)
    {
        ComplexPlane cp;

        if (dictionary.TryGetValue(position, out cp))
        {
            return cp;
        } else
        {
            return null;
        }
    }

    public void CreatePlanes(Utility.Edge edge)
    {
        bool crossingLeftEdge = (edge & Utility.Edge.LEFT) == Utility.Edge.LEFT;
        bool crossingRightEdge = (edge & Utility.Edge.RIGHT) == Utility.Edge.RIGHT;
        bool crossingTopEdge = (edge & Utility.Edge.TOP) == Utility.Edge.TOP;
        bool crossingBottomEdge = (edge & Utility.Edge.BELOW) == Utility.Edge.BELOW;

        Vector3 position = currentComplexPlane.transform.position;

        if (crossingLeftEdge)
        {
            CreateComplexPlanePart(position, Utility.Position.LEFT);
            CreateComplexPlanePart(position, Utility.Position.LEFT_TOP);
            CreateComplexPlanePart(position, Utility.Position.LEFT_BELOW);
        }
        if (crossingRightEdge)
        {
            CreateComplexPlanePart(position, Utility.Position.RIGHT);
            CreateComplexPlanePart(position, Utility.Position.RIGHT_TOP);
            CreateComplexPlanePart(position, Utility.Position.RIGHT_BELOW);
        }
        if (crossingTopEdge)
        {
            CreateComplexPlanePart(position, Utility.Position.TOP);
            CreateComplexPlanePart(position, Utility.Position.LEFT_TOP);
            CreateComplexPlanePart(position, Utility.Position.RIGHT_TOP);
        }
        if (crossingBottomEdge)
        {
            CreateComplexPlanePart(position, Utility.Position.BELOW);
            CreateComplexPlanePart(position, Utility.Position.LEFT_BELOW);
            CreateComplexPlanePart(position, Utility.Position.RIGHT_BELOW);
        }
    }

    void CreateComplexPlanePart (Vector3 referencePlanePosition, Utility.Position position)
    {
        Vector3 newPosition = Vector3.zero;
        Utility.PlaneType newPlaneType = Utility.PlaneType.CENTRE;
        ComplexPlane tmp = null;

        switch (position)
        {
            case Utility.Position.LEFT:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(-centreComplexPlaneComponent.Size.x * scaleFactor, 0f, 0f);
                break;
            case Utility.Position.LEFT_BELOW:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(-centreComplexPlaneComponent.Size.x, -centreComplexPlaneComponent.Size.y, 0f) * scaleFactor;    
                break;
            case Utility.Position.LEFT_TOP:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(-centreComplexPlaneComponent.Size.x, centreComplexPlaneComponent.Size.y, 0f) * scaleFactor;
                break;
            case Utility.Position.RIGHT:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(centreComplexPlaneComponent.Size.x, 0f, 0f) * scaleFactor;
                break;
            case Utility.Position.RIGHT_BELOW:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(centreComplexPlaneComponent.Size.x, -centreComplexPlaneComponent.Size.y, 0f) * scaleFactor;
                break;
            case Utility.Position.RIGHT_TOP:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(centreComplexPlaneComponent.Size.x, centreComplexPlaneComponent.Size.y, 0f) * scaleFactor;
                break;
            case Utility.Position.TOP:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(0f, centreComplexPlaneComponent.Size.y, 0f) * scaleFactor;
                break;
            case Utility.Position.BELOW:
                // setup new position of the plane
                newPosition = referencePlanePosition + new Vector3(0f, -centreComplexPlaneComponent.Size.y, 0f) * scaleFactor;
                break;
        }

        
        if (!dictionary.TryGetValue(newPosition, out tmp))
        {
            newPlaneType = GetPlaneTypeFromNewPosition(newPosition);
            CreateNewComplexPlanePart(newPlaneType, newPosition);
        }
    }

    Utility.PlaneType GetPlaneTypeFromNewPosition (Vector3 newPosition)
    {
        Utility.PlaneType newPlaneType;

        if (newPosition.y == 0.0f)
        {
            newPlaneType = Utility.PlaneType.HORIZONTAL;
        }
        else if (newPosition.x == 0.0f)
        {
            newPlaneType = Utility.PlaneType.VERTICAL;
        }
        else
        {
            newPlaneType = Utility.PlaneType.DIAGONAL;
        }

        return newPlaneType;
    }
}