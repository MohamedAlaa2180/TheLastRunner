using System.Collections.Generic;
using Genesis.Core.Events;
using Reflex.Attributes;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] Coin coinPrefab;
    [SerializeField] PlayerMovement player;
    [SerializeField] EndlessEnvironmentSpawner environmentSpawner;
    [SerializeField] float spawnInterval = 8f;
    [SerializeField] float spawnDistanceAhead = 30f;
    [SerializeField] float recycleOffset = 20f;
    [SerializeField] int coinsPerRun = 4;
    [SerializeField] float coinSpacing = 1.5f;
    [SerializeField] float coinHeight = 1f;

    [Inject] IEventBus eventBus;

    readonly Queue<Coin> pool = new();
    readonly List<Coin> active = new();
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
            Coin coin = active[i];
            coin.transform.position += move;
            if (coin.transform.position.z < recycleZ)
            {
                active.RemoveAt(i);
                Release(coin);
            }
        }

        distanceUntilNextSpawn -= delta;
        if (distanceUntilNextSpawn <= 0f)
        {
            SpawnRun();
            distanceUntilNextSpawn = spawnInterval;
        }
    }

    void SpawnRun()
    {
        int lane = Random.Range(0, 3);
        float x = (lane - 1) * player.LaneSpacing;
        float startZ = player.transform.position.z + spawnDistanceAhead;

        for (int i = 0; i < coinsPerRun; i++)
        {
            Coin coin = GetCoin();
            coin.transform.position = new Vector3(x, coinHeight, startZ + i * coinSpacing);
            active.Add(coin);
        }
    }

    Coin GetCoin()
    {
        Coin coin;
        if (pool.Count > 0)
        {
            coin = pool.Dequeue();
        }
        else
        {
            coin = Instantiate(coinPrefab);
            coin.Collected += OnCoinCollected;
        }
        coin.Configure(eventBus);
        coin.gameObject.SetActive(true);
        return coin;
    }

    void OnCoinCollected(Coin coin)
    {
        active.Remove(coin);
        Release(coin);
    }

    void Release(Coin coin)
    {
        coin.gameObject.SetActive(false);
        pool.Enqueue(coin);
    }
}
