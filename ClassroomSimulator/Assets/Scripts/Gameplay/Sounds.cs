using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public float SpatialSound;

    [Range(0f, 1f)]
    public float time;

    public bool loop;
    public bool PlayOnAwake;

    [HideInInspector]
    public AudioSource source;
}
