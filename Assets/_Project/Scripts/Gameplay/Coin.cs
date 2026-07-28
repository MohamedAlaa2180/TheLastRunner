using System;
using DG.Tweening;
using Genesis.Core.Events;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] float rotationDuration = 1f;

    IEventBus eventBus;
    Tween rotationTween;

    public event Action<Coin> Collected;

    public void Configure(IEventBus bus) => eventBus = bus;

    void OnEnable()
    {
        rotationTween = transform
            .DOLocalRotate(new Vector3(0f, 360f, 0f), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    void OnDisable()
    {
        rotationTween?.Kill();
        rotationTween = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement _)) return;
        eventBus.Publish(new CoinCollectedEvent(value));
        gameObject.SetActive(false);
        Collected?.Invoke(this);
    }
}
