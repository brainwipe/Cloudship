using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    public Sound[] Sounds;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(var sound in Sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }
    }

    public void Play(string name)
    {
        var sound = Array.Find(Sounds, s => s.Name == name);
        if (sound == null)
        {
            Debug.LogWarning("Could not find sound " + name);
            return;
        }
        sound.Source.Play();
    }

    void Start()
    {
        Play("Wind");
    }
}

[Serializable]
public class Sound
{
    public string Name;

    public AudioClip Clip;

    [HideInInspector]
    public AudioSource Source;

    [Range(0f,1f)]
    public float Volume;

    [Range(0.1f, 3f)]
    public float Pitch;

    public bool Loop;
}