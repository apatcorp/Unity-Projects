using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ComplexPlane : MonoBehaviour, IPointerClickHandler {

    [Range(1, 5)]
    public int spacingInterval = 1;

    [Range(1, 10)]
    public int gridInterval = 5;

    public GameObject spacingHorizontalPrefab, spacingVerticalPrefab;
    public GameObject gridHorizontalPrefab, gridVerticalPrefab;
    public GameObject complexNumberPrefab;

    Transform cpGFX, spacingGFX, gridGFX;
    GameObject imAxis, reAxis;
    int scaleFactor;

    Vector2 extents = Vector3.zero;
    public Vector2 Extents { get { return extents; } }
    public Vector2 Size { get { return extents * (2f / scaleFactor); } }

    public static bool overComplexNumber = false;


    public void SetupComplexPlane (Utility.PlaneType planeType, Vector3 position, int scaleFactor)
    {
        this.scaleFactor = scaleFactor;

        // set position and scale
        transform.position = position;
        transform.localScale *= scaleFactor;

        // get the drifferent gfx transforms
        cpGFX = transform.GetChild(0);
        spacingGFX = transform.GetChild(1);
        gridGFX = transform.GetChild(2);

        // get the real ans imaginary axis game objects
        reAxis = cpGFX.Find("Re-AxisGFX").gameObject;
        if (reAxis == null) reAxis = cpGFX.GetChild(2).gameObject;
        imAxis = cpGFX.Find("Im-AxisGFX").gameObject;
        if (imAxis == null) imAxis = cpGFX.GetChild(1).gameObject;

        // store the size of the complex plane
        BoxCollider boxCollider = cpGFX.GetComponentInChildren<BoxCollider>();
        extents = boxCollider.bounds.extents;

        // setup spacing and grid
        SetupSpacing(planeType);
        SetupGrid();
    }

    void SetupSpacing (Utility.PlaneType planeType)
    {
        switch (planeType)
        {
            case Utility.PlaneType.CENTRE:
                // setup both horizontal and vertical spacing
                SetupHorizontalSpacing(false);
                SetupVerticalSpacing(false);
                break;
            case Utility.PlaneType.HORIZONTAL:
                // setup horizontal spacing and hide imaginary axis
                imAxis.SetActive(false);
                SetupHorizontalSpacing(true);
                break;
            case Utility.PlaneType.VERTICAL:
                // setup vertical spacing and hide real axis
                reAxis.SetActive(false);
                SetupVerticalSpacing(true);
                break;
            case Utility.PlaneType.DIAGONAL:
                // hide both axis
                imAxis.SetActive(false);
                reAxis.SetActive(false);
                break;
        }
    }

    void SetupHorizontalSpacing (bool centreSpacing)
    {
        if (centreSpacing)
            CreateHorizontalSpacing(new Vector2(0, 0));

        for (int i = 1; i <= extents.x; i++)
        {
            if (i%spacingInterval == 0)
            {
                CreateHorizontalSpacing(new Vector2(i, 0));
                CreateHorizontalSpacing(new Vector2(-i, 0));
            }
        }
    }

    void SetupVerticalSpacing (bool centreSpacing)
    {
        if (centreSpacing)
            CreateVerticalSpacing(new Vector2(0, 0));

        for (int i = 1; i <= extents.y; i++)
        {
            if (i%spacingInterval == 0)
            {
                CreateVerticalSpacing(new Vector2(0, i));
                CreateVerticalSpacing(new Vector2(0, -i));
            }   
        }
    }

    void CreateHorizontalSpacing(Vector2 position)
    {
        GameObject horizontalSpacing = Instantiate(spacingHorizontalPrefab, spacingGFX);
        horizontalSpacing.transform.localPosition = new Vector3(position.x / scaleFactor, position.y / scaleFactor, horizontalSpacing.transform.localPosition.z);
        horizontalSpacing.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.RoundToInt(horizontalSpacing.transform.position.x).ToString();
    }

    void CreateVerticalSpacing(Vector2 position)
    {
        GameObject verticalSpacing = Instantiate(spacingVerticalPrefab, spacingGFX);
        verticalSpacing.transform.localPosition = new Vector3(position.x / scaleFactor, position.y / scaleFactor, verticalSpacing.transform.localPosition.z);
        verticalSpacing.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.RoundToInt(verticalSpacing.transform.position.y) + "i";
    }

    void SetupGrid()
    {
        SetupHorizontalGrid();
        SetupVerticalGrid();
    }

    void SetupHorizontalGrid()
    {
        for (int i = 0; i <= extents.y; i++)
        {
            if (i % gridInterval == 0)
            {
                CreateHorizontalGrid(new Vector2(0, i));
                CreateHorizontalGrid(new Vector2(0, -i));
            }
        }
    }

    void SetupVerticalGrid()
    {
        for (int i = 0; i <= extents.x; i++)
        {
            if (i % gridInterval == 0)
            {
                CreateVerticalGrid(new Vector2(i, 0));
                CreateVerticalGrid(new Vector2(-i, 0));
            }
        }
    }

    void CreateHorizontalGrid(Vector2 position)
    {
        GameObject horizontalGrid = Instantiate(gridHorizontalPrefab, gridGFX);
        horizontalGrid.transform.localPosition = new Vector3(position.x / scaleFactor, position.y / scaleFactor, horizontalGrid.transform.localPosition.z);
    }

    void CreateVerticalGrid(Vector2 position)
    {
        GameObject verticalGrid = Instantiate(gridVerticalPrefab, gridGFX);
        verticalGrid.transform.localPosition = new Vector3(position.x / scaleFactor, position.y / scaleFactor, verticalGrid.transform.localPosition.z);
    }

    void CreateNewComplexNumber (Vector3 position)
    {
        if (complexNumberPrefab)
        {
            GameObject nCn = Instantiate(complexNumberPrefab, transform.parent);
            ComplexNumberController cnC = nCn.GetComponent<ComplexNumberController>();
            cnC.OverComplexNumber += OverComplexNumber;
            cnC.SetupComplexNumberController(position);
        }    
    }

    void OverComplexNumber (bool overComplexNumber)
    {
        ComplexPlane.overComplexNumber = overComplexNumber;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!overComplexNumber && !eventData.dragging && eventData.button == PointerEventData.InputButton.Left)
        {
            CreateNewComplexNumber(eventData.pointerCurrentRaycast.worldPosition);
        }  
    }
}