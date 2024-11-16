using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float gameTime;
    [SerializeField] float currentCurrency;
    Transform enemyContainer;
    [System.Serializable]
    struct EnemyTier
    {
        public float startSpawnTime;
        public float cost;
        public GameObject[] enemies;
    }

    [SerializeField] EnemyTier[] enemyTiers;
    EnemyTier nextEnemyTier;
    public bool readyForNewEnemy;
    void Awake()
    {
        enemyContainer = GameObject.Find("EnemyContainer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;


        if (readyForNewEnemy)
        {
            nextEnemyTier = GetNextEnemyTier();
            readyForNewEnemy = false;
        }
        else if (currentCurrency < nextEnemyTier.cost)
        {
            currentCurrency += Time.deltaTime;
        }
        else
        {
            currentCurrency = 0;
            readyForNewEnemy = true;
            GameObject enemyToSpawn = nextEnemyTier.enemies[Random.Range(0, nextEnemyTier.enemies.Length)];
            SpawnEnemy(enemyToSpawn);
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        float ScreenSize = Camera.main.orthographicSize;
        Vector2 spawnSide = Vector2.zero;
        switch (Random.Range(0, 4))
        {
            case 0: spawnSide = Vector2.up; break;
            case 1: spawnSide = Vector2.down; break;
            case 2: spawnSide = Vector2.left; break;
            case 3: spawnSide = Vector2.right; break;
        }
        spawnSide *= ScreenSize + 1;
        float spawnOffset = Random.Range(-ScreenSize, ScreenSize);

        Vector2 spawnPos = Vector2.zero;
        if (spawnSide.x == 0)
        {
            spawnPos = new Vector2(spawnOffset, spawnSide.y);
        }
        if (spawnSide.y == 0)
        {
            spawnPos = new Vector2(spawnSide.x, spawnOffset);
        }
        Instantiate(enemy, spawnPos, Quaternion.identity, enemyContainer);
    }

    EnemyTier GetNextEnemyTier()
    {
        EnemyTier nextTierSpawned;
        do
        {
            nextTierSpawned = enemyTiers[Random.Range(0, enemyTiers.Length)];
        } while (nextTierSpawned.startSpawnTime > gameTime);

        return nextTierSpawned;
    }
}
