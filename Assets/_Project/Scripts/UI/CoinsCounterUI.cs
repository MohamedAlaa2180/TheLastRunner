using System;
using Genesis.Core.Events;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class CoinsCounterUI : MonoBehaviour
{
    [SerializeField] TMP_Text valueText;

    [Inject] IEventBus eventBus;

    int total;
    IDisposable coinSub;
    IDisposable startedSub;

    void OnEnable()
    {
        coinSub = eventBus.Subscribe<CoinCollectedEvent>(OnCoinCollected);
        startedSub = eventBus.Subscribe<GameStartedEvent>(_ => ResetCount());
    }

    void OnDisable()
    {
        coinSub?.Dispose();
        startedSub?.Dispose();
    }

    void OnCoinCollected(CoinCollectedEvent evt)
    {
        total += evt.Amount;
        UpdateText();
    }

    void ResetCount()
    {
        total = 0;
        UpdateText();
    }

    void UpdateText() => valueText.text = total.ToString();
}
