using System;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] int maxLives = 3;

    [Inject] IEventBus eventBus;

    public int Lives { get; private set; }

    IDisposable hitSub;

    void OnEnable()
    {
        Lives = maxLives;
        hitSub = eventBus.Subscribe<GameHitEvent>(_ => LoseLife());
        eventBus.Publish(new LivesChangedEvent(Lives));
    }

    void OnDisable() => hitSub?.Dispose();

    void LoseLife()
    {
        Lives = Mathf.Max(0, Lives - 1);
        eventBus.Publish(new LivesChangedEvent(Lives));
    }
}
