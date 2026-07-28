using System;
using System.Collections.Generic;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class PlayerCoinEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem shinePrefab;
    [SerializeField] Vector3 offset;
    [SerializeField] float effectDuration = 1f;
    [SerializeField] int poolSize = 4;

    [Inject] IEventBus eventBus;

    readonly Queue<ParticleSystem> pool = new();
    readonly List<(ParticleSystem ps, float timer)> active = new();

    IDisposable coinSub;

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
            pool.Enqueue(CreateInstance());
    }

    void OnEnable() => coinSub = eventBus.Subscribe<CoinCollectedEvent>(OnCoinCollected);

    void OnDisable() => coinSub?.Dispose();

    void Update()
    {
        for (int i = active.Count - 1; i >= 0; i--)
        {
            (ParticleSystem ps, float timer) = active[i];
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                active.RemoveAt(i);
                Release(ps);
            }
            else
            {
                active[i] = (ps, timer);
            }
        }
    }

    void OnCoinCollected(CoinCollectedEvent evt)
    {
        ParticleSystem ps = pool.Count > 0 ? pool.Dequeue() : CreateInstance();
        ps.transform.localPosition = offset;
        ps.gameObject.SetActive(true);
        ps.Play();
        active.Add((ps, effectDuration));
    }

    ParticleSystem CreateInstance()
    {
        ParticleSystem ps = Instantiate(shinePrefab, transform);
        ps.gameObject.SetActive(false);
        return ps;
    }

    void Release(ParticleSystem ps)
    {
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.gameObject.SetActive(false);
        pool.Enqueue(ps);
    }
}
