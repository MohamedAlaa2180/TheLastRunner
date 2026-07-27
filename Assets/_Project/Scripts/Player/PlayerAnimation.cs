using System;
using UnityEngine;
using Genesis.Core.Events;
using Reflex.Attributes;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    static readonly int JumpHash = Animator.StringToHash("Jump");
    static readonly int SlideHash = Animator.StringToHash("Slide");
    static readonly int GroundedHash = Animator.StringToHash("Grounded");

    [Inject] IEventBus eventBus;

    Animator animator;
    IDisposable jumpSub;
    IDisposable slideSub;
    IDisposable groundedSub;

    void Awake() => animator = GetComponent<Animator>();

    void OnEnable()
    {
        jumpSub = eventBus.Subscribe<PlayerJumpedEvent>(_ => animator.SetTrigger(JumpHash));
        slideSub = eventBus.Subscribe<PlayerSlidedEvent>(_ => animator.SetTrigger(SlideHash));
        groundedSub = eventBus.Subscribe<PlayerGroundedChangedEvent>(e => animator.SetBool(GroundedHash, e.IsGrounded));
    }

    void OnDisable()
    {
        jumpSub?.Dispose();
        slideSub?.Dispose();
        groundedSub?.Dispose();
    }
}
