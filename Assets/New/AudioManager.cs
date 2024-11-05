using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<AudioSource> audioSources = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAudioSources()
    {
        for (int i = 0; i < 10; i++) // Adjust the number of sources as needed
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            audioSources.Add(source);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource availableSource = audioSources.Find(source => !source.isPlaying);
        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.Play();
        }
    }
}
