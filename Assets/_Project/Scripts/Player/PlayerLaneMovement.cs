using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerLaneMovement : MonoBehaviour
{
    [SerializeField] float laneSpacing = 2.25f;
    [SerializeField] float laneChangeSpeed = 15f;

    static readonly int JumpHash = Animator.StringToHash("Jump");
    static readonly int SlideHash = Animator.StringToHash("Slide");

    Rigidbody rb;
    Animator animator;
    InputSystem_Actions controls;
    [SerializeField] int laneIndex = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        controls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        controls.Player.SwitchLaneLeft.performed += OnSwitchLaneLeft;
        controls.Player.SwitchLaneRight.performed += OnSwitchLaneRight;
        controls.Player.Jump.performed += OnJump;
        controls.Player.Slide.performed += OnSlide;
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
        controls.Player.SwitchLaneLeft.performed -= OnSwitchLaneLeft;
        controls.Player.SwitchLaneRight.performed -= OnSwitchLaneRight;
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Slide.performed -= OnSlide;
    }

    void OnSwitchLaneLeft(InputAction.CallbackContext context) => laneIndex = Mathf.Clamp(laneIndex - 1, 0, 2);

    void OnSwitchLaneRight(InputAction.CallbackContext context) => laneIndex = Mathf.Clamp(laneIndex + 1, 0, 2);

    void OnJump(InputAction.CallbackContext context) => animator.SetTrigger(JumpHash);

    void OnSlide(InputAction.CallbackContext context) => animator.SetTrigger(SlideHash);

    void FixedUpdate()
    {
        float targetX = (laneIndex - 1) * laneSpacing;
        Vector3 position = rb.position;
        position.x = Mathf.MoveTowards(position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        rb.MovePosition(position);
    }
}
