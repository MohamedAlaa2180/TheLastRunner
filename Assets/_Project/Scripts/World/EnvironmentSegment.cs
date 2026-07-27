using UnityEngine;

public class EnvironmentSegment : MonoBehaviour
{
    [SerializeField] Transform anchorStart;
    [SerializeField] Transform anchorEnd;

    public float StartOffset => anchorStart.localPosition.z;
    public float EndOffset => anchorEnd.localPosition.z;
    public float Length => EndOffset - StartOffset;
}
