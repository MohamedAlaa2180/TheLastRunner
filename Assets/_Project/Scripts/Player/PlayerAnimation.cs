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
    static readonly int StunHash = Animator.StringToHash("Stun");

    [Inject] IEventBus eventBus;

    Animator animator;
    IDisposable jumpSub;
    IDisposable slideSub;
    IDisposable groundedSub;
    IDisposable hitSub;
    IDisposable resumeSub;
    IDisposable startedSub;

    void Awake() => animator = GetComponent<Animator>();

    void OnEnable()
    {
        jumpSub = eventBus.Subscribe<PlayerJumpedEvent>(_ => animator.SetTrigger(JumpHash));
        slideSub = eventBus.Subscribe<PlayerSlidedEvent>(_ => animator.SetTrigger(SlideHash));
        groundedSub = eventBus.Subscribe<PlayerGroundedChangedEvent>(e => animator.SetBool(GroundedHash, e.IsGrounded));
        hitSub = eventBus.Subscribe<GameHitEvent>(_ =>
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetBool(StunHash, true);
        });
        resumeSub = eventBus.Subscribe<GameResumedEvent>(_ => ClearStun());
        startedSub = eventBus.Subscribe<GameStartedEvent>(_ => ClearStun());
    }

    void OnDisable()
    {
        jumpSub?.Dispose();
        slideSub?.Dispose();
        groundedSub?.Dispose();
        hitSub?.Dispose();
        resumeSub?.Dispose();
        startedSub?.Dispose();
    }

    void ClearStun()
    {
        animator.SetBool(StunHash, false);
        animator.updateMode = AnimatorUpdateMode.Normal;
    }
}
