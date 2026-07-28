using System;
using Genesis.Core.Events;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;

    IEventBus eventBus;

    public event Action<Coin> Collected;

    public void Configure(IEventBus bus) => eventBus = bus;

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement _)) return;
        eventBus.Publish(new CoinCollectedEvent(value));
        gameObject.SetActive(false);
        Collected?.Invoke(this);
    }
}
