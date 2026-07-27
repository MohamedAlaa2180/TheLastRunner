using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float rayStartOffset = 0.1f;
    [SerializeField] float rayLength = 0.3f;

    public bool IsGrounded { get; private set; }

    void FixedUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * rayStartOffset;
        IsGrounded = Physics.Raycast(origin, Vector3.down, rayLength, groundLayer);
    }
}
