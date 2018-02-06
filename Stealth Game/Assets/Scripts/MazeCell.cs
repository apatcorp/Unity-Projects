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
    [Range(10, 180)]
    public float spotAngle = 90f;
    [Range(0f, 20f)]
    public float rangeModifier = 10f;
    public Color lightColor = Color.white;
    public LightShadows shadowType = LightShadows.Hard;

    [Header("Reflection properties")]
    public bool boxProjection = true;
    [Range(0.01f, 5f)]
    public float reflectionIntensity = 1;
    public UnityEngine.Rendering.ReflectionProbeRefreshMode refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;

    // flickering
    float minTime;
    float maxTime;

    bool lightIsOn = true;
    bool playerInRange = false;
    bool activeCell = false;
    Color emissionColor;
    Vector3 worldPosition;

    Transform player;
    Vector3 directionToPlayer;

    AudioSource flickering;

    private void Start()
    {
        Random.InitState(name.GetHashCode());

        minTime = Random.Range(0.03f, 0.1f);
        maxTime = Random.Range(0.2f, 1f);

        player = FindObjectOfType<PlayerController>().transform;
    }

    public void SetupMazeCell(Cell mazeCell, bool lights)
    {
        this.mazeCell = mazeCell;
        worldPosition = mazeCell.worldPosition;
        transform.position = worldPosition;

        // get light and reflection probe components
        ceilingLight = GetComponentInChildren<Light>();
        reflectionProbe = GetComponentInChildren<ReflectionProbe>();
        lightMaterial = GetComponentInChildren<MeshRenderer>().material;
        emissionColor = lightMaterial.GetColor("_EmissionColor");


        if (lights)
        {
            // setup the ceiling light's position, rotation and scale
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
            ceilingLight.range = mazeCell.cellWidth + rangeModifier;

            // create two audio sources
            flickering = AudioManager.InstantiateAudioSource(ceilingLight.transform.position, AudioManager.Singleton.audios[1], transform);
        }
        else
        {
            // disbale ceiling lights
            ceilingLight.gameObject.SetActive(false);
        }

        // setup the reflection probe
        reflectionProbe.size = Vector3.one * 2f * mazeCell.cellWidth;
        reflectionProbe.boxProjection = boxProjection;
        //reflectionProbe.center = transform.localPosition;
        reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        reflectionProbe.refreshMode = refreshMode;
        reflectionProbe.importance = 1;
        reflectionProbe.intensity = reflectionIntensity;

        activeCell = lights;

        ToogleLight();
    }

    void ToogleLight ()
    {
        lightIsOn = !lightIsOn;
        ceilingLight.enabled = lightIsOn;
        lightMaterial.SetColor("_EmissionColor", lightIsOn ? emissionColor : Color.black);
    }

    void Update()
    {
        if (player && activeCell)
        {
            if (PlayerInRange())
            {
                if (!playerInRange)
                {
                    playerInRange = true;
                    ToogleLight();
                    StartCoroutine("Flickering");
                }
            } else
            {
                if (playerInRange)
                {
                    StopCoroutine("Flickering");                 
                    ToogleLight();
                    playerInRange = false;
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

    void AdjustVolume ()
    {
        directionToPlayer = (player.position - ceilingLight.transform.position).normalized;

        Ray ray = new Ray(ceilingLight.transform.position, directionToPlayer);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            float distance = (hit.collider.tag == "Player") ? hit.distance : Vector3.Distance(ray.origin, player.position);

            flickering.volume = (hit.collider.tag == "Player") ? 1f / distance : (1f / distance) * 0.4f;
            flickering.volume = Mathf.Clamp01(flickering.volume);
        }
    }

    IEnumerator Flickering ()
    {
        bool lightIsOn = false;

        while (playerInRange)
        {
            int randomOnOff = Random.Range(0, 2);
            float seconds = Random.Range(minTime, maxTime);

            AdjustVolume();
           
            if (randomOnOff == 0)
            {
                if (lightIsOn)
                {
                    lightIsOn = false;
                    ceilingLight.enabled = lightIsOn;
                    lightMaterial.SetColor("_EmissionColor", Color.black);
                    flickering.Stop();
                }
            }
            else
            {
                if (!lightIsOn)
                {
                    lightIsOn = true;
                    ceilingLight.enabled = lightIsOn;
                    lightMaterial.SetColor("_EmissionColor", emissionColor);
                    flickering.Play();
                }
            }

            
            yield return new WaitForSeconds(seconds);
        }
    }
}
