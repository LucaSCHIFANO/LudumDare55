using Unity.Mathematics;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnRadius = 1;
    [SerializeField] private Summonable skeleton;
    [SerializeField] private float spawnTime = 2;
    [SerializeField] private int startSpawnAmount = 8;
    private Pool<Summonable> pool;
    private float timer = 0;
    private bool isSpawning = false;
    int spawnCount = 0;

    private void Start()
    {
        pool = new Pool<Summonable>(skeleton);
        for (int i = 0; i < startSpawnAmount; i++)
        {
            SpawnSkeleton();
        }
        timer = spawnTime;
    }

    private void OnSkeletonSummoned()
    {
        isSpawning = true;
        spawnCount++;
    }

    private void Update()
    {
        if (!isSpawning) return;

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = spawnTime;
            SpawnSkeleton();
            spawnCount--;
            if(spawnCount <= 0)
            {
                spawnCount = 0;
                isSpawning = false;
            }
        }
    }

    private void SpawnSkeleton()
    {
        int rand = UnityEngine.Random.Range(0, spawnPoints.Length);
        Vector3 position = spawnPoints[rand].position + new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0);
        pool.Get(position).onGetSummoned.AddListener(OnSkeletonSummoned);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(spawnPoints[i].position, spawnRadius);
        }
    }
}
