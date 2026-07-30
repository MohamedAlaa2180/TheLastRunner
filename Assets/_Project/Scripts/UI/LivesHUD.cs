using System;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class LivesHUD : MonoBehaviour
{
    [SerializeField] Image[] lifeIcons;

    [Inject] IEventBus eventBus;

    IDisposable livesSub;

    void OnEnable() => livesSub = eventBus.Subscribe<LivesChangedEvent>(OnLivesChanged);

    void OnDisable() => livesSub?.Dispose();

    void OnLivesChanged(LivesChangedEvent evt)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
            lifeIcons[i].gameObject.SetActive(i < evt.Lives);
    }
}
