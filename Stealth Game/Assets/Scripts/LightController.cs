using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    public LightGroup[] lightGroups;

    const float scalingFactor = 4f;

    Cell cellInfo;

    bool lightIsOn = false;
    // flickering
    float minTime;
    float maxTime;

    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    public void SetupLights (Cell cellInfo)
    {
        this.cellInfo = cellInfo;

        // set random range of flickering
        minTime = Random.Range(0.03f, 0.1f);
        maxTime = Random.Range(0.2f, 0.6f);

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

        PerformFlickering();

        foreach (LightGroup lightGroup in lightGroups)
        {
            lightGroup.EnableAllLights(lightIsOn);
        }
    }

    void AdjustVolume()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

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
        bool localLightIsOn = false;

        while (lightIsOn)
        {
            int randomOnOff = Random.Range(0, 2);
            float seconds = Random.Range(minTime, maxTime);

            AdjustVolume();

            if (randomOnOff == 0)
            {
                if (localLightIsOn)
                {
                    localLightIsOn = false;

                    group.EnableAllLights(localLightIsOn);
                    group.audioSource.Stop();
                }
            }
            else
            {
                if (!localLightIsOn)
                {
                    localLightIsOn = true;

                    group.EnableAllLights(localLightIsOn);
                    group.audioSource.Play();
                }
            }

            yield return new WaitForSeconds(seconds);
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
            lightRenderer.material.SetColor("_EmissionColor", value ? Color.white : Color.black);
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
