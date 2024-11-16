using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    //Current Projectile prefab
    [SerializeField] private GameObject projectile;

    [SerializeField] private ProjectileStats projectileStats;

    private Transform spawnPoint;

    void Start()
    {
        spawnPoint = transform.GetChild(0);

        SpawnProjectiles(100, 0.3f, projectileStats);
    }

    /// <summary>
    /// Spawns a certain number of projectiles at certain intervals
    /// </summary>
    /// <param name="projectileNum"></param>
    /// <param name="delayBetween"></param>
    /// <param name="projectileSpeed"></param>
    void SpawnProjectiles(int projectileNum, float delayBetween, ProjectileStats stats)
    {
        StartCoroutine(SpawnPattern(projectileNum, delayBetween, stats));
    }

    IEnumerator SpawnPattern(int projectileNum, float delayBetween, ProjectileStats stats)
    {
        WaitForSeconds waitTime = new WaitForSeconds(delayBetween);

        for (int i = 0; i < projectileNum; i++)
        {
            //Create projectile instance
            Projectile instance = Instantiate(projectile, spawnPoint.position, Quaternion.identity).GetComponent<Projectile>();

            instance.SetDirection(spawnPoint.position);
            //Set Projectile speed
            instance.SetStats(stats);
            yield return waitTime;
        }
        yield return null;
    }
}

[System.Serializable]
public struct ProjectileStats
{
    public float Speed;
    public float LifeTime;

    public ProjectileStats(float speed, float lifeTime)
    {
        Speed = speed;
        LifeTime = lifeTime;
    }
}
