using System;
using System.Collections;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class PlayerObstacleCollision : MonoBehaviour
{
    [SerializeField] float invulnerabilityDuration = 1.5f;

    [Inject] IEventBus eventBus;

    bool invulnerable;
    IDisposable hitSub;
    IDisposable resumeSub;
    IDisposable startedSub;
    Coroutine invulnRoutine;

    void OnEnable()
    {
        hitSub = eventBus.Subscribe<GameHitEvent>(_ => invulnerable = true);
        resumeSub = eventBus.Subscribe<GameResumedEvent>(_ => BeginInvulnerabilityWindow());
        startedSub = eventBus.Subscribe<GameStartedEvent>(_ =>
        {
            invulnerable = false;
            if (invulnRoutine != null)
            {
                StopCoroutine(invulnRoutine);
                invulnRoutine = null;
            }
        });
    }

    void OnDisable()
    {
        hitSub?.Dispose();
        resumeSub?.Dispose();
        startedSub?.Dispose();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (invulnerable) return;

        var obstacle = collision.gameObject.GetComponentInParent<Obstacle>();
        if (obstacle == null) return;

        invulnerable = true;
        obstacle.gameObject.SetActive(false);
        eventBus.Publish(new PlayerHitObstacleEvent());
    }

    void BeginInvulnerabilityWindow()
    {
        if (invulnRoutine != null)
            StopCoroutine(invulnRoutine);
        invulnRoutine = StartCoroutine(ClearInvulnerability());
    }

    IEnumerator ClearInvulnerability()
    {
        invulnerable = true;
        yield return new WaitForSecondsRealtime(invulnerabilityDuration);
        invulnerable = false;
        invulnRoutine = null;
    }
}
