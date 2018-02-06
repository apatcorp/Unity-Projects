using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Audio[] audios;

    static AudioManager singleton;
    public static AudioManager Singleton
    {
        get
        {
            return singleton;
        }
    }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }


    public static AudioSource InstantiateAudioSource(Vector3 audioSourcePosition, Audio audio, Transform parent)
    {
        GameObject obj = new GameObject("AudioSource: " + parent.name);
        obj.transform.position = audioSourcePosition;

        if (parent != null)
            obj.transform.SetParent(parent);

        AudioSource source = obj.AddComponent<AudioSource>();

        source.clip = audio.clip;
        source.pitch = audio.pitch;
        source.playOnAwake = audio.playOnAwake;
        source.volume = audio.volume;
        source.loop = audio.loop;

        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.minDistance = audio.minDistance;
        source.maxDistance = audio.maxDistance;

        if (!source.isPlaying && source.playOnAwake)
            source.Play();

        return source;
    }
}

[System.Serializable]
public class Audio
{
    public AudioClip clip;
    [Range(0,1)]
    public float volume;
    [Range(-3, 3)]
    public float pitch;

    public bool playOnAwake;
    public bool loop;

    [Range(0, 500)]
    public float minDistance, maxDistance;
}
