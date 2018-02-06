using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    // reference to the light controller
    LightController lightController;
    ReflectionProbe reflectionProbe;
    Material lightMaterial;

    Cell mazeCell;

    [Header("Reflection properties")]
    public bool boxProjection = true;
    [Range(0.01f, 5f)]
    public float reflectionIntensity = 1;
    public UnityEngine.Rendering.ReflectionProbeRefreshMode refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;

    
    bool playerInRange = false;
    bool lightActiveInCell = false;
    Color emissionColor;
    Vector3 worldPosition;

    Transform player;
    Vector3 directionToPlayer;

    AudioSource flickering;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    public void SetupMazeCell(Cell mazeCell, bool lights)
    {
        // set the cell position in the maze
        this.mazeCell = mazeCell;
        worldPosition = mazeCell.worldPosition;
        transform.position = worldPosition;

        // setup the light 
        lightActiveInCell = lights;
        lightController = GetComponentInChildren<LightController>();
        lightController.gameObject.SetActive(lightActiveInCell);
        if (lightActiveInCell)
            lightController.SetupLights(this.mazeCell);  

        // setup the reflection probe
        reflectionProbe = GetComponentInChildren<ReflectionProbe>();
        reflectionProbe.size = Vector3.one * 2f * mazeCell.cellWidth;
        reflectionProbe.boxProjection = boxProjection;
        reflectionProbe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        reflectionProbe.refreshMode = refreshMode;
        reflectionProbe.importance = 1;
        reflectionProbe.intensity = reflectionIntensity;
    }

    void Update()
    {
        if (player && lightActiveInCell)
        {
            if (PlayerInRange())
            {
                if (!playerInRange)
                {
                    lightController.ToogleLights();
                    playerInRange = true;
                }
            } else
            {
                if (playerInRange)
                {
                    lightController.ToogleLights();
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
}
