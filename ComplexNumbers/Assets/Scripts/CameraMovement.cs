using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject orthographicCamera;

    Camera orthoCamComponent;

    public float movementSpeed = 250f;
    public float smoothTime = 0.4f;
    public float scrollSpeed = 8f;
    public float scrollSmoothTime = 0.9f;
    [Range(.5f, .99f)]
    public float screenDistancePercent = 0.85f;
    public float minDistance = 5f;
    public float maxDistance = 20f;

    float standardDistance = 0f;
    float scrollValue = 0f;
    float currentDistance = 0f;
    float currentVelocityScroll;

    float draggingSpeed;
    Vector3 oldPosition, direction = Vector3.zero, velocity = Vector3.zero;
    bool dragging = false, overComplexNumber = false;

    Vector3 movementDirection = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    ComplexPlane currentComplexPlane = null;
    Utility.Edge crossingEdge = Utility.Edge.NONE;

    ComplexNumberVisualizer cnv;

	void Awake ()
    {
		if (!orthographicCamera)
        {
            orthoCamComponent = Camera.main;
            orthographicCamera = Camera.main.gameObject;
        }
        if (!orthographicCamera)
        {
            Debug.LogError("No Camera found in scene");
            return;
        }    
    }

    private void Start()
    {
        // get complex number visualizer
        cnv = ComplexNumberVisualizer.Singleton;

        // get orthographic component
        orthoCamComponent = orthographicCamera.GetComponent<Camera>();
        orthoCamComponent.orthographic = true;

        // set camera parameters
        minDistance = Mathf.Clamp(minDistance, 0f, minDistance);
        maxDistance = Mathf.Clamp(maxDistance, minDistance, 50);
        standardDistance = (maxDistance + minDistance) / 2f;
        orthoCamComponent.orthographicSize = standardDistance;
        currentDistance = standardDistance;
    }

    void Update ()
    {
        //MoveCamera();
        Zoom();
        CheckCameraViewPort();

        if (Input.GetMouseButton(1) && !ComplexPlane.overComplexNumber)
        {
            if (!dragging)
            {
                oldPosition = Input.mousePosition;
                direction = Vector3.zero;
                dragging = true;
            }
            Drag();
        } else
        {
            if (dragging)
                dragging = false;
        }
	}

    void Drag()
    {
        velocity = (Input.mousePosition - oldPosition);
        draggingSpeed = velocity.magnitude / 15f;
        draggingSpeed = Mathf.Clamp(draggingSpeed, 0f, 50f);
        direction = (Input.mousePosition - oldPosition).normalized * draggingSpeed * Time.deltaTime;
        orthographicCamera.transform.position = Vector3.Lerp(orthographicCamera.transform.position, orthographicCamera.transform.position - direction, 5f);
    }

    void MoveCamera ()
    {
        movementDirection = Vector3.zero;
     
        if (Input.mousePosition.x > screenDistancePercent * Screen.width)
        {
            movementDirection += Vector3.right;
        }
        if (Input.mousePosition.x < Screen.width - screenDistancePercent * Screen.width)
        {
            movementDirection += Vector3.left;
        }
        if (Input.mousePosition.y > screenDistancePercent * Screen.height)
        {
            movementDirection += Vector3.up;
        }
        if (Input.mousePosition.y < Screen.height - screenDistancePercent * Screen.height)
        {
            movementDirection += Vector3.down;
        }

        movementDirection = movementDirection.normalized * movementSpeed * Time.deltaTime;

        orthographicCamera.transform.position = Vector3.SmoothDamp(orthographicCamera.transform.position, orthographicCamera.transform.position + movementDirection, ref currentVelocity, smoothTime);
    }

    void Zoom ()
    {
        scrollValue = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        currentDistance = Mathf.SmoothDamp(currentDistance, currentDistance - scrollValue, ref currentVelocityScroll, scrollSmoothTime * Time.deltaTime);
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        orthoCamComponent.orthographicSize = currentDistance;
    }

    void CheckCameraViewPort ()
    {
        float camXPosition = orthographicCamera.transform.position.x;
        float camYPosition = orthographicCamera.transform.position.y;
        float conversionXY = orthoCamComponent.orthographicSize * Screen.width / Screen.height;

        float currentViewRightEdge = camXPosition + conversionXY;
        float currentViewLeftEdge = camXPosition - conversionXY;
        float currentViewTopEdge = camYPosition + orthoCamComponent.orthographicSize;
        float currentViewBottomEdge = camYPosition - orthoCamComponent.orthographicSize;

        currentComplexPlane = cnv.GetOverCurrentComplexPlane(orthoCamComponent);

        if (currentComplexPlane)
        {
            bool crossingLeftEdge = currentViewLeftEdge <= (currentComplexPlane.transform.position.x - currentComplexPlane.Extents.x) * screenDistancePercent;
            bool crossingRightEdge = currentViewRightEdge >= (currentComplexPlane.transform.position.x + currentComplexPlane.Extents.x) * screenDistancePercent;
            bool crossingTopEdge = currentViewTopEdge >= (currentComplexPlane.transform.position.y + currentComplexPlane.Extents.y) * screenDistancePercent;
            bool crossingBottomEdge = currentViewBottomEdge <= (currentComplexPlane.transform.position.y - currentComplexPlane.Extents.y) * screenDistancePercent;

            crossingEdge = Utility.Edge.NONE;

            if (crossingLeftEdge)
            {
                crossingEdge |= Utility.Edge.LEFT;
            }
            if (crossingRightEdge)
            {
                crossingEdge |= Utility.Edge.RIGHT;
            }
            if (crossingTopEdge)
            {
                crossingEdge |= Utility.Edge.TOP;
            }
            if (crossingBottomEdge)
            {
                crossingEdge |= Utility.Edge.BELOW;
            }

            cnv.CreatePlanes(crossingEdge);
        }
    }
}
