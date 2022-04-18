using UnityEngine.Audio;
using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

    public Sounds[] sound;
  
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sounds s in sound)
        {
           s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.time = s.time;
            s.source.playOnAwake = s.PlayOnAwake;
            s.source.spatialBlend = s.SpatialSound;
        }
    }

    void Start()
    {
        Play("bgmusic1");
        InvokeRepeating("playBell", 5, 1800);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sound, sound => sound.name == name);
        
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sounds s = Array.Find(sound, sound => sound.name == name);
        s.source.Stop();
    }

    void playBell()
    {
        Play("SchoolBell");
    }

}
