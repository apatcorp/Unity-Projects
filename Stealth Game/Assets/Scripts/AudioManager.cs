using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public List<Audio> audios;

    static Dictionary<int, Audio> audioDictionary = new Dictionary<int, Audio>();

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

        foreach (Audio audio in audios)
        {
            audioDictionary.Add(audio.name.GetHashCode(), audio);
        }
    }

    public static AudioSource InstantiateAudioSource(Vector3 audioSourcePosition, string name, Transform parent)
    {
        GameObject obj = new GameObject("AudioSource: " + name);
        obj.transform.position = audioSourcePosition;

        if (parent != null)
            obj.transform.SetParent(parent);

        AudioSource source = obj.AddComponent<AudioSource>();
        Audio audio;

        if (audioDictionary.TryGetValue(name.GetHashCode(), out audio))
        {
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
        } else
        {
            Debug.LogWarning("Audio \"" + name + "\" not found. Maybe check spelling");
        }

        return source;
    }
}

[System.Serializable]
public class Audio
{
    public string name;

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
