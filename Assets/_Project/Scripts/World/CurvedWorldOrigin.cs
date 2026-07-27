using UnityEngine;

/// <summary>
/// Feeds the global bend values used by the "Curved World/Lit" shader.
/// The bend is visual only: colliders, transforms and gameplay logic stay flat.
/// </summary>
[ExecuteAlways]
public class CurvedWorldOrigin : MonoBehaviour
{
    static readonly int OriginId = Shader.PropertyToID("_CurveOrigin");
    static readonly int AmountId = Shader.PropertyToID("_CurveAmount");

    [Tooltip("Point where the world stays flat. Usually the player or the camera. Falls back to this transform.")]
    [SerializeField] Transform origin;

    [Tooltip("Bend along world Z (the running direction).")]
    [SerializeField, Range(0f, 0.02f)] float forwardCurve = 0.002f;

    [Tooltip("Bend along world X (the sides).")]
    [SerializeField, Range(0f, 0.02f)] float sideCurve = 0.0005f;

    void OnEnable() => Apply();

    void OnValidate() => Apply();

    void LateUpdate() => Apply();

    void OnDisable() => Shader.SetGlobalVector(AmountId, Vector4.zero);

    void Apply()
    {
        Transform pivot = origin != null ? origin : transform;
        Shader.SetGlobalVector(OriginId, pivot.position);
        Shader.SetGlobalVector(AmountId, new Vector4(forwardCurve, sideCurve, 0f, 0f));
    }
}
