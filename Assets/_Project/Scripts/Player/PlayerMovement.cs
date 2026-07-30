using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Genesis.Core.Events;
using Reflex.Attributes;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float laneSpacing = 2.25f;
    [SerializeField] float laneChangeSpeed = 15f;
    [SerializeField] float jumpForce = 6f;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] float coyoteTime = 0.15f;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] float slideDuration = 0.8f;
    [SerializeField] float slideHeight = 1.6f;
    int laneIndex = 1;

    public float LaneSpacing => laneSpacing;

    [Inject] IEventBus eventBus;

    Rigidbody rb;
    InputSystem_Actions controls;
    float coyoteTimer;
    bool wasGrounded;

    float standHeight;
    Vector3 standCenter;
    float slideTimer;
    bool isSliding;
    bool isJumping;

    IDisposable startedSub;
    IDisposable pauseSub;
    IDisposable resumeSub;
    IDisposable gameOverSub;
    IDisposable hitSub;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();
        standHeight = capsuleCollider.height;
        standCenter = capsuleCollider.center;
    }

    void OnEnable()
    {
        controls.Player.SwitchLaneLeft.performed += OnSwitchLaneLeft;
        controls.Player.SwitchLaneRight.performed += OnSwitchLaneRight;
        controls.Player.Jump.performed += OnJump;
        controls.Player.Slide.performed += OnSlide;
        controls.Player.Disable();

        startedSub = eventBus.Subscribe<GameStartedEvent>(_ => controls.Player.Enable());
        pauseSub = eventBus.Subscribe<GamePausedEvent>(_ => controls.Player.Disable());
        resumeSub = eventBus.Subscribe<GameResumedEvent>(_ => controls.Player.Enable());
        gameOverSub = eventBus.Subscribe<GameOverEvent>(_ => controls.Player.Disable());
        hitSub = eventBus.Subscribe<GameHitEvent>(_ => controls.Player.Disable());
    }

    void OnDisable()
    {
        controls.Player.Disable();
        controls.Player.SwitchLaneLeft.performed -= OnSwitchLaneLeft;
        controls.Player.SwitchLaneRight.performed -= OnSwitchLaneRight;
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Slide.performed -= OnSlide;

        startedSub?.Dispose();
        pauseSub?.Dispose();
        resumeSub?.Dispose();
        gameOverSub?.Dispose();
        hitSub?.Dispose();
    }

    void OnSwitchLaneLeft(InputAction.CallbackContext context) => laneIndex = Mathf.Clamp(laneIndex - 1, 0, 2);

    void OnSwitchLaneRight(InputAction.CallbackContext context) => laneIndex = Mathf.Clamp(laneIndex + 1, 0, 2);

    void OnJump(InputAction.CallbackContext context)
    {
        if (coyoteTimer <= 0f) return;
        coyoteTimer = 0f;
        isJumping = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        eventBus.Publish(new PlayerJumpedEvent());
    }

    void OnSlide(InputAction.CallbackContext context)
    {
        if (isSliding || isJumping || !groundCheck.IsGrounded) return;

        isSliding = true;
        slideTimer = slideDuration;

        float bottom = standCenter.y - standHeight / 2f;
        capsuleCollider.height = slideHeight;
        capsuleCollider.center = new Vector3(standCenter.x, bottom + slideHeight / 2f, standCenter.z);

        eventBus.Publish(new PlayerSlidedEvent());
    }

    void FixedUpdate()
    {
        bool grounded = groundCheck.IsGrounded && rb.linearVelocity.y <= 0f;
        coyoteTimer = grounded ? coyoteTime : coyoteTimer - Time.fixedDeltaTime;
        if (grounded) isJumping = false;

        if (grounded != wasGrounded)
        {
            wasGrounded = grounded;
            eventBus.Publish(new PlayerGroundedChangedEvent(grounded));
        }

        if (isSliding)
        {
            slideTimer -= Time.fixedDeltaTime;
            if (slideTimer <= 0f)
            {
                isSliding = false;
                capsuleCollider.height = standHeight;
                capsuleCollider.center = standCenter;
            }
        }

        float targetX = (laneIndex - 1) * laneSpacing;
        Vector3 position = rb.position;
        position.x = Mathf.MoveTowards(position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        rb.MovePosition(position);
    }
}
