using System.Collections.Generic;
using UnityEngine;

public class EndlessEnvironmentSpawner : MonoBehaviour
{
    [SerializeField] EnvironmentSegment segmentPrefab;
    [SerializeField] Transform player;
    [SerializeField] int segmentCount = 4;
    [SerializeField] float speed = 8f;
    [SerializeField] float recycleOffset = 20f;

    public float Speed => speed;

    readonly Queue<EnvironmentSegment> pool = new();
    readonly List<EnvironmentSegment> active = new();

    void Start()
    {
        for (int i = 0; i < segmentCount; i++)
            SpawnSegment();
    }

    void Update()
    {
        if (active.Count == 0)
            return;

        Vector3 move = Vector3.back * speed * Time.deltaTime;
        foreach (EnvironmentSegment segment in active)
            segment.transform.position += move;

        EnvironmentSegment oldest = active[0];
        float trailingEdgeZ = oldest.transform.position.z + oldest.StartOffset;
        if (trailingEdgeZ < player.position.z - recycleOffset)
        {
            active.RemoveAt(0);
            Release(oldest);
            SpawnSegment();
        }
    }

    void SpawnSegment()
    {
        EnvironmentSegment segment = pool.Count > 0 ? pool.Dequeue() : Instantiate(segmentPrefab, transform);
        float boundaryZ = active.Count > 0 ? active[^1].transform.position.z + active[^1].EndOffset : 0f;
        segment.transform.position = new Vector3(0f, 0f, boundaryZ - segment.StartOffset);
        segment.gameObject.SetActive(true);
        active.Add(segment);
    }

    void Release(EnvironmentSegment segment)
    {
        segment.gameObject.SetActive(false);
        pool.Enqueue(segment);
    }
}
