using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public float gameTime;
    [SerializeField] float currentCurrency;
    Transform enemyContainer;
    Transform player;
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
        player = GameObject.FindWithTag("Player").transform;
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
        Camera cam = Camera.main;
        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector2 spawnSide = Vector2.zero;
        float spawnOffset =0;
        switch (Random.Range(0, 4))
        {
            case 0: 
                spawnSide = Vector2.up;
                spawnSide *= height + 1;
                spawnOffset = Random.Range(-width, width);
                break;
            case 1: 
                spawnSide = Vector2.down;
                spawnSide *= height + 1;
                spawnOffset = Random.Range(-width, width);

                break;
            case 2: 
                spawnSide = Vector2.left;
                spawnSide *= width + 1;
                spawnOffset = Random.Range(-height, height);

                break;
            case 3: 
                spawnSide = Vector2.right;
                spawnSide *= width + 1;
                spawnOffset = Random.Range(-height, height);

                break;
        }

        Vector2 spawnPos = Vector2.zero;
        if (spawnSide.x == 0)
        {
            spawnPos = new Vector2(spawnOffset, spawnSide.y) + new Vector2(player.position.x, player.position.y);
        }
        if (spawnSide.y == 0)
        {
            spawnPos = new Vector2(spawnSide.x, spawnOffset) + new Vector2(player.position.x, player.position.y);
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
