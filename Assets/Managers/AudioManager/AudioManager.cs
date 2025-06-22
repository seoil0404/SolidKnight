using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backGroundAudioSourcePrefab;
    [SerializeField] private AudioSource effectAudioSourcePrefab;

    private LinkedList<AudioSource> backGroundAudioSource = new();
    private List<AudioSource> effectAudioSources = new();

    private static AudioManager Instance { get; set; } = null;
    public static bool IsAllocated => Instance != null;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        gameObject.name = typeof(AudioManager).Name;

        backGroundAudioSource.AddFirst(Instantiate(backGroundAudioSourcePrefab, transform));
    }

    private void PlayByInstance(AudioClip audioClip, float volume)
    {
        AudioSource audioSource = GetAvailableSource();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    private AudioSource GetAvailableSource()
    {
        foreach (var effectAudioSource in effectAudioSources)
        {
            if (!effectAudioSource.isPlaying)
                return effectAudioSource;
        }

        AudioSource newSource = Instantiate(effectAudioSourcePrefab, transform);
        effectAudioSources.Add(newSource);
        return newSource;
    }

    public static void Play(AudioClip audioClip, float volume = 1f)
    {
        Instance.PlayByInstance(audioClip, volume);
    }
        

    private void SetBackgroundMusicByInstance(AudioClip audioClip, float volume , float fadeTime)
    {
        backGroundAudioSource.First.Value.DOKill();
        backGroundAudioSource.First.Value.DOFade(0, fadeTime);
        backGroundAudioSource.AddLast(backGroundAudioSource.First.Value);
        backGroundAudioSource.RemoveFirst();

        if(!Mathf.Approximately(backGroundAudioSource.First.Value.volume, 0))
            backGroundAudioSource.AddFirst(Instantiate(backGroundAudioSourcePrefab, transform));

        AudioSource audioSource = backGroundAudioSource.First.Value;
        audioSource.clip = audioClip;
        audioSource.Play();
        audioSource.volume = 0;
        audioSource.DOKill();
        audioSource.DOFade(volume, fadeTime);
    }

    public static void SetBackgroundMusic(AudioClip audioClip, float volume = 1f , float fadeTime = 0.5f)
    {
        Instance.SetBackgroundMusicByInstance(audioClip, volume, fadeTime);
    }
        


    private void SetBackGroundVolumeByInstance(float volume, float time)
    {
        backGroundAudioSource.First.Value.DOKill();
        backGroundAudioSource.First.Value.DOFade(volume, time);
    }

    public static void SetBackGroundVolume(float volume, float time = 0.5f) =>
        Instance.SetBackGroundVolumeByInstance(volume, time);
}
