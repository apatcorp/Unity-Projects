using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    Light ceilingLight;
    ReflectionProbe reflectionProbe;
    Material lightMaterial;

    Cell mazeCell;

    const float scalingFactor = 4f;

    [Header("Light properties")]
    public float intensity = 2;
    public float spotAngle = 90f;
    public Color lightColor = Color.white;
    public LightShadows shadowType = LightShadows.Hard;

    [Header("Reflection properties")]
    public bool boxProjection = true;
    public float RPintensity = 2;
    public UnityEngine.Rendering.ReflectionProbeRefreshMode refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
    
    bool lightOn = false;
    Color emissionColor;
    Vector3 worldPosition;

    Transform player;
    Bounds bounds;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

        StartCoroutine("Flickering");
    }

    public void SetupMazeCell(Cell mazeCell)
    {
        this.mazeCell = mazeCell;
        worldPosition = mazeCell.worldPosition;

        // get light and reflection probe components
        ceilingLight = GetComponentInChildren<Light>();
        reflectionProbe = GetComponentInChildren<ReflectionProbe>();
        lightMaterial = GetComponentInChildren<MeshRenderer>().material;
        emissionColor = lightMaterial.GetColor("_EmissionColor");

        // setup the ceiling light's position, rotation and scale
        transform.position = mazeCell.worldPosition;
        ceilingLight.transform.localPosition += Vector3.up * mazeCell.cellWidth / 2f;
        ceilingLight.transform.localScale = Vector3.one * mazeCell.cellWidth / scalingFactor;
        ceilingLight.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        // setup the light component
        ceilingLight.type = LightType.Spot;
        ceilingLight.spotAngle = spotAngle;
        ceilingLight.color = lightColor;
        ceilingLight.intensity = intensity;
        ceilingLight.renderMode = LightRenderMode.ForcePixel;
        ceilingLight.shadows = shadowType;
        ceilingLight.range = mazeCell.cellWidth + scalingFactor;

        // setup the reflection probe
        reflectionProbe.size = Vector3.one * 2f * mazeCell.cellWidth;
        reflectionProbe.boxProjection = boxProjection;
        //reflectionProbe.center = transform.localPosition;
        reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        reflectionProbe.refreshMode = refreshMode;
        reflectionProbe.importance = 1;
        reflectionProbe.intensity = RPintensity;
    }

    void ToggleLight ()
    {
        ceilingLight.enabled = lightOn;
        lightMaterial.SetColor("_EmissionColor", lightOn ? emissionColor: Color.black);
    }

    void ToogleReflectionProbe ()
    {
        reflectionProbe.enabled = lightOn;
    }

    private void Update()
    {
        if (player)
        {
            if (PlayerInRange())
            {
                if (!lightOn)
                {
                    lightOn = true;
                    ToggleLight();
                    //ToogleReflectionProbe();
                }
            } else
            {
                if (lightOn)
                {
                    lightOn = false;
                    ToggleLight();
                    //ToogleReflectionProbe();
                }
            }
        }     
    }

    bool PlayerInRange ()
    {
        bool playerInRange = false;

        Vector3 leftFrontEdge = worldPosition + new Vector3((-mazeCell.cellWidth / 2f), 0f, (mazeCell.cellWidth / 2f));
        Vector3 rightFrontEdge = worldPosition + new Vector3((mazeCell.cellWidth / 2f), 0f, mazeCell.cellWidth / 2f);
        Vector3 leftBackEdge = worldPosition + new Vector3((-mazeCell.cellWidth / 2f), 0f, -mazeCell.cellWidth / 2f);
        Vector3 rightBackEdge = worldPosition + new Vector3((mazeCell.cellWidth / 2f), 0f, -mazeCell.cellWidth / 2f);

        Ray ray1 = new Ray(leftFrontEdge, (player.position - leftFrontEdge).normalized);
        Ray ray2 = new Ray(rightFrontEdge, (player.position - rightFrontEdge).normalized);
        Ray ray3 = new Ray(leftBackEdge, (player.position - leftBackEdge).normalized);
        Ray ray4 = new Ray(rightBackEdge, (player.position - rightBackEdge).normalized);


        Debug.DrawRay(ray1.origin, ray1.direction * Mathf.Infinity, Color.red);
        Debug.DrawRay(ray2.origin, ray2.direction * Mathf.Infinity, Color.green);
        Debug.DrawRay(ray3.origin, ray3.direction * Mathf.Infinity, Color.blue);
        Debug.DrawRay(ray4.origin, ray4.direction * Mathf.Infinity, Color.yellow);

        RaycastHit hit1, hit2, hit3, hit4;

        bool ray1Hit = Physics.Raycast(ray1, out hit1);
        bool ray2Hit = Physics.Raycast(ray2, out hit2);
        bool ray3Hit = Physics.Raycast(ray3, out hit3);
        bool ray4Hit = Physics.Raycast(ray4, out hit4);

        if (ray1Hit && !playerInRange)
        {
            playerInRange = hit1.collider.tag == "Player";
        }

        if (ray2Hit && !playerInRange)
        {
            playerInRange = hit2.collider.tag == "Player";
        }

        if (ray3Hit && !playerInRange)
        {
            playerInRange = hit3.collider.tag == "Player";
        }

        if (ray4Hit && !playerInRange)
        {
            playerInRange = hit4.collider.tag == "Player";
        }

        return playerInRange;
    }

    IEnumerator Flickering ()
    {
        while (true)
        {
            int randomno = Random.Range(0, 1);
            float seconds = Random.Range(0.1f, 3.2f);

            if (randomno == 0)
            {
                lightOn = false;
            } else
            {
                lightOn = true;
            }

            ToggleLight();

            yield return new WaitForSeconds(seconds);
        }
    }
}
