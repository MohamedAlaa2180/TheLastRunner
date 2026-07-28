using System;
using System.Collections.Generic;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IAudioService
{
    [Header("Mixer")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerGroup sfxGroup;
    [SerializeField] AudioMixerGroup musicGroup;

    [Header("SFX Pool")]
    [SerializeField] int sfxPoolSize = 8;

    [Header("Coin Combo")]
    [SerializeField] AudioClip coinClip;
    [SerializeField] float basePitch = 1f;
    [SerializeField] float pitchStep = 0.08f;
    [SerializeField] float maxPitch = 2f;
    [SerializeField] float comboResetTime = 1f;

    [Inject] IEventBus eventBus;

    readonly List<AudioSource> sfxPool = new();
    AudioSource musicSource;

    int coinStreak;
    float lastCoinTime;
    IDisposable coinSub;

    void Awake()
    {
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.outputAudioMixerGroup = sfxGroup;
            src.playOnAwake = false;
            sfxPool.Add(src);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.playOnAwake = false;
    }

    void OnEnable() => coinSub = eventBus.Subscribe<CoinCollectedEvent>(OnCoinCollected);

    void OnDisable() => coinSub?.Dispose();

    void OnCoinCollected(CoinCollectedEvent evt)
    {
        coinStreak = Time.time - lastCoinTime > comboResetTime ? 0 : coinStreak + 1;
        lastCoinTime = Time.time;

        float pitch = Mathf.Min(basePitch + coinStreak * pitchStep, maxPitch);
        PlaySfx(coinClip, pitch: pitch);
    }

    public void PlaySfx(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;
        var src = GetFreeSource();
        src.pitch = pitch;
        src.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void SetSfxVolume(float linear01) =>
        mixer.SetFloat("SFXVolume", LinearToDb(linear01));

    public void SetMusicVolume(float linear01) =>
        mixer.SetFloat("MusicVolume", LinearToDb(linear01));

    static float LinearToDb(float linear01) =>
        Mathf.Log10(Mathf.Max(linear01, 0.0001f)) * 20f;

    AudioSource GetFreeSource()
    {
        foreach (var src in sfxPool)
            if (!src.isPlaying) return src;
        return sfxPool[0];
    }
}
