using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    Sound wind;
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
            sound.Initialise();
        }
        wind = Find("Wind");
    }

    void Start()
    {
        wind.Source.Play();
    }

    public void Play(string soundName)
    {
        Sound sound = Find(soundName);
        sound.Source.Play();
    }

    public void SetWindFromVelocity(float velocity)
    {
        var minVolume = 0.2f;
        var maxVolume = 1f;
        var maxVelocity = FlyingPhysics.Vne;
        var minPitch = 0.3f;
        var maxPitch = 1.2f;

        wind.SetVolume(Maths.Rescale(minVolume, maxVolume, 0, maxVelocity, velocity));
        wind.SetPitch(Maths.Rescale(minPitch, maxPitch, 0, maxVelocity, velocity));
    }

    Sound Find(string soundName)
    {
        var sound = Array.Find(Sounds, s => s.Name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Could not find sound " + soundName);
            return null;
        }
        return sound;
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

    public void Initialise()
    {
        Source = AudioManager.Instance.gameObject.AddComponent<AudioSource>();
        Source.clip = Clip;
        Source.volume = Volume;
        Source.pitch = Pitch;
        Source.loop = Loop;
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
        Source.volume = volume;
    }

    public void SetPitch(float pitch)
    {
        Pitch = pitch;
        Source.pitch = pitch;
    }
}