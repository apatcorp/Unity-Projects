using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class ComplexNumberController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    MeshRenderer cnMeshRenderer;
    LineRenderer cnLineRenderer;
    TextMeshProUGUI complexnumberText;
    GameObject cpCanvas;

    ComplexNumber complexNumber;

    Color initialColour, focusColour;
    Vector3 initialScale, focusScale;

    bool focusing = false;

    public Action<bool> OverComplexNumber;

    private void Awake()
    {
        cnMeshRenderer = GetComponent<MeshRenderer>();
        cnLineRenderer = GetComponent<LineRenderer>();
        complexnumberText = GetComponentInChildren<TextMeshProUGUI>();
        cpCanvas = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        initialColour = cnMeshRenderer.material.color;
        focusColour = Color.green;

        initialScale = transform.localScale;
        focusScale = initialScale * 1.5f;
    }

    public void SetupComplexNumberController (Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, position.y, -1f);
        transform.position = newPosition;

        cnLineRenderer.positionCount += 2;
        cnLineRenderer.SetPosition(0, newPosition);
        cnLineRenderer.SetPosition(1, Vector3.zero);

        complexNumber = new ComplexNumber(transform.position.x, transform.position.y);

        complexnumberText.text = complexNumber.ToString();
        cpCanvas.SetActive(focusing);
    }

    IEnumerator ColourAnimation ()
    {
        float startTime = 0f, animationTime = .3f;

        Color targetColour = focusing ? focusColour : initialColour;

        while (startTime <= animationTime)
        {
            cnMeshRenderer.material.color = Color.Lerp(cnMeshRenderer.material.color, targetColour, startTime);
            cnLineRenderer.material.color = Color.Lerp(cnLineRenderer.material.color, targetColour, startTime);

            startTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ScaleAnimation()
    {
        float startTime = 0f, animationTime = .3f;

        Vector3 targetScale = focusing ? focusScale : initialScale;

        if (focusing)
            cpCanvas.SetActive(focusing);

        while (startTime <= animationTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, startTime);

            startTime += Time.deltaTime;
            yield return null;
        }

        if (!focusing)
            cpCanvas.SetActive(focusing);
    }

    void Animate ()
    {
        StopAllCoroutines();

        StartCoroutine("ColourAnimation");
        StartCoroutine("ScaleAnimation");
    }

    void UpdateComplexNumber(Vector2 newPosition)
    {
        // set the compley number to the new position
        Vector3 newPos = new Vector3(newPosition.x, newPosition.y, -1f);
        transform.position = newPosition;
        cnLineRenderer.SetPosition(0, newPos);

        // update complex number and text
        complexNumber.Real = transform.position.x;
        complexNumber.Imaginary = transform.position.y;

        complexnumberText.text = complexNumber.ToString();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UpdateComplexNumber(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UpdateComplexNumber(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UpdateComplexNumber(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        focusing = !focusing;
        Animate();

        if (OverComplexNumber != null)
            OverComplexNumber(focusing);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        focusing = !focusing;
        Animate();

        if (OverComplexNumber != null)
            OverComplexNumber(focusing);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            focusing = !focusing;

            if (OverComplexNumber != null)
                OverComplexNumber(focusing);

            Destroy(gameObject);
        }

        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            complexNumber.ComplexConjugate();
            UpdateComplexNumber(new Vector3(transform.position.x, complexNumber.Imaginary));
        }
    }
}
