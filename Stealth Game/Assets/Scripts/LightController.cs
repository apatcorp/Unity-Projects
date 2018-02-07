using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    public LightGroup[] lightGroups;

    const float scalingFactor = 4f;

    Cell cellInfo;

    bool lightIsOn = false;

    // flickering
    [Header("Flickering")]
    public bool flickering = true;
    [Range(0.01f, 5f)]
    public float minTimeUntilNextFlicker = 0.05f;
    [Range(0.05f, 15f)]
    public float maxTimeUntiNextFlicker = 2f;
    [Range(0.01f, .05f)]
    public float minflickerTime = .05f;
    [Range(0.1f, 2f)]
    public float maxflickerTime = .1f;

    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    public void SetupLights (Cell cellInfo)
    {
        this.cellInfo = cellInfo;

        // position the light component at the ceiling    
        transform.localScale = Vector3.one * cellInfo.cellWidth / scalingFactor;
        transform.position += Vector3.up * (cellInfo.cellWidth / 2f) * .8f;

        // setup the lights for each light in each light group
        foreach (LightGroup lightGroup in lightGroups)
        {
            lightGroup.SetupLights(this.cellInfo.cellWidth);
            lightGroup.EnableAllLights(lightIsOn);
        }
    }

    public void ToogleLights()
    {
        lightIsOn = !lightIsOn;

        if (flickering)
            PerformFlickering();

        foreach (LightGroup lightGroup in lightGroups)
        {
            lightGroup.EnableAllLights(lightIsOn);
        }
    }

    void AdjustVolume()
    {
        // find direction vector from light object to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // cast a ray to that player and adjust the sound accordingly
        Ray ray = new Ray(transform.position, directionToPlayer);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            float distance = (hit.collider.tag == "Player") ? hit.distance : Vector3.Distance(ray.origin, player.position);

            foreach (LightGroup group in lightGroups)
            {
                group.audioSource.volume = (hit.collider.tag == "Player") ? 1f / distance : (1f / distance) * 0.4f;
                group.audioSource.volume = Mathf.Clamp01(group.audioSource.volume);
            }
        }
    }

    void PerformFlickering ()
    {
        if (lightIsOn)
        {
            foreach (LightGroup group in lightGroups)
            {
                StartCoroutine("Flickering", group);
            }
        }
        else
        {
            StopAllCoroutines();
        }       
    }

    IEnumerator Flickering(LightGroup group)
    {
        // set local light is on variable
        int localLightIsOn = 0;
        float secondsUntilNextFlicker = 0f;
        float flickerTime = 0f;

        while (lightIsOn)
        {
            // define randomly whether this light group's lights are on (1) or off (0)
            localLightIsOn = Random.Range(0, 2);
            secondsUntilNextFlicker = Random.Range(minTimeUntilNextFlicker, maxTimeUntiNextFlicker);
            flickerTime = Random.Range(minflickerTime, maxflickerTime);

            AdjustVolume();

            if (localLightIsOn == 0)
            {
                group.EnableAllLights(false);
                group.audioSource.Stop();
                yield return new WaitForSeconds(flickerTime);
                group.EnableAllLights(true);
                group.audioSource.Play();
            }
            else
            {
                group.EnableAllLights(true);
                group.audioSource.Play();
                yield return new WaitForSeconds(flickerTime);
                group.EnableAllLights(false);
                group.audioSource.Stop();
            }

            yield return new WaitForSeconds(secondsUntilNextFlicker);
        }
    }

    [System.Serializable]
    public class LightGroup
    {
        public Transform lightGroup;
        List<Light> lights = new List<Light>();

        [Header("Light properties")]
        public float intensity = 2;
        [Range(10, 180)]
        public float spotAngle = 90f;
        [Range(0f, 20f)]
        public float rangeModifier = 10f;
        public Color lightColor = Color.white;
        public LightShadows shadowType = LightShadows.Hard;

        public MeshRenderer lightRenderer;

        public AudioSource audioSource { get; private set; }

        public void EnableAllLights (bool value)
        {
            lightRenderer.material.SetColor("_EmissionColor", value ? lightColor : Color.black);
            lights.ForEach(x => x.enabled = value);
        }

        public void SetupLights (float cellWidth)
        {
            for (int i = 0; i < lightGroup.childCount; i++)
            {
                Light light = lightGroup.GetChild(i).GetComponent<Light>();
                SetupLight(light, cellWidth);
                lights.Add(light);
            }

            audioSource = AudioManager.InstantiateAudioSource(lightGroup.position, AudioManager.Singleton.audios[1], lightGroup);
        }

        void SetupLight(Light light, float cellWidth)
        {
            light.type = LightType.Spot;
            light.spotAngle = spotAngle;
            light.color = lightColor;
            light.intensity = intensity;
            light.renderMode = LightRenderMode.ForcePixel;
            light.shadows = shadowType;
            light.range = cellWidth + rangeModifier;
        }
    }
}
