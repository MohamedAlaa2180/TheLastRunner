using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] PlayerMovement player;
    [SerializeField] EndlessEnvironmentSpawner environmentSpawner;
    [SerializeField] float spawnInterval = 10f;
    [SerializeField] float spawnDistanceAhead = 40f;
    [SerializeField] float recycleOffset = 20f;

    readonly Dictionary<GameObject, Queue<GameObject>> pools = new();
    readonly List<(GameObject prefab, GameObject instance)> active = new();
    float distanceUntilNextSpawn;

    void Start()
    {
        distanceUntilNextSpawn = spawnDistanceAhead;
    }

    void Update()
    {
        float delta = environmentSpawner.Speed * Time.deltaTime;
        Vector3 move = Vector3.back * delta;
        float recycleZ = player.transform.position.z - recycleOffset;

        for (int i = active.Count - 1; i >= 0; i--)
        {
            (GameObject prefab, GameObject instance) entry = active[i];
            if (!entry.instance.activeSelf)
            {
                active.RemoveAt(i);
                Release(entry.prefab, entry.instance);
                continue;
            }

            entry.instance.transform.position += move;
            if (entry.instance.transform.position.z < recycleZ)
            {
                active.RemoveAt(i);
                Release(entry.prefab, entry.instance);
            }
        }

        distanceUntilNextSpawn -= delta;
        if (distanceUntilNextSpawn <= 0f)
        {
            SpawnObstacle();
            distanceUntilNextSpawn = spawnInterval;
        }
    }

    void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        int lane = Random.Range(0, 3);
        float x = (lane - 1) * player.LaneSpacing;
        float z = player.transform.position.z + spawnDistanceAhead;

        GameObject instance = GetInstance(prefab);
        instance.transform.position = new Vector3(x, prefab.transform.position.y, z);
        active.Add((prefab, instance));
    }

    GameObject GetInstance(GameObject prefab)
    {
        Queue<GameObject> pool = GetPool(prefab);
        GameObject instance = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        instance.SetActive(true);
        return instance;
    }

    Queue<GameObject> GetPool(GameObject prefab)
    {
        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools[prefab] = pool;
        }
        return pool;
    }

    void Release(GameObject prefab, GameObject instance)
    {
        instance.SetActive(false);
        GetPool(prefab).Enqueue(instance);
    }
}
