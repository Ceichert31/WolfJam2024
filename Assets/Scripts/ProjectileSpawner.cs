using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    //Current Projectile prefab
    [SerializeField] private GameObject projectile;

    [SerializeField] private ProjectileStats projectileStats;

    [SerializeField] private bool canRotate;
    [SerializeField] private float rotationAngle;
    void Start()
    {
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
            Projectile instance = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            stats.Direction = -transform.right;

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
    [HideInInspector] public Vector2 Direction;

    public ProjectileStats(float speed, float lifeTime, Vector2 direction)
    {
        Speed = speed;
        LifeTime = lifeTime;
        Direction = direction;
    }
}
